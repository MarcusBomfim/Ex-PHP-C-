/*
|--------------------------------------------------------------------------
| EXERCÍCIO 10 — Ordenação e busca
|--------------------------------------------------------------------------
|
| Mesmo problema do php/10_ordenacao.php.
|
| Conteúdo: OrderBy / ThenByDescending do LINQ, IComparable, Array.Sort,
| Array.BinarySearch e a diferença entre ordenação estável e instável.
|
| 1) static List<Produto> OrdenarPorPreco(IEnumerable<Produto> produtos, bool crescente = true)
| 2) static List<Produto> OrdenarPorCategoriaEPreco(IEnumerable<Produto> produtos)
|    Categoria crescente; dentro dela, preço decrescente.
| 3) static int BuscaBinaria(int[] ordenados, int alvo)   — implementada na mão
| 4) static List<Produto> MaisBaratos(IEnumerable<Produto> produtos, int quantos)
| 5) static SortedDictionary<string, List<string>> AgruparPorCategoria(...)
|
| Diferença grande para o PHP: lá, usort ordena NO LUGAR e é preciso copiar
| o array antes para não alterar o do chamador. Aqui o OrderBy do LINQ
| devolve uma sequência NOVA e nunca toca na original — o cuidado some.
| Em compensação, Array.Sort e List.Sort ordenam no lugar, igual ao PHP.
|
| Outra: o OrderBy do LINQ é ESTÁVEL (empates ficam na ordem original);
| Array.Sort e List.Sort NÃO são. Com vários critérios isso muda resultado.
|
| Rode com:  dotnet run --project 10_Ordenacao
|
*/

using System.Globalization;

var cultura = new CultureInfo("pt-BR");

Produto[] catalogo =
[
    new("Cimento CP-II 50 kg", "material", 38.90m),
    new("Trena 5 m", "ferramenta", 24.50m),
    new("Areia média m³", "material", 110.00m),
    new("Capacete", "epi", 32.00m),
    new("Nível a laser", "ferramenta", 289.90m),
    new("Luva de raspa", "epi", 18.75m),
];

void Mostrar(IEnumerable<Produto> produtos)
{
    foreach (Produto produto in produtos)
    {
        Console.WriteLine($"  {produto.Nome,-22}{produto.Categoria,-12}{produto.Preco.ToString("C", cultura),12}");
    }
}

Console.WriteLine("--- Por preço (crescente) ---");
Mostrar(OrdenarPorPreco(catalogo));

Console.WriteLine("\n--- Por preço (decrescente) ---");
Mostrar(OrdenarPorPreco(catalogo, crescente: false));

Console.WriteLine("\n--- O original não foi alterado ---");
Console.WriteLine($"  primeiro item ainda é: {catalogo[0].Nome}");

Console.WriteLine("\n--- Por categoria, e dentro dela do mais caro ---");
Mostrar(OrdenarPorCategoriaEPreco(catalogo));

Console.WriteLine("\n--- Os 3 mais baratos ---");
Mostrar(MaisBaratos(catalogo, 3));

Console.WriteLine("\n--- Agrupado por categoria ---");

foreach (var grupo in AgruparPorCategoria(catalogo))
{
    Console.WriteLine($"  {grupo.Key}: {string.Join(", ", grupo.Value)}");
}

Console.WriteLine("\n--- Busca binária ---");
int[] numeros = [1, 3, 5, 7, 9, 11, 13, 15];

foreach (int alvo in new[] { 7, 1, 15, 4 })
{
    int indice = BuscaBinaria(numeros, alvo);
    Console.WriteLine($"  procurando {alvo}: {(indice == -1 ? "não encontrado" : $"índice {indice}")}");
}

Console.WriteLine("\n--- A da biblioteca padrão concorda? ---");

foreach (int alvo in new[] { 7, 4 })
{
    // Array.BinarySearch devolve o complemento negativo da posição onde o
    // item DEVERIA estar quando não acha — por isso o < 0 em vez de == -1.
    int daBiblioteca = Array.BinarySearch(numeros, alvo);
    Console.WriteLine($"  {alvo}: minha = {BuscaBinaria(numeros, alvo)}, Array.BinarySearch = {daBiblioteca}");
}

static List<Produto> OrdenarPorPreco(IEnumerable<Produto> produtos, bool crescente = true)
{
    /*
     * OrderBy devolve uma sequência NOVA — a original fica intacta. É a
     * diferença para o usort do PHP, que ordena no lugar e obriga a copiar
     * antes.
     *
     * O ToList() no fim força a execução: sem ele o LINQ é preguiçoso e a
     * ordenação só aconteceria quando alguém percorresse o resultado.
     */
    return crescente
        ? produtos.OrderBy(p => p.Preco).ToList()
        : produtos.OrderByDescending(p => p.Preco).ToList();
}

static List<Produto> OrdenarPorCategoriaEPreco(IEnumerable<Produto> produtos)
{
    /*
     * ThenByDescending encadeia o segundo critério. É o equivalente do
     * `($a->categoria <=> $b->categoria) ?: ($b->preco <=> $a->preco)` do
     * PHP — com a vantagem de não precisar lembrar de inverter os
     * operandos para conseguir a ordem decrescente.
     */
    return produtos
        .OrderBy(p => p.Categoria, StringComparer.Ordinal)
        .ThenByDescending(p => p.Preco)
        .ToList();
}

static int BuscaBinaria(int[] ordenados, int alvo)
{
    int inicio = 0;
    int fim = ordenados.Length - 1;

    while (inicio <= fim)
    {
        /*
         * `inicio + (fim - inicio) / 2`, e não `(inicio + fim) / 2`.
         *
         * Com arrays grandes, a soma dos dois índices estoura o int e vira
         * número negativo — o acesso quebra. Foi um bug real na biblioteca
         * padrão do Java, descoberto só depois de nove anos.
         */
        int meio = inicio + (fim - inicio) / 2;

        if (ordenados[meio] == alvo)
        {
            return meio;
        }

        if (ordenados[meio] < alvo)
        {
            // O alvo só pode estar na metade de cima.
            inicio = meio + 1;
        }
        else
        {
            fim = meio - 1;
        }
    }

    return -1;
}

static List<Produto> MaisBaratos(IEnumerable<Produto> produtos, int quantos)
{
    // Take aceita número maior que a lista sem reclamar — devolve o que tem.
    return produtos.OrderBy(p => p.Preco).Take(Math.Max(0, quantos)).ToList();
}

static SortedDictionary<string, List<string>> AgruparPorCategoria(IEnumerable<Produto> produtos)
{
    /*
     * SortedDictionary mantém as chaves ordenadas sozinho — não é preciso
     * um ksort depois, como no PHP. O custo é que cada inserção é O(log n)
     * em vez de O(1); para listas pequenas não faz diferença.
     */
    var grupos = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);

    foreach (Produto produto in produtos)
    {
        if (!grupos.TryGetValue(produto.Categoria, out List<string>? nomes))
        {
            nomes = [];
            grupos[produto.Categoria] = nomes;
        }

        nomes.Add(produto.Nome);
    }

    return grupos;
}

/*
 * `record` em vez de `class`: gera construtor, propriedades só de leitura,
 * Equals, GetHashCode e ToString de graça. Para um objeto que só carrega
 * dados, é o que o C# moderno usa. O PHP não tem equivalente direto — lá
 * seria a classe com `public readonly` no construtor.
 */
public sealed record Produto(string Nome, string Categoria, decimal Preco);
