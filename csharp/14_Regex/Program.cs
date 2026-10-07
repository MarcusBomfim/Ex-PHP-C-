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
    // TODO: implemente
    return false;
}

static List<int> ExtrairNumeros(string texto)
{
    // TODO: implemente
    return [];
}

static string MascararTelefone(string texto)
{
    // TODO: implemente
    return "";
}

static List<string> SepararPorPontuacao(string texto)
{
    // TODO: implemente
    return [];
}

static List<string> ProblemasDaSenha(string senha)
{
    // TODO: implemente
    return [];
}

static LinhaDeLog? LerLinhaDeLog(string linha)
{
    // TODO: implemente — use grupos nomeados: (?<nivel>ERRO|AVISO|INFO)
    return null;
}

public sealed record LinhaDeLog(string Data, string Nivel, string Mensagem);
