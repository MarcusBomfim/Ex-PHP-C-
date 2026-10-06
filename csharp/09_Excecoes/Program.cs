/*
|--------------------------------------------------------------------------
| EXERCÍCIO 09 — Exceções e tratamento de erros
|--------------------------------------------------------------------------
|
| Mesmo problema do php/09_excecoes.php.
|
| Conteúdo: try/catch/finally, exceções próprias, hierarquia, InnerException
| e quando NÃO usar exceção.
|
| 1) class ErroDeValidacao : ArgumentException
|    Guarda uma LISTA de problemas, não um só. Formulário com três campos
|    errados deve mostrar os três de uma vez.
|
| 2) static void ValidarCadastro(...)
|    Confere nome, e-mail e idade. JUNTA os problemas e lança uma vez só.
|
| 3) static double Dividir(double a, double b)
|    O ponto do exercício: em C#, double dividido por zero NÃO lança — dá
|    Infinity. Já int por zero lança DivideByZeroException. É uma pegadinha
|    que o PHP não tem, porque lá o `/` lança nos dois casos.
|
| 4) static int ConverterParaInteiro(string texto)
|    Lança ErroDeConversao com a exceção original em InnerException.
|
| 5) static int LerComFallback(Func<int> tentativa, int padrao)
|    O `finally` roda nos dois casos.
|
| Quando NÃO usar exceção: "usuário digitou e-mail errado" é fluxo normal
| de formulário e dá para prever. Exceção é para o que foge do esperado — e
| em C# ela custa caro: montar o stack trace não é de graça. Por isso o
| padrão `TryParse`, que devolve bool em vez de lançar.
|
| Rode com:  dotnet run --project 09_Excecoes
|
*/

Console.WriteLine("--- Validação com vários problemas ---");

try
{
    ValidarCadastro(nome: "Jo", email: "nao-e-email", idade: "999");
}
catch (ErroDeValidacao erro)
{
    Console.WriteLine($"Mensagem: {erro.Message}");
    Console.WriteLine("Lista:");

    foreach (string problema in erro.Problemas)
    {
        Console.WriteLine($"  - {problema}");
    }
}

Console.WriteLine("\n--- Cadastro válido ---");

try
{
    ValidarCadastro(nome: "Marcus", email: "marcus@obra.dev", idade: "30");
    Console.WriteLine("Passou sem erro.");
}
catch (ErroDeValidacao erro)
{
    Console.WriteLine($"Não deveria falhar: {erro.Message}");
}

Console.WriteLine("\n--- A pegadinha do double ---");
Console.WriteLine($"10.0 / 0.0 em double  = {10.0 / 0.0}   <- não lança!");
Console.WriteLine($"Dividir(10, 4)        = {Dividir(10.0, 4.0)}");

try
{
    Console.WriteLine($"Dividir(10, 0)        = {Dividir(10.0, 0.0)}");
}
catch (DivideByZeroException erro)
{
    Console.WriteLine($"Dividir(10, 0)        -> {erro.Message}");
}

Console.WriteLine("\n--- Encadeamento de exceções ---");

try
{
    ConverterParaInteiro("vinte");
}
catch (ErroDeConversao erro)
{
    Console.WriteLine($"Topo:  {erro.Message}");
    Console.WriteLine($"Causa: {erro.InnerException?.Message ?? "(nenhuma)"}");
}

Console.WriteLine($"ConverterParaInteiro(\"  42 \") = {ConverterParaInteiro("  42 ")}");

Console.WriteLine("\n--- Fallback com finally ---");
Console.WriteLine($"sucesso: {LerComFallback(() => 7, 0)}");
Console.WriteLine($"falha:   {LerComFallback(() => throw new InvalidOperationException("quebrou"), -1)}");

