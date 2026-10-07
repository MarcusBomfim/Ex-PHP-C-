/*
|--------------------------------------------------------------------------
| EXERCÍCIO 15 — Funções de ordem superior e closures
|--------------------------------------------------------------------------
|
| Mesmo problema do php/15_funcoes_de_ordem_superior.php.
|
| Conteúdo: Func<>, Action<>, lambdas, closures e captura de variável.
|
| Função de ordem superior é a que recebe outra função como argumento, ou
| devolve uma. `OrderBy` é uma: recebe o seletor. Este exercício é sobre
| escrever as suas.
|
| Nos três primeiros, IMPLEMENTE À MÃO — nada de Select, Where ou
| Aggregate. O objetivo é entender o que o LINQ faz por dentro.
|
| Diferença grande para o PHP: lá, a closure precisa declarar o que captura
| com `use ($x)` — por valor — ou `use (&$x)` — por referência. Em C# a
| captura é automática e SEMPRE por referência à variável. Por isso o
| contador do item 5, que no PHP exige `use (&$n)`, aqui funciona sem
| cerimônia nenhuma.
|
| O outro lado da moeda: essa captura automática é a origem do bug clássico
| do laço. Antes do C# 5, capturar a variável do `for` dava a todas as
| closures o MESMO valor final. Vale testar o que acontece hoje com `for`
| e com `foreach` — eles não se comportam igual.
|
| 1) static List<TSaida> AplicarEmTodos<TEntrada, TSaida>(
|        IEnumerable<TEntrada> itens, Func<TEntrada, TSaida> transformacao)
|    AplicarEmTodos([1,2,3], n => n * 2) -> [2, 4, 6]
|
| 2) static List<T> Filtrar<T>(IEnumerable<T> itens, Func<T, bool> criterio)
|    Filtrar([1,2,3,4], n => n % 2 == 0) -> [2, 4]
|
| 3) static TAcumulado Reduzir<T, TAcumulado>(
|        IEnumerable<T> itens, Func<TAcumulado, T, TAcumulado> acumulador, TAcumulado inicial)
|    Reduzir([1,2,3], (t, n) => t + n, 0) -> 6
|
| 4) static Func<T, T> Compor<T>(Func<T, T> depois, Func<T, T> antes)
|    var f = Compor<int>(n => n + 1, n => n * 2);
|    f(5) -> 11    (5*2 = 10, depois +1)
|
| 5) static Func<int> CriarContador(int inicio = 0)
|    Cada chamada devolve o próximo número. Dois contadores criados
|    separadamente NÃO compartilham estado.
|
| 6) static Func<T, TResultado> Memoizar<T, TResultado>(Func<T, TResultado> funcao)
|    Guarda os resultados já calculados. (T precisa de `where T : notnull`
|    para servir de chave de dicionário.)
|
| 7) static Dictionary<TChave, List<T>> AgruparPor<T, TChave>(
|        IEnumerable<T> itens, Func<T, TChave> chave)
|
| Rode com:  dotnet run --project 15_FuncoesDeOrdemSuperior
|
*/

using System.Diagnostics;

static string Lista<T>(IEnumerable<T> itens) => "[" + string.Join(", ", itens) + "]";

Console.WriteLine("--- Aplicar em todos ---");
Console.WriteLine("  dobrar [1,2,3] = " + Lista(AplicarEmTodos([1, 2, 3], n => n * 2)));
Console.WriteLine("  nomes em maiúsculas = " + Lista(AplicarEmTodos(["ana", "bruno"], s => s.ToUpperInvariant())));

Console.WriteLine("\n--- Filtrar ---");
Console.WriteLine("  pares de [1,2,3,4,5,6] = " + Lista(Filtrar([1, 2, 3, 4, 5, 6], n => n % 2 == 0)));

Console.WriteLine("\n--- Reduzir ---");
Console.WriteLine("  soma de [1,2,3,4] = " + Reduzir([1, 2, 3, 4], (int t, int n) => t + n, 0));
Console.WriteLine("  juntar [\"a\",\"b\",\"c\"] = \"" + Reduzir(["a", "b", "c"], (string t, string s) => t + s, "") + "\"");
Console.WriteLine("  maior de [3,9,1] = " + Reduzir([3, 9, 1], (int t, int n) => Math.Max(t, n), 0));

