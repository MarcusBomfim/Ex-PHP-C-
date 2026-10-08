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
    var resultado = new List<TSaida>();

    // É literalmente isto que o Select faz — com uma diferença: o Select é
    // PREGUIÇOSO. Ele não percorre nada até alguém iterar o resultado;
    // esta versão percorre na hora. Para 10 itens dá no mesmo; para uma
    // sequência infinita, só a do LINQ funciona.
    foreach (TEntrada item in itens)
    {
        resultado.Add(transformacao(item));
    }

    return resultado;
}

static List<T> Filtrar<T>(IEnumerable<T> itens, Func<T, bool> criterio)
{
    var resultado = new List<T>();

    foreach (T item in itens)
    {
        if (criterio(item))
        {
            resultado.Add(item);
        }
    }

    return resultado;
}

static TAcumulado Reduzir<T, TAcumulado>(
    IEnumerable<T> itens,
    Func<TAcumulado, T, TAcumulado> acumulador,
    TAcumulado inicial)
{
    TAcumulado acumulado = inicial;

    /*
     * O valor inicial não é detalhe: ele define o TIPO do resultado — que
     * pode ser diferente do tipo dos itens — e o que sai quando a lista é
     * vazia. Somar começa em 0, concatenar começa em "", multiplicar
     * começa em 1.
     *
     * O Aggregate do LINQ tem uma sobrecarga sem valor inicial, que usa o
     * primeiro item. Ela lança InvalidOperationException em lista vazia, e
     * é por isso que esta assinatura exige o inicial.
     */
    foreach (T item in itens)
    {
        acumulado = acumulador(acumulado, item);
    }

    return acumulado;
}

static Func<T, T> Compor<T>(Func<T, T> depois, Func<T, T> antes)
{
    /*
     * A ordem de leitura é de dentro para fora: `antes` roda primeiro, e o
     * resultado dele alimenta `depois`. É a mesma convenção da matemática,
     * onde (f ∘ g)(x) significa f(g(x)).
     */
    return valor => depois(antes(valor));
}

static Func<int> CriarContador(int inicio = 0)
{
    int proximo = inicio;

    /*
     * Aqui está a diferença para o PHP. Lá, esta closure precisaria
     * declarar `use (&$proximo)` — por referência — e com `use ($proximo)`
     * o contador travaria no valor inicial.
     *
     * Em C# a captura é automática e sempre por referência à VARIÁVEL, não
     * ao valor. O compilador move `proximo` para um objeto escondido que
     * sobrevive ao fim do método, e a lambda guarda a referência a ele.
     *
     * Cada chamada a CriarContador cria um objeto novo — é por isso que
     * dois contadores não compartilham estado.
     */
    return () => proximo++;
}

static Func<T, TResultado> Memoizar<T, TResultado>(Func<T, TResultado> funcao)
    where T : notnull
{
    var cache = new Dictionary<T, TResultado>();

    return entrada =>
    {
        /*
         * TryGetValue em vez de ContainsKey seguido de acesso: faz uma
         * busca só no dicionário em vez de duas.
         *
         * O `where T : notnull` na assinatura existe porque chave de
         * Dictionary não pode ser null — sem ele o compilador avisa.
         */
        if (!cache.TryGetValue(entrada, out TResultado? guardado))
        {
            guardado = funcao(entrada);
            cache[entrada] = guardado;
        }

        return guardado;
    };
}

static Dictionary<TChave, List<T>> AgruparPor<T, TChave>(IEnumerable<T> itens, Func<T, TChave> chave)
    where TChave : notnull
{
    var grupos = new Dictionary<TChave, List<T>>();

    foreach (T item in itens)
    {
        TChave k = chave(item);

        // A lista interna precisa ser criada na primeira vez. No PHP,
        // $grupos[$k][] = $item faz isso sozinho.
        if (!grupos.TryGetValue(k, out List<T>? doGrupo))
        {
            doGrupo = [];
            grupos[k] = doGrupo;
        }

        doGrupo.Add(item);
    }

    return grupos;
}
