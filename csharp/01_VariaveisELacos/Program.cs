/*
|--------------------------------------------------------------------------
| EXERCÍCIO 01 — Variáveis, operadores e laços
|--------------------------------------------------------------------------
|
| Mesmo problema do php/01_variaveis_e_lacos.php. Compare os dois no fim.
|
| Conteúdo: tipos (int, double, string), interpolação ($"..."), laço `for`,
| Math.Round e Console.WriteLine.
|
| 1) static string[] Tabuada(int numero)
|    Array com as 10 linhas da tabuada, no formato "3 x 1 = 3".
|
|    Tabuada(3)[0]  ->  "3 x 1 = 3"
|    Tabuada(3)[9]  ->  "3 x 10 = 30"
|
| 2) static int SomaAte(int limite)
|    Soma os inteiros de 1 até `limite` (inclusive).
|
|    SomaAte(5) -> 15
|    SomaAte(0) -> 0
|
| 3) static double PrecoComDesconto(double preco, double percentual)
|    Desconto percentual, arredondado para 2 casas (veja Math.Round).
|
|    PrecoComDesconto(100.0, 10.0) -> 90.0
|    PrecoComDesconto(59.9, 15.0)  -> 50.92
|
| Diferença para o PHP que vale notar: lá o array cresce com []; aqui você
| precisa decidir o tamanho na criação (new string[10]) ou usar List<string>
| e chamar ToArray() no fim.
|
| Rode com:  dotnet run --project 01_VariaveisELacos
|
*/

Console.WriteLine("--- Tabuada do 7 ---");

foreach (string linha in Tabuada(7))
{
    Console.WriteLine(linha);
}

Console.WriteLine("\n--- Soma ---");
Console.WriteLine($"SomaAte(5)   = {SomaAte(5)}");
Console.WriteLine($"SomaAte(100) = {SomaAte(100)}");

Console.WriteLine("\n--- Desconto ---");
Console.WriteLine($"R$ 100,00 com 10% = {PrecoComDesconto(100.0, 10.0)}");
Console.WriteLine($"R$ 59,90 com 15%  = {PrecoComDesconto(59.9, 15.0)}");

static string[] Tabuada(int numero)
{
    /*
     * Aqui o tamanho é conhecido — são sempre 10 linhas —, então o array
     * de tamanho fixo serve e evita a realocação que a List faria.
     *
     * É a diferença para o PHP: lá, $linhas[] = ... faz o array crescer
     * sozinho porque todo array do PHP é, por baixo, um mapa ordenado.
     */
    string[] linhas = new string[10];

    for (int i = 1; i <= 10; i++)
    {
        linhas[i - 1] = $"{numero} x {i} = {numero * i}";
    }

    return linhas;
}

static int SomaAte(int limite)
{
    int soma = 0;

    // Com limite 0 (ou negativo) o laço não entra e a soma fica em 0.
    for (int i = 1; i <= limite; i++)
    {
        soma += i;
    }

    return soma;
}

static double PrecoComDesconto(double preco, double percentual)
{
    /*
     * MidpointRounding.AwayFromZero de propósito. O padrão do .NET é
     * "para o par mais próximo" (banker's rounding): Math.Round(2.5) dá 2,
     * não 3. É ótimo para estatística, porque não vicia a média para cima —
     * e é péssimo para preço, porque ninguém espera que R$ 2,50 vire R$ 2.
     */
    return Math.Round(preco * (1 - percentual / 100), 2, MidpointRounding.AwayFromZero);
}
