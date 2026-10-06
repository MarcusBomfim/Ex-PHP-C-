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
    // A guarda não é frescura: Average() num array vazio lança
    // InvalidOperationException, não devolve zero.
    if (notas.Length == 0)
    {
        return 0.0;
    }

    return Math.Round(notas.Average(), 2, MidpointRounding.AwayFromZero);
}

static (int? Menor, int? Maior) MaiorEMenor(int[] numeros)
{
    /*
     * Tupla nomeada: dá para devolver dois valores sem criar uma classe
     * só para isso. É o equivalente do array associativo que o PHP
     * devolve — com a diferença de que aqui os nomes e os tipos são
     * conferidos na compilação.
     */
    if (numeros.Length == 0)
    {
        return (null, null);
    }

    return (numeros.Min(), numeros.Max());
}

static List<int> ApenasPares(int[] numeros)
{
    // Versão LINQ. Na mão seria um foreach com if e lista.Add(n) —
    // vale escrever as duas e comparar.
    return numeros.Where(n => n % 2 == 0).ToList();
}

static Dictionary<string, int> ContarOcorrencias(string[] palavras)
{
    /*
     * StringComparer.OrdinalIgnoreCase no construtor: o próprio dicionário
     * passa a tratar "obra" e "Obra" como a mesma chave, e não é preciso
     * normalizar cada palavra antes. No PHP isso não existe — lá a chave
     * tem que ser passada já em minúsculas.
     */
    var contagem = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    foreach (string palavra in palavras)
    {
        // CollectionsMarshal à parte, este é o jeito legível: pega o atual
        // (0 se não existe) e grava o incrementado.
        contagem.TryGetValue(palavra, out int atual);
        contagem[palavra] = atual + 1;
    }

    return contagem;
}

static Dictionary<char, List<string>> AgruparPorInicial(string[] nomes)
{
    var grupos = new Dictionary<char, List<string>>();

    foreach (string nome in nomes)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            continue;
        }

        char inicial = char.ToUpperInvariant(nome[0]);

        /*
         * Aqui está a diferença mais visível para o PHP. Lá,
         * $grupos[$inicial][] = $nome cria o array interno sozinho. Em C#
         * é preciso criar a lista na primeira vez — senão a chave não
         * existe e o acesso lança KeyNotFoundException.
         */
        if (!grupos.TryGetValue(inicial, out List<string>? doGrupo))
        {
            doGrupo = [];
            grupos[inicial] = doGrupo;
        }

        doGrupo.Add(nome);
    }

    return grupos;
}
