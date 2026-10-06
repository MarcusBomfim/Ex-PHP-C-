/*
|--------------------------------------------------------------------------
| EXERCÍCIO 05 — Métodos e recursão
|--------------------------------------------------------------------------
|
| Mesmo problema do php/05_funcoes_e_recursao.php.
|
| Conteúdo: parâmetros com valor padrão, recursão, caso-base, e o
| StackOverflowException que aparece quando ele falta.
|
| Todos abaixo devem ser RECURSIVOS — sem for/while.
|
| 1) static long Fatorial(int n)
|    Fatorial(0) -> 1    (caso-base)
|    Fatorial(5) -> 120
|    É `long` e não `int` de propósito: fatorial estoura int já no 13.
|
| 2) static long Fibonacci(int n)
|    A sequência começa em 0: 0, 1, 1, 2, 3, 5, 8, 13...
|
|    Fibonacci(0) -> 0
|    Fibonacci(1) -> 1
|    Fibonacci(7) -> 13
|
| 3) static long Potencia(int baseNumero, int expoente)
|    Potencia(2, 10) -> 1024
|    Potencia(5, 0)  -> 1
|
| 4) static int SomaDosDigitos(int numero)
|    SomaDosDigitos(1234) -> 10
|    SomaDosDigitos(7)    -> 7
|
| 5) static string InverterRecursivo(string texto)
|    Como o exercício 03, mas sem Array.Reverse e sem laço.
|    InverterRecursivo("obra") -> "arbo"
|
| Pergunta para pensar (não precisa escrever a resposta):
| Fibonacci(45) demora bastante com a versão recursiva simples. Por quê?
| O que mudaria se você guardasse os resultados já calculados num
| Dictionary<int, long>?
|
| RESPOSTA: cada chamada dispara outras duas, então o número de chamadas
| dobra a cada nível — Fibonacci(45) passa de um bilhão delas, e quase todas
| recalculam algo que já foi calculado. Guardando cada resultado, cada valor
| é calculado uma vez só e o custo cai de exponencial para linear. A versão
| memoizada está no fim do arquivo, com a medição de tempo lado a lado.
|
| Rode com:  dotnet run --project 05_MetodosERecursao
|
*/

using System.Diagnostics;

Console.WriteLine("--- Fatorial ---");

foreach (int n in new[] { 0, 1, 5, 10 })
{
    Console.WriteLine($"Fatorial({n}) = {Fatorial(n)}");
}

Console.WriteLine("\n--- Fibonacci ---");

for (int i = 0; i <= 10; i++)
{
    Console.Write($"{Fibonacci(i)} ");
}

Console.WriteLine("\n\n--- Potência ---");
Console.WriteLine($"Potencia(2, 10) = {Potencia(2, 10)}");
Console.WriteLine($"Potencia(5, 0)  = {Potencia(5, 0)}");

Console.WriteLine("\n--- Soma dos dígitos ---");
Console.WriteLine($"SomaDosDigitos(1234) = {SomaDosDigitos(1234)}");

Console.WriteLine("\n--- Inverter recursivo ---");
Console.WriteLine($"InverterRecursivo(\"obra\") = {InverterRecursivo("obra")}");

Console.WriteLine("\n--- Recursão simples x memoizada ---");

var cronometro = Stopwatch.StartNew();
long simples = Fibonacci(32);
cronometro.Stop();
Console.WriteLine($"Fibonacci(32)          = {simples}  em {cronometro.ElapsedMilliseconds} ms");

cronometro.Restart();
long memoizado = FibonacciMemoizado(32);
cronometro.Stop();
Console.WriteLine($"FibonacciMemoizado(32) = {memoizado}  em {cronometro.ElapsedMilliseconds} ms");

static long Fatorial(int n)
{
    /*
     * O caso-base é o que encerra a recursão. Sem ele — ou com ele errado —
     * a função chama a si mesma até estourar a pilha, e em C# o
     * StackOverflowException nem é capturável: o processo morre.
     *
     * Vale para 0 e para 1 porque 0! = 1 e 1! = 1.
     */
    if (n <= 1)
    {
        return 1;
    }

    return n * Fatorial(n - 1);
}

static long Fibonacci(int n)
{
    // Dois casos-base, porque a conta depende dos DOIS anteriores.
    if (n <= 0)
    {
        return 0;
    }

    if (n == 1)
    {
        return 1;
    }

    return Fibonacci(n - 1) + Fibonacci(n - 2);
}

static long Potencia(int baseNumero, int expoente)
{
    // Qualquer número elevado a 0 é 1 — e é esse o caso-base.
    if (expoente <= 0)
    {
        return 1;
    }

    return baseNumero * Potencia(baseNumero, expoente - 1);
}

static int SomaDosDigitos(int numero)
{
    numero = Math.Abs(numero);

    // Um dígito só: não há o que somar.
    if (numero < 10)
    {
        return numero;
    }

    // % 10 pega o último dígito; / 10 em inteiros descarta ele e segue.
    return (numero % 10) + SomaDosDigitos(numero / 10);
}

static string InverterRecursivo(string texto)
{
    // String vazia já está invertida — caso-base.
    if (texto.Length == 0)
    {
        return "";
    }

    // Tira a primeira letra, inverte o resto, e joga a primeira no fim.
    return InverterRecursivo(texto[1..]) + texto[0];
}

/*
 * A mesma conta, guardando o que já foi calculado.
 *
 * O dicionário vive fora do método para sobreviver entre as chamadas —
 * inclusive entre as recursivas. É o equivalente ao `static $array` do PHP.
 */
static long FibonacciMemoizado(int n)
{
    var jaCalculados = new Dictionary<int, long>();

    return Calcular(n);

    long Calcular(int k)
    {
        if (k <= 0)
        {
            return 0;
        }

        if (k == 1)
        {
            return 1;
        }

        if (jaCalculados.TryGetValue(k, out long guardado))
        {
            return guardado;
        }

        long valor = Calcular(k - 1) + Calcular(k - 2);
        jaCalculados[k] = valor;

        return valor;
    }
}
