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
    /*
     * String em C# é imutável: não dá para trocar texto[0] de lugar. O
     * caminho é copiar para um char[], inverter o array (esse sim é
     * mutável) e montar uma string nova a partir dele.
     *
     * Vale a mesma ressalva do PHP: isto inverte UNIDADES do char, e um
     * emoji ou um acento composto ocupa mais de uma. Para texto comum
     * resolve; para Unicode completo, veja StringInfo.
     */
    char[] letras = texto.ToCharArray();
    Array.Reverse(letras);

    return new string(letras);
}

static bool EhPalindromo(string texto)
{
    // Normaliza primeiro — minúsculas e sem espaço —, depois compara com
    // a própria inversão. Comparar antes de normalizar reprovaria "Ana".
    string limpo = texto.ToLowerInvariant().Replace(" ", "");

    return limpo == Inverter(limpo);
}

static int ContarVogais(string texto)
{
    const string vogais = "aeiou";
    int total = 0;

    foreach (char letra in texto.ToLowerInvariant())
    {
        if (vogais.Contains(letra))
        {
            total++;
        }
    }

    return total;
}

static string Iniciais(string nomeCompleto)
{
    /*
     * StringSplitOptions.RemoveEmptyEntries evita que dois espaços
     * seguidos virem uma inicial vazia — " marcus  silva " daria
     * "..M..S." sem ele.
     */
    string[] partes = nomeCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var iniciais = new System.Text.StringBuilder();

    foreach (string parte in partes)
    {
        iniciais.Append(char.ToUpperInvariant(parte[0])).Append('.');
    }

    return iniciais.ToString();
}

static string MascararEmail(string email)
{
    int arroba = email.IndexOf('@');

    // Sem arroba não é e-mail: devolve como veio em vez de inventar.
    if (arroba <= 0)
    {
        return email;
    }

    string usuario = email[..arroba];
    string dominio = email[arroba..];

    // new string('*', n) repete o caractere n vezes — é o str_repeat do PHP.
    return usuario[0] + new string('*', usuario.Length - 1) + dominio;
}
