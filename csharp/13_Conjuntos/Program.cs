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
    /*
     * Distinct do LINQ, e não new HashSet<string>(itens).ToList().
     *
     * Os dois tiram as repetições, mas só o Distinct garante a ORDEM da
     * primeira aparição. O HashSet não promete ordem nenhuma — hoje ele
     * costuma sair na ordem de inserção, e isso é detalhe de
     * implementação, não contrato.
     *
     * É a mesma razão de todas as funções aqui devolverem List<string> em
     * vez de HashSet<string>.
     */
    return itens.Distinct().ToList();
}

static List<string> Uniao(IEnumerable<string> a, IEnumerable<string> b)
{
    // Union já tira as repetições dos dois lados — não precisa de Distinct
    // depois. A ordem fica: os de `a`, depois os de `b` que são novos.
    return a.Union(b).ToList();
}

static List<string> Intersecao(IEnumerable<string> a, IEnumerable<string> b)
{
    // Intersect também já devolve sem repetição, mesmo que `a` traga o
    // mesmo item duas vezes.
    return a.Intersect(b).ToList();
}

static List<string> Diferenca(IEnumerable<string> a, IEnumerable<string> b)
{
    /*
     * Except, e não ExceptWith.
     *
     * Os métodos do HashSet terminados em "With" alteram o conjunto NO
     * LUGAR e devolvem void; os do LINQ devolvem uma sequência nova e não
     * tocam na original. Num método que recebe IEnumerable de fora, mexer
     * no que chegou seria efeito colateral escondido.
     */
    return a.Except(b).ToList();
}

static List<string> DiferencaSimetrica(IEnumerable<string> a, IEnumerable<string> b)
{
    // "O que só está em A" mais "o que só está em B". Montada a partir das
    // operações acima em vez de refazer a conta.
    return a.Except(b).Union(b.Except(a)).ToList();
}

static bool ContemTodos(IEnumerable<string> conjunto, IEnumerable<string> itens)
{
    /*
     * IsSupersetOf é a pergunta exata, e o caso do conjunto vazio sai de
     * graça: todo conjunto contém o vazio, que é a definição matemática.
     *
     * A alternativa `itens.All(conjunto.Contains)` dá o mesmo resultado,
     * mas percorre a lista inteira a cada item — O(n×m). O HashSet
     * resolve em O(n+m).
     */
    return new HashSet<string>(conjunto).IsSupersetOf(itens);
}

static Dictionary<string, int> MaisFrequentes(IEnumerable<string> itens, int quantos)
{
    /*
     * GroupBy agrupa pelo próprio valor; Count() conta cada grupo.
     *
     * Dictionary<K,V> não garante ordem de iteração — na prática ele
     * costuma sair na ordem de inserção, e é com isso que esta função
     * conta. Se a ordem fosse um requisito firme, o certo seria devolver
     * uma List<(string, int)>.
     */
    return itens
        .GroupBy(item => item)
        .OrderByDescending(grupo => grupo.Count())
        .Take(Math.Max(0, quantos))
        .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());
}
