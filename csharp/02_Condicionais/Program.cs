/*
|--------------------------------------------------------------------------
| EXERCÍCIO 02 — Condicionais
|--------------------------------------------------------------------------
|
| Mesmo problema do php/02_condicionais.php.
|
| Conteúdo: if/else if/else, `switch` com expressão (switch expression),
| operadores lógicos e o `is` com padrões de intervalo.
|
| 1) static string ClassificarIdade(int idade)
|    0 a 11   -> "criança"
|    12 a 17  -> "adolescente"
|    18 a 59  -> "adulto"
|    60 ou +  -> "idoso"
|    Negativa -> "idade inválida"
|
|    Tente escrever de dois jeitos: com if/else if e com switch expression
|    usando padrões (`idade switch { < 0 => ..., <= 11 => ..., ... }`).
|
| 2) static bool EhBissexto(int ano)
|    Divisível por 4, EXCETO se divisível por 100 — a menos que também
|    seja divisível por 400.
|
|    EhBissexto(2024) -> true
|    EhBissexto(1900) -> false
|    EhBissexto(2000) -> true
|
| 3) static int MaiorDeTres(int a, int b, int c)
|    O maior dos três, sem usar Math.Max — a ideia é praticar o if.
|
| 4) static double CustoDoFrete(double pesoEmKg, bool ehExpresso)
|    Até 1 kg ............ 10.0
|    Acima de 1 até 10 kg  20.0
|    Acima de 10 kg ...... 35.0
|    Expresso dobra o total.
|
|    CustoDoFrete(0.5, false)  -> 10.0
|    CustoDoFrete(5.0, true)   -> 40.0
|    CustoDoFrete(12.0, false) -> 35.0
|
| Rode com:  dotnet run --project 02_Condicionais
|
*/

Console.WriteLine("--- Faixa etária ---");

foreach (int idade in new[] { 5, 15, 30, 70, -1 })
{
    Console.WriteLine($"{idade} anos: {ClassificarIdade(idade)}");
}

Console.WriteLine("\n--- Ano bissexto ---");

foreach (int ano in new[] { 2024, 1900, 2000, 2026 })
{
    Console.WriteLine($"{ano}: {(EhBissexto(ano) ? "bissexto" : "comum")}");
}

Console.WriteLine("\n--- Maior de três ---");
Console.WriteLine($"MaiorDeTres(3, 9, 7) = {MaiorDeTres(3, 9, 7)}");

Console.WriteLine("\n--- Frete ---");
Console.WriteLine($"0,5 kg normal = R$ {CustoDoFrete(0.5, false)}");
Console.WriteLine($"5 kg expresso = R$ {CustoDoFrete(5.0, true)}");
Console.WriteLine($"12 kg normal  = R$ {CustoDoFrete(12.0, false)}");

static string ClassificarIdade(int idade)
{
    // TODO: implemente
    return "";
}

static bool EhBissexto(int ano)
{
    // TODO: implemente
    return false;
}

static int MaiorDeTres(int a, int b, int c)
{
    // TODO: implemente
    return 0;
}

static double CustoDoFrete(double pesoEmKg, bool ehExpresso)
{
    // TODO: implemente
    return 0.0;
}
