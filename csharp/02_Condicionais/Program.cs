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
    /*
     * Switch expression com padrões relacionais. Ele é avaliado de cima
     * para baixo e para no primeiro que casa — por isso o `< 0` vem antes
     * de tudo: -1 também satisfaz `<= 11`, e sem essa ordem viraria
     * "criança".
     *
     * Vantagem sobre o if/else if: o compilador avisa se faltar caso. Tire
     * o `_` do fim e ele reclama que nem todo valor está coberto.
     */
    return idade switch
    {
        < 0 => "idade inválida",
        <= 11 => "criança",
        <= 17 => "adolescente",
        <= 59 => "adulto",
        _ => "idoso",
    };
}

static bool EhBissexto(int ano)
{
    /*
     * A regra em uma linha: divisível por 4 e não por 100, OU por 400.
     *
     * 1900 passa no "divisível por 4" mas cai no "e não por 100" — e não é
     * divisível por 400. Resultado: comum. 2000 é divisível por 400, e o
     * segundo lado do OU resolve sozinho.
     */
    return (ano % 4 == 0 && ano % 100 != 0) || ano % 400 == 0;
}

static int MaiorDeTres(int a, int b, int c)
{
    int maior = a;

    if (b > maior)
    {
        maior = b;
    }

    if (c > maior)
    {
        maior = c;
    }

    return maior;
}

static double CustoDoFrete(double pesoEmKg, bool ehExpresso)
{
    double baseDoFrete = pesoEmKg switch
    {
        <= 1.0 => 10.0,
        <= 10.0 => 20.0,
        _ => 35.0,
    };

    // "Expresso dobra o valor FINAL": a faixa é calculada primeiro.
    return ehExpresso ? baseDoFrete * 2 : baseDoFrete;
}
