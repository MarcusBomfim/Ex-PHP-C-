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
| Rode com:  dotnet run --project 05_MetodosERecursao
|
*/

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

static long Fatorial(int n)
{
    // TODO: implemente de forma recursiva
    return 0;
}

static long Fibonacci(int n)
{
    // TODO: implemente de forma recursiva
    return 0;
}

static long Potencia(int baseNumero, int expoente)
{
    // TODO: implemente de forma recursiva
    return 0;
}

static int SomaDosDigitos(int numero)
{
    // TODO: implemente de forma recursiva
    return 0;
}

static string InverterRecursivo(string texto)
{
    // TODO: implemente de forma recursiva
    return "";
}
