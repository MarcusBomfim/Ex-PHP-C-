/*
|--------------------------------------------------------------------------
| EXERCÍCIO 13 — Conjuntos
|--------------------------------------------------------------------------
|
| Mesmo problema do php/13_conjuntos.php.
|
| Um conjunto é uma coleção SEM repetição e SEM ordem. O PHP não tem o
| tipo: lá se usa array comum com array_unique, ou um array associativo
| cujas chaves são os elementos.
|
| O C# tem HashSet<T> pronto, e com os métodos de conjunto embutidos:
| UnionWith, IntersectWith, ExceptWith, SymmetricExceptWith, IsSubsetOf.
| Repare que todos terminam em "With" — eles alteram o conjunto NO LUGAR,
| e não devolvem um novo. O LINQ tem as versões que devolvem: Union,
| Intersect, Except.
|
| Cuidado que o exercício cobra: HashSet não tem ordem garantida. Se a
| saída precisa sair na ordem da primeira aparição, o HashSet sozinho não
| resolve — e é por isso que as assinaturas abaixo devolvem List<string>.
|
| 1) static List<string> SemRepetidos(IEnumerable<string> itens)
|    ["a","b","a","c","b"] -> ["a","b","c"]   (ordem da primeira aparição)
|
| 2) static List<string> Uniao(IEnumerable<string> a, IEnumerable<string> b)
| 3) static List<string> Intersecao(IEnumerable<string> a, IEnumerable<string> b)
| 4) static List<string> Diferenca(IEnumerable<string> a, IEnumerable<string> b)
|    O que está em `a` e não está em `b`.
| 5) static List<string> DiferencaSimetrica(IEnumerable<string> a, IEnumerable<string> b)
|    O que está em um ou no outro, mas não nos dois.
|
| 6) static bool ContemTodos(IEnumerable<string> conjunto, IEnumerable<string> itens)
|    Conjunto vazio cabe em qualquer um: ContemTodos(x, []) -> true
|
| 7) static Dictionary<string, int> MaisFrequentes(IEnumerable<string> itens, int quantos)
|    Do mais para o menos frequente.
|
| Um caso de uso real: comparar as ferramentas que saíram do almoxarifado
| com as que voltaram. A diferença é o que sumiu.
|
| Rode com:  dotnet run --project 13_Conjuntos
|
*/

static string Lista(IEnumerable<string> itens) => "[" + string.Join(", ", itens) + "]";

Console.WriteLine("--- Sem repetidos ---");
Console.WriteLine("  " + Lista(SemRepetidos(["a", "b", "a", "c", "b"])));

string[] saiu = ["martelo", "trena", "nível", "furadeira", "martelo"];
string[] voltou = ["trena", "martelo", "esquadro"];

Console.WriteLine("\n--- Almoxarifado ---");
Console.WriteLine("  saiu ...: " + Lista(saiu));
Console.WriteLine("  voltou .: " + Lista(voltou));
Console.WriteLine("  união ..: " + Lista(Uniao(saiu, voltou)));
Console.WriteLine("  nos dois: " + Lista(Intersecao(saiu, voltou)));
Console.WriteLine("  sumiu ..: " + Lista(Diferenca(saiu, voltou)));
Console.WriteLine("  a mais .: " + Lista(Diferenca(voltou, saiu)) + "  (voltou o que não saiu)");
Console.WriteLine("  simétr .: " + Lista(DiferencaSimetrica(saiu, voltou)));

Console.WriteLine("\n--- Contém todos ---");

string[] baseDeTeste = ["1", "2", "3"];

foreach (string[] procurados in new[] { new[] { "1", "3" }, ["1", "9"], [] })
{
    Console.WriteLine($"  {Lista(baseDeTeste)} contém {Lista(procurados)}: "
        + (ContemTodos(baseDeTeste, procurados) ? "sim" : "não"));
}

Console.WriteLine("\n--- Mais frequentes ---");

string[] apontamentos = ["pedreiro", "servente", "pedreiro", "carpinteiro", "pedreiro", "servente"];

foreach (var par in MaisFrequentes(apontamentos, 2))
{
    Console.WriteLine($"  {par.Key}: {par.Value}");
}

static List<string> SemRepetidos(IEnumerable<string> itens)
{
    // TODO: implemente — e repare que a ordem da primeira aparição importa
    return [];
}

static List<string> Uniao(IEnumerable<string> a, IEnumerable<string> b)
{
    // TODO: implemente
    return [];
}

static List<string> Intersecao(IEnumerable<string> a, IEnumerable<string> b)
{
    // TODO: implemente
    return [];
}

static List<string> Diferenca(IEnumerable<string> a, IEnumerable<string> b)
{
    // TODO: implemente
    return [];
}

static List<string> DiferencaSimetrica(IEnumerable<string> a, IEnumerable<string> b)
{
    // TODO: implemente
    return [];
}

static bool ContemTodos(IEnumerable<string> conjunto, IEnumerable<string> itens)
{
    // TODO: implemente
    return false;
}

static Dictionary<string, int> MaisFrequentes(IEnumerable<string> itens, int quantos)
{
    // TODO: implemente
    return [];
}