Console.WriteLine("\n--- Composição ---");
Func<int, int> dobrarEIncrementar = Compor<int>(n => n + 1, n => n * 2);
Console.WriteLine($"  Compor(+1, *2)(5) = {dobrarEIncrementar(5)}   (5*2 = 10, depois +1)");

Console.WriteLine("\n--- Contador com estado ---");
Func<int> primeiro = CriarContador();
Func<int> segundo = CriarContador(100);

Console.WriteLine($"  primeiro: {primeiro()}, {primeiro()}, {primeiro()}");
Console.WriteLine($"  segundo : {segundo()}, {segundo()}");
Console.WriteLine($"  primeiro de novo: {primeiro()}   (os dois não se misturam)");

Console.WriteLine("\n--- Memoização ---");
int vezesChamada = 0;

Func<int, int> lenta = n =>
{
    vezesChamada++;
    Thread.Sleep(1);

    return n * n;
};

Func<int, int> rapida = Memoizar(lenta);

Console.WriteLine($"  rapida(9) = {rapida(9)}");
Console.WriteLine($"  rapida(9) = {rapida(9)}");
Console.WriteLine($"  rapida(4) = {rapida(4)}");
Console.WriteLine($"  a função original foi chamada {vezesChamada} vez(es) — deveria ser 2");

Console.WriteLine("\n--- Agrupar por ---");

foreach (var grupo in AgruparPor(["ana", "bruno", "alice", "carlos", "bia"], n => n[0]))
{
    Console.WriteLine($"  {grupo.Key}: {string.Join(", ", grupo.Value)}");
}

Console.WriteLine("\n--- A pegadinha da captura ---");
Console.WriteLine("  Experimento: criar 3 closures dentro de um laço e chamá-las depois.");

var deFor = new List<Func<int>>();

for (int i = 0; i < 3; i++)
{
    deFor.Add(() => i);
}

var deForeach = new List<Func<int>>();

foreach (int i in new[] { 0, 1, 2 })
{
    deForeach.Add(() => i);
}

Console.WriteLine($"  com for .....: {Lista(deFor.Select(f => f()))}");
Console.WriteLine($"  com foreach .: {Lista(deForeach.Select(f => f()))}");
Console.WriteLine("  Por que são diferentes? A variável do `for` é UMA só, compartilhada");
Console.WriteLine("  por todas as closures; a do `foreach` é nova a cada volta.");

static List<TSaida> AplicarEmTodos<TEntrada, TSaida>(
    IEnumerable<TEntrada> itens,
    Func<TEntrada, TSaida> transformacao)
{
    // TODO: implemente à mão, com foreach — sem Select
    return [];
}

static List<T> Filtrar<T>(IEnumerable<T> itens, Func<T, bool> criterio)
{
    // TODO: implemente à mão, com foreach — sem Where
    return [];
}

static TAcumulado Reduzir<T, TAcumulado>(
    IEnumerable<T> itens,
    Func<TAcumulado, T, TAcumulado> acumulador,
    TAcumulado inicial)
{
    // TODO: implemente à mão, com foreach — sem Aggregate
    return inicial;
}

static Func<T, T> Compor<T>(Func<T, T> depois, Func<T, T> antes)
{
    // TODO: devolva uma lambda que encadeia as duas
    return valor => valor;
}

static Func<int> CriarContador(int inicio = 0)
{
    // TODO: devolva uma lambda com estado próprio
    return () => 0;
}

static Func<T, TResultado> Memoizar<T, TResultado>(Func<T, TResultado> funcao)
    where T : notnull
{
    // TODO: devolva uma versão que guarda os resultados
    return funcao;
}

static Dictionary<TChave, List<T>> AgruparPor<T, TChave>(IEnumerable<T> itens, Func<T, TChave> chave)
    where TChave : notnull
{
    // TODO: implemente
    return [];
}
