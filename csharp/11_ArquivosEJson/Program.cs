/*
|--------------------------------------------------------------------------
| EXERCÍCIO 11 — Arquivos e JSON
|--------------------------------------------------------------------------
|
| Mesmo problema do php/11_arquivos_e_json.php.
|
| Conteúdo: File.WriteAllText, File.ReadAllLines, System.Text.Json,
| JsonSerializerOptions, `using` e por que ele substitui o finally.
|
| 1) static void SalvarTexto(string caminho, string conteudo)
| 2) static List<string> LerLinhas(string caminho)   — sem as vazias
| 3) static void SalvarComoJson<T>(string caminho, T dados)   — indentado, com acento
| 4) static T LerJson<T>(string caminho)             — lança se inválido
| 5) static List<Dictionary<string, string>> LerCsv(string caminho)
| 6) static void AnexarLinha(string caminho, string linha)
|
| Diferenças para o PHP que aparecem aqui:
|
| - Lá é preciso conferir o retorno de file_put_contents, que devolve false
|   em caso de falha. Aqui File.WriteAllText LANÇA — não dá para ignorar
|   sem querer.
| - O JSON do C# é tipado: LerJson<Obra> devolve um objeto Obra, não um
|   array associativo. Campo que não existe vira erro de compilação, não
|   um null descoberto em produção.
| - `using var` fecha o arquivo sozinho no fim do bloco, inclusive se
|   lançar. É o finally do PHP, escrito pelo compilador.
|
| Rode com:  dotnet run --project 11_ArquivosEJson
|
*/

using System.Text.Json;
using System.Text.Json.Serialization;

string pasta = Path.Combine(Path.GetTempPath(), "ex-php-csharp-" + Guid.NewGuid().ToString("N")[..8]);
Directory.CreateDirectory(pasta);

try
{
    Console.WriteLine("--- Texto ---");
    string texto = Path.Combine(pasta, "anotacoes.txt");

    SalvarTexto(texto, "primeira linha\nsegunda linha\n");
    AnexarLinha(texto, "terceira linha");

    List<string> linhas = LerLinhas(texto);

    for (int i = 0; i < linhas.Count; i++)
    {
        Console.WriteLine($"  {i + 1}: {linhas[i]}");
    }

    Console.WriteLine("\n--- JSON ---");
    string json = Path.Combine(pasta, "obra.json");

    var obra = new Obra("OBR-2026-007", "Edifício Vista Serra", 6, ["laje", "pilar", "viga"]);
    SalvarComoJson(json, obra);

    Console.WriteLine(File.ReadAllText(json));

    Obra lida = LerJson<Obra>(json);
    Console.WriteLine($"Lido de volta: {lida.Nome} com {lida.Concretagens} concretagens");
    Console.WriteLine($"Elementos: {string.Join(", ", lida.Elementos)}");

    Console.WriteLine("\n--- CSV ---");
    string csv = Path.Combine(pasta, "equipe.csv");

    SalvarTexto(csv, "nome,funcao,horas\nAna,Engenheira,180\nBruno,Pedreiro,200\nCarla,Mestre,190\n");

    foreach (var pessoa in LerCsv(csv))
    {
        Console.WriteLine($"  {pessoa["nome"],-8} {pessoa["funcao"],-12} {pessoa["horas"]} h");
    }

    Console.WriteLine("\n--- O que deve dar erro ---");

    try
    {
        LerJson<Obra>(Path.Combine(pasta, "nao-existe.json"));
        Console.WriteLine("ERRO: leu arquivo inexistente!");
    }
    catch (FileNotFoundException erro)
    {
        Console.WriteLine($"Recusado, como esperado: {erro.Message}");
    }

    string quebrado = Path.Combine(pasta, "quebrado.json");
    SalvarTexto(quebrado, "{ isto não é json }");

    try
    {
        LerJson<Obra>(quebrado);
        Console.WriteLine("ERRO: aceitou JSON inválido!");
    }
    catch (JsonException erro)
    {
        Console.WriteLine($"Recusado, como esperado: {erro.Message.Split('.')[0]}.");
    }
}
finally
{
    // Limpeza: o exercício não deixa arquivo para trás.
    Directory.Delete(pasta, recursive: true);
    Console.WriteLine("\n(arquivos temporários removidos)");
}

