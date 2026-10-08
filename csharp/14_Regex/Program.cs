/*
|--------------------------------------------------------------------------
| EXERCÍCIO 14 — Expressões regulares
|--------------------------------------------------------------------------
|
| Mesmo problema do php/14_regex.php.
|
| Conteúdo: Regex.IsMatch, Regex.Matches, Regex.Replace, Regex.Split,
| grupos de captura e grupos nomeados.
|
| Aviso que vale para o exercício inteiro: regex é ótima para RECONHECER
| formato e péssima para VALIDAR significado. "000.000.000-00" passa em
| qualquer regex de CPF e não é um CPF válido — os dígitos verificadores
| são conta, não padrão.
|
| Diferenças para o PHP:
|
| - Lá a regex é string com delimitador: '/^\d{3}$/'. Aqui é string normal,
|   sem delimitador, e vale usar @"..." (verbatim) para não ter que
|   escapar cada barra invertida duas vezes.
| - Compilar a regex a cada chamada custa caro. Em C#, o padrão é um campo
|   `static readonly Regex` — ou, melhor ainda, o [GeneratedRegex] do .NET 7+,
|   que gera o código em tempo de compilação. O PHP guarda um cache interno
|   e o problema é menor.
| - Grupo nomeado: (?<nivel>...) nos dois, mas aqui se lê por
|   `m.Groups["nivel"].Value`.
|
| 1) static bool TemFormatoDeCpf(string texto)
|    Aceita exatamente 000.000.000-00.
|
|    "123.456.789-09"  -> true
|    "12345678909"     -> false  (sem pontuação)
|    "123.456.789-091" -> false  (sobrou dígito)
|
|    Dica: sem ^ e $ a regex casa um pedaço no meio de qualquer texto — e
|    é por isso que a terceira linha acima costuma passar por engano.
|
| 2) static List<int> ExtrairNumeros(string texto)
|    "laje 3, pilar 12 e 7 vigas" -> [3, 12, 7]
|
| 3) static string MascararTelefone(string texto)
|    (11) 91234-5678 -> (11) *****-5678
|
| 4) static List<string> SepararPorPontuacao(string texto)
|    "cimento, areia;  brita." -> ["cimento","areia","brita"]
|    Sem itens vazios no resultado.
|
| 5) static List<string> ProblemasDaSenha(string senha)
|    Lista o que falta, em vez de só dizer "senha fraca":
|      "ao menos 8 caracteres" / "ao menos uma letra maiúscula" /
|      "ao menos uma letra minúscula" / "ao menos um número" /
|      "ao menos um símbolo"
|    Senha boa devolve lista vazia.
|
| 6) static LinhaDeLog? LerLinhaDeLog(string linha)
|    De "[2026-10-06 14:32:01] ERRO Falha ao gravar o diário" extrai
|    data, nível e mensagem. Linha fora do formato devolve null.
|
| Rode com:  dotnet run --project 14_Regex
|
*/

using System.Text.RegularExpressions;

Console.WriteLine("--- Formato de CPF ---");

foreach (string texto in new[] { "123.456.789-09", "12345678909", "123.456.789-091", "abc.def.ghi-jk" })
{
    Console.WriteLine($"  {texto,-18} {(TemFormatoDeCpf(texto) ? "formato ok" : "formato inválido")}");
}

Console.WriteLine("\n--- Extrair números ---");
Console.WriteLine("  " + string.Join(", ", ExtrairNumeros("laje 3, pilar 12 e 7 vigas")));

Console.WriteLine("\n--- Mascarar telefone ---");
Console.WriteLine("  " + MascararTelefone("Ligar para (11) 91234-5678 ou (13) 99876-5432."));

Console.WriteLine("\n--- Separar por pontuação ---");
Console.WriteLine("  [" + string.Join(" | ", SepararPorPontuacao("cimento, areia;  brita.")) + "]");

Console.WriteLine("\n--- Força da senha ---");

foreach (string senha in new[] { "abc", "Senha123", "Senha@123" })
{
    List<string> problemas = ProblemasDaSenha(senha);
    Console.WriteLine($"  \"{senha}\": " + (problemas.Count == 0 ? "ok" : string.Join("; ", problemas)));
}

Console.WriteLine("\n--- Linha de log ---");

string[] linhas =
[
    "[2026-10-06 14:32:01] ERRO Falha ao gravar o diário",
    "[2026-10-06 14:35:40] INFO Medição 7 fechada",
    "isto não é uma linha de log",
];

foreach (string linha in linhas)
{
    LinhaDeLog? partes = LerLinhaDeLog(linha);

    if (partes is null)
    {
        Console.WriteLine($"  (fora do formato) {linha}");
        continue;
    }

    Console.WriteLine($"  {partes.Data} [{partes.Nivel}] {partes.Mensagem}");
}

