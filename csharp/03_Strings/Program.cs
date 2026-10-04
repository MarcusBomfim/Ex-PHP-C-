/*
|--------------------------------------------------------------------------
| EXERCÍCIO 03 — Strings
|--------------------------------------------------------------------------
|
| Mesmo problema do php/03_strings.php.
|
| Conteúdo: métodos de string (ToLower, Replace, Split, Substring, Trim),
| char, e o fato de string em C# ser IMUTÁVEL — todo "Replace" devolve uma
| string nova em vez de alterar a original.
|
| 1) static string Inverter(string texto)
|    Inverter("obra") -> "arbo"
|    Dica: existe Array.Reverse, e também new string(char[]).
|
| 2) static bool EhPalindromo(string texto)
|    Ignora maiúsculas e espaços.
|
|    EhPalindromo("arara")               -> true
|    EhPalindromo("Ana")                 -> true
|    EhPalindromo("socorram me")         -> false
|    EhPalindromo("A mala nada na lama")  -> true
|
| 3) static int ContarVogais(string texto)
|    a, e, i, o, u — maiúsculas e minúsculas.
|
|    ContarVogais("concreto") -> 3
|    ContarVogais("XYZ")      -> 0
|
| 4) static string Iniciais(string nomeCompleto)
|    Iniciais("marcus bomfim silva") -> "M.B.S."
|    Iniciais("Ana")                 -> "A."
|
| 5) static string MascararEmail(string email)
|    MascararEmail("marcus@obra.dev") -> "m*****@obra.dev"
|    O número de asteriscos é o tamanho do resto do usuário.
|    Dica: veja new string('*', quantidade).
|
| Rode com:  dotnet run --project 03_Strings
|
*/

Console.WriteLine("--- Inverter ---");
Console.WriteLine($"Inverter(\"obra\") = {Inverter("obra")}");

Console.WriteLine("\n--- Palíndromo ---");

foreach (string texto in new[] { "arara", "Ana", "socorram me", "A mala nada na lama" })
{
    Console.WriteLine($"\"{texto}\": {(EhPalindromo(texto) ? "sim" : "não")}");
}

Console.WriteLine("\n--- Vogais ---");
Console.WriteLine($"ContarVogais(\"concreto\") = {ContarVogais("concreto")}");

Console.WriteLine("\n--- Iniciais ---");
Console.WriteLine($"Iniciais(\"marcus bomfim silva\") = {Iniciais("marcus bomfim silva")}");

Console.WriteLine("\n--- E-mail mascarado ---");
Console.WriteLine(MascararEmail("marcus@obra.dev"));

static string Inverter(string texto)
{
    // TODO: implemente
    return "";
}

static bool EhPalindromo(string texto)
{
    // TODO: implemente
    return false;
}

static int ContarVogais(string texto)
{
    // TODO: implemente
    return 0;
}

static string Iniciais(string nomeCompleto)
{
    // TODO: implemente
    return "";
}

static string MascararEmail(string email)
{
    // TODO: implemente
    return "";
}