static void SalvarTexto(string caminho, string conteudo)
{
    /*
     * Sem conferência de retorno, e de propósito: File.WriteAllText não
     * devolve nada — ela LANÇA se falhar (IOException,
     * UnauthorizedAccessException...).
     *
     * É a diferença para o PHP, onde file_put_contents devolve false e o
     * programa segue achando que gravou. Aqui o erro não passa batido.
     */
    File.WriteAllText(caminho, conteudo);
}

static List<string> LerLinhas(string caminho)
{
    if (!File.Exists(caminho))
    {
        throw new FileNotFoundException($"Arquivo não encontrado: {caminho}.", caminho);
    }

    return File.ReadAllLines(caminho)
        .Where(linha => !string.IsNullOrWhiteSpace(linha))
        .ToList();
}

static void SalvarComoJson<T>(string caminho, T dados)
{
    /*
     * WriteIndented  — indentado, para dar para ler e versionar.
     * Encoder relaxado — sem ele, "Edifício" vira "Edifício". É o
     *                    equivalente do JSON_UNESCAPED_UNICODE do PHP.
     *
     * O serializador é estático e reutilizável; criar as opções a cada
     * chamada é desperdício num laço, mas aqui a clareza vale mais.
     */
    var opcoes = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    SalvarTexto(caminho, JsonSerializer.Serialize(dados, opcoes));
}

static T LerJson<T>(string caminho)
{
    if (!File.Exists(caminho))
    {
        throw new FileNotFoundException($"Arquivo não encontrado: {caminho}.", caminho);
    }

    string conteudo = File.ReadAllText(caminho);

    /*
     * Deserialize<T> devolve T?, podendo ser null quando o JSON é
     * literalmente "null". O `??` transforma isso em exceção em vez de
     * deixar um null escapar para o resto do programa.
     *
     * Repare no ganho sobre o PHP: aqui o resultado é um objeto Obra, com
     * campos conferidos pelo compilador. Lá é um array associativo, e
     * $dados['nomo'] (com erro de digitação) só falha em produção.
     */
    return JsonSerializer.Deserialize<T>(conteudo)
        ?? throw new JsonException($"O conteúdo de {caminho} não produziu um objeto.");
}

static List<Dictionary<string, string>> LerCsv(string caminho)
{
    /*
     * `using var` em vez de try/finally: o leitor é fechado no fim do
     * método, inclusive se alguma linha lançar no meio. O compilador
     * escreve o finally sozinho.
     *
     * Arquivo aberto e não fechado é descritor vazando — num processo de
     * longa duração acaba em "too many open files".
     */
    using var leitor = new StreamReader(caminho);

    string? cabecalhoBruto = leitor.ReadLine();

    if (cabecalhoBruto is null)
    {
        return [];
    }

    string[] cabecalho = cabecalhoBruto.Split(',');
    var linhas = new List<Dictionary<string, string>>();

    while (leitor.ReadLine() is { } linha)
    {
        if (string.IsNullOrWhiteSpace(linha))
        {
            continue;
        }

        string[] campos = linha.Split(',');
        var registro = new Dictionary<string, string>();

        for (int i = 0; i < cabecalho.Length && i < campos.Length; i++)
        {
            registro[cabecalho[i]] = campos[i];
        }

        linhas.Add(registro);
    }

    return linhas;
}

static void AnexarLinha(string caminho, string linha)
{
    // AppendAllText escreve no fim em vez de truncar. Para concorrência de
    // verdade seria preciso um FileStream com FileShare controlado — o
    // LOCK_EX do PHP não tem equivalente direto nesta chamada.
    File.AppendAllText(caminho, linha + Environment.NewLine);
}

/*
 * O JSON tipado: os nomes das propriedades viram chaves do arquivo, e o
 * JsonPropertyName controla como. Sem ele, "Codigo" sairia com C maiúsculo.
 */
public sealed record Obra(
    [property: JsonPropertyName("codigo")] string Codigo,
    [property: JsonPropertyName("nome")] string Nome,
    [property: JsonPropertyName("concretagens")] int Concretagens,
    [property: JsonPropertyName("elementos")] IReadOnlyList<string> Elementos
);