static bool TemFormatoDeCpf(string texto)
{
    /*
     * O ^ e o $ são a parte que mais se esquece. Sem eles, a regex casa um
     * PEDAÇO de qualquer texto: "123.456.789-091" passaria, porque os 14
     * primeiros caracteres formam o padrão e o "1" sobrando é ignorado.
     */
    return Padroes.Cpf().IsMatch(texto);
}

static List<int> ExtrairNumeros(string texto)
{
    // Matches devolve a coleção de casamentos; cada um tem o texto casado
    // em .Value. O int.Parse é seguro aqui porque a regex só casou dígitos.
    return Padroes.Numero()
        .Matches(texto)
        .Select(m => int.Parse(m.Value))
        .ToList();
}

static string MascararTelefone(string texto)
{
    /*
     * Os parênteses do telefone precisam ser escapados — \( e \) —, porque
     * parêntese solto em regex abre grupo de captura.
     *
     * Na substituição, $1 e $2 são os grupos capturados: o DDD e os quatro
     * últimos dígitos. O miolo some atrás dos asteriscos.
     */
    return Padroes.Telefone().Replace(texto, "($1) *****-$2");
}

static List<string> SepararPorPontuacao(string texto)
{
    /*
     * O + depois da classe é o que resolve os "espaços repetidos": ele faz
     * a regex consumir a sequência inteira de separadores de uma vez. Sem
     * ele, "areia;  brita" produziria itens vazios no meio.
     *
     * O C# não tem o PREG_SPLIT_NO_EMPTY do PHP — o Where no fim faz esse
     * papel, descartando o que sobra nas pontas.
     */
    return Padroes.Pontuacao()
        .Split(texto.Trim())
        .Where(parte => !string.IsNullOrEmpty(parte))
        .ToList();
}

static List<string> ProblemasDaSenha(string senha)
{
    var problemas = new List<string>();

    if (senha.Length < 8)
    {
        problemas.Add("ao menos 8 caracteres");
    }

    /*
     * Para estas quatro, os métodos de char batem a regex: são mais
     * rápidos, mais legíveis e não precisam ser compilados.
     *
     * "Símbolo" definido pela negativa — o que não é letra nem dígito.
     * Listar os símbolos aceitos deixaria de fora os que ninguém lembrou.
     */
    if (!senha.Any(char.IsUpper))
    {
        problemas.Add("ao menos uma letra maiúscula");
    }

    if (!senha.Any(char.IsLower))
    {
        problemas.Add("ao menos uma letra minúscula");
    }

    if (!senha.Any(char.IsDigit))
    {
        problemas.Add("ao menos um número");
    }

    if (!senha.Any(c => !char.IsLetterOrDigit(c)))
    {
        problemas.Add("ao menos um símbolo");
    }

    return problemas;
}

static LinhaDeLog? LerLinhaDeLog(string linha)
{
    Match casamento = Padroes.LinhaDeLog().Match(linha);

    if (!casamento.Success)
    {
        return null;
    }

    /*
     * Grupos NOMEADOS, lidos por Groups["nome"].
     *
     * A alternativa é Groups[1], [2], [3] — e aí basta alguém inserir um
     * grupo no meio da regex para todos os índices de baixo saírem do
     * lugar em silêncio. Com nome, inserir grupo não quebra nada.
     */
    return new LinhaDeLog(
        casamento.Groups["data"].Value,
        casamento.Groups["nivel"].Value,
        casamento.Groups["mensagem"].Value
    );
}

public sealed record LinhaDeLog(string Data, string Nivel, string Mensagem);

/*
 * As regexes num lugar só, com [GeneratedRegex].
 *
 * Esse atributo faz o compilador GERAR o código de casamento em tempo de
 * compilação, em vez de interpretar o padrão a cada execução. É mais
 * rápido que `new Regex(...)` e muito mais rápido que os métodos estáticos
 * `Regex.IsMatch(texto, padrao)`, que recompilam o padrão toda vez.
 *
 * No PHP não há equivalente: o preg_* mantém um cache interno de padrões
 * já compilados, e o problema é menor — mas também não há como garantir
 * que o padrão está certo antes de rodar. Aqui, um erro de sintaxe na
 * regex é erro de COMPILAÇÃO.
 *
 * O @"..." é string verbatim: dentro dela a barra invertida é literal, e
 * não é preciso escrever \\d para dizer \d.
 */
internal static partial class Padroes
{
    [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")]
    public static partial Regex Cpf();

    [GeneratedRegex(@"\d+")]
    public static partial Regex Numero();

    [GeneratedRegex(@"\((\d{2})\)\s*\d{5}-(\d{4})")]
    public static partial Regex Telefone();

    [GeneratedRegex(@"[\s,;.]+")]
    public static partial Regex Pontuacao();

    [GeneratedRegex(@"^\[(?<data>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\] (?<nivel>ERRO|AVISO|INFO) (?<mensagem>.+)$")]
    public static partial Regex LinhaDeLog();
}
