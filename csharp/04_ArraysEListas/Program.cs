/*
|--------------------------------------------------------------------------
| EXERCÍCIO 04 — Arrays, listas e dicionários
|--------------------------------------------------------------------------
|
| Mesmo problema do php/04_arrays.php — e é aqui que as duas linguagens mais
| se afastam. No PHP, um array faz tudo: lista, mapa, pilha. Em C# são tipos
| diferentes: array (tamanho fixo), List<T> (cresce) e Dictionary<K,V>.
|
| Conteúdo: List<T>, Dictionary<TKey, TValue>, foreach e os métodos do LINQ
| (Average, Min, Max, Where, ToList). Tente fazer cada um DUAS vezes: uma
| com laço na mão e outra com LINQ.
|
| 1) static double Media(double[] notas)
|    Média aritmética, arredondada para 2 casas. Array vazio devolve 0.0.
|
|    Media([8.0, 6.0, 10.0]) -> 8.0
|    Media([])               -> 0.0
|
| 2) static (int? Menor, int? Maior) MaiorEMenor(int[] numeros)
|    Devolve uma tupla. Array vazio devolve (null, null).
|
|    MaiorEMenor([3, 9, 1, 7]) -> (1, 9)
|
| 3) static List<int> ApenasPares(int[] numeros)
|    ApenasPares([1, 2, 3, 4, 5, 6]) -> [2, 4, 6]
|
| 4) static Dictionary<string, int> ContarOcorrencias(string[] palavras)
|    Sem diferenciar maiúsculas.
|
|    ContarOcorrencias(["obra", "Obra", "laje"]) -> {"obra": 2, "laje": 1}
|
| 5) static Dictionary<char, List<string>> AgruparPorInicial(string[] nomes)
|    AgruparPorInicial(["ana", "bruno", "alice"])
|      -> {'A': ["ana", "alice"], 'B': ["bruno"]}
|
| Rode com:  dotnet run --project 04_ArraysEListas
|
*/

Console.WriteLine("--- Média ---");
Console.WriteLine($"Media([8, 6, 10]) = {Media([8.0, 6.0, 10.0])}");

Console.WriteLine("\n--- Maior e menor ---");
var (menor, maior) = MaiorEMenor([3, 9, 1, 7]);
Console.WriteLine($"menor = {menor}, maior = {maior}");

Console.WriteLine("\n--- Apenas pares ---");
Console.WriteLine(string.Join(", ", ApenasPares([1, 2, 3, 4, 5, 6])));

Console.WriteLine("\n--- Ocorrências ---");

foreach (var par in ContarOcorrencias(["obra", "Obra", "laje"]))
{
    Console.WriteLine($"  {par.Key}: {par.Value}");
}

Console.WriteLine("\n--- Agrupado por inicial ---");

foreach (var grupo in AgruparPorInicial(["ana", "bruno", "alice", "carlos", "bia"]))
{
    Console.WriteLine($"  {grupo.Key}: {string.Join(", ", grupo.Value)}");
}

static double Media(double[] notas)
{
    // TODO: implemente
    return 0.0;
}

static (int? Menor, int? Maior) MaiorEMenor(int[] numeros)
{
    // TODO: implemente
    return (null, null);
}

static List<int> ApenasPares(int[] numeros)
{
    // TODO: implemente
    return [];
}

static Dictionary<string, int> ContarOcorrencias(string[] palavras)
{
    // TODO: implemente
    return [];
}

static Dictionary<char, List<string>> AgruparPorInicial(string[] nomes)
{
    // TODO: implemente
    return [];
}