static void ValidarCadastro(string? nome, string? email, string? idade)
{
    var problemas = new List<string>();

    if (string.IsNullOrWhiteSpace(nome))
    {
        problemas.Add("o nome é obrigatório");
    }
    else if (nome.Trim().Length < 3)
    {
        problemas.Add("o nome precisa ter ao menos 3 caracteres");
    }

    if (string.IsNullOrWhiteSpace(email))
    {
        problemas.Add("o e-mail é obrigatório");
    }
    else if (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@'))
    {
        problemas.Add($"o e-mail \"{email}\" não tem formato válido");
    }

    if (string.IsNullOrWhiteSpace(idade))
    {
        problemas.Add("a idade é obrigatória");
    }
    else if (!int.TryParse(idade, out int anos))
    {
        /*
         * TryParse em vez de Parse dentro de um try/catch. Ele devolve
         * bool e não monta stack trace — num laço sobre mil linhas de CSV,
         * a diferença de desempenho é de ordem de grandeza.
         *
         * É a resposta do C# para "não use exceção em fluxo esperado".
         */
        problemas.Add("a idade precisa ser um número inteiro");
    }
    else if (anos is < 0 or > 130)
    {
        problemas.Add("a idade informada não é plausível");
    }

    // Lança UMA vez, no fim, com tudo junto. Lançar na primeira falha
    // obrigaria a pessoa a corrigir um campo por tentativa.
    if (problemas.Count > 0)
    {
        throw new ErroDeValidacao(problemas);
    }
}

static double Dividir(double a, double b)
{
    /*
     * Esta guarda existe porque o C# NÃO lança aqui. Em ponto flutuante,
     * a norma IEEE 754 manda devolver Infinity — e o programa segue com um
     * número que não é número, contaminando toda conta depois dele.
     *
     * Com `int` o comportamento é outro: 10 / 0 lança DivideByZeroException
     * sozinho. Mesma operação, tipos diferentes, resultados opostos.
     */
    if (b == 0.0)
    {
        throw new DivideByZeroException("Divisão por zero.");
    }

    return a / b;
}

static int ConverterParaInteiro(string texto)
{
    try
    {
        return int.Parse(texto.Trim());
    }
    catch (Exception causa) when (causa is FormatException or OverflowException)
    {
        /*
         * `when` é o filtro de exceção do C#: captura só se a condição for
         * verdadeira. Sem ele seriam dois blocos catch iguais.
         *
         * O segundo argumento é a InnerException: a exceção original vai
         * junto. Sem ela, a mensagem de alto nível substitui a causa raiz
         * e o rastro se perde — é o motivo de tantos "erro ao processar"
         * que não dizem nada.
         */
        throw new ErroDeConversao($"Não foi possível converter \"{texto}\".", causa);
    }
}

static int LerComFallback(Func<int> tentativa, int padrao)
{
    try
    {
        return tentativa();
    }
    catch (Exception)
    {
        return padrao;
    }
    finally
    {
        /*
         * O finally roda SEMPRE — inclusive depois do `return` do try e do
         * `return` do catch. É onde vai o que precisa acontecer de qualquer
         * jeito: fechar arquivo, soltar conexão, liberar trava.
         *
         * Em C# o `using` costuma ser melhor para isso: ele chama Dispose()
         * sozinho, sem finally escrito à mão.
         */
        Console.WriteLine("  (finally: sempre executa)");
    }
}

/*
 * Estende ArgumentException, e não Exception direto, por um motivo prático:
 * quem só quer saber "deu erro de entrada" captura a classe da biblioteca
 * padrão e pega esta junto, sem conhecer o nome dela.
 */
public sealed class ErroDeValidacao : ArgumentException
{
    public ErroDeValidacao(IReadOnlyList<string> problemas)
        : base($"{problemas.Count} problema(s) no cadastro: {string.Join("; ", problemas)}")
    {
        Problemas = problemas;
    }

    public IReadOnlyList<string> Problemas { get; }
}

public sealed class ErroDeConversao(string mensagem, Exception causa)
    : InvalidOperationException(mensagem, causa);
