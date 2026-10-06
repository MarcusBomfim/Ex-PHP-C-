/*
|--------------------------------------------------------------------------
| EXERCÍCIO 08 — Herança, classes abstratas e interfaces
|--------------------------------------------------------------------------
|
| Mesmo problema do php/08_heranca.php.
|
| Conteúdo: abstract class, herança, interface, polimorfismo e a diferença
| entre "é um" (herança) e "sabe fazer" (interface).
|
| O problema: uma folha de pagamento com três tipos de funcionário, cada um
| com uma regra de salário diferente.
|
| 1) abstract class Funcionario
|    - Nome e Matricula
|    - abstract decimal CalcularSalario()
|    - string Resumo()  — concreto, usa CalcularSalario()
|
| 2) Mensalista : Funcionario      — salário fixo
| 3) Horista : Funcionario         — hora x horas; acima de 180 h, 1,5x
| 4) Comissionado : Funcionario    — base + % das vendas
|
| 5) interface IBonificavel { decimal Bonus(); }
|    Mensalista (10% do salário) e Comissionado (5% das vendas). O Horista
|    NÃO implementa.
|
| 6) static decimal FolhaDePagamento(IEnumerable<Funcionario> funcionarios)
|
| Diferença para o PHP que vale notar: aqui a herança usa `:` em vez de
| `extends`/`implements`, e a mesma lista pode trazer os dois — a classe
| base primeiro, as interfaces depois. E o `override` é obrigatório: sem
| ele o compilador recusa, o que impede sobrescrever um método por engano.
|
| Rode com:  dotnet run --project 08_Heranca
|
*/

using System.Globalization;

var cultura = new CultureInfo("pt-BR");

Funcionario[] equipe =
[
    new Mensalista("Ana", "M-001", 4500.00m),
    new Horista("Bruno", "H-014", 32.50m, 200),
    new Comissionado("Carla", "C-007", 2000.00m, 48000.00m, 3.0m),
];

Console.WriteLine("--- Folha ---");

foreach (Funcionario funcionario in equipe)
{
    /*
     * `is IBonificavel comBonus` testa e já converte numa linha só. Sem
     * isso seria um `is` seguido de um cast — e o cast é onde nasce o
     * InvalidCastException de quem esqueceu de testar antes.
     */
    string extra = funcionario is IBonificavel comBonus
        ? $"  + bônus {comBonus.Bonus().ToString("C", cultura)}"
        : "  (sem bônus)";

    Console.WriteLine(funcionario.Resumo(cultura) + extra);
}

Console.WriteLine($"\nTotal da folha: {FolhaDePagamento(equipe).ToString("C", cultura)}");

Console.WriteLine("\n--- Horas extras ---");
var semExtra = new Horista("Bruno", "H-014", 32.50m, 180);
var comExtra = new Horista("Bruno", "H-014", 32.50m, 200);

Console.WriteLine($"180 h = {semExtra.CalcularSalario().ToString("C", cultura)}");
Console.WriteLine($"200 h = {comExtra.CalcularSalario().ToString("C", cultura)}  (as 20 h extras valem 1,5x)");

Console.WriteLine("\n--- O que a classe abstrata impede ---");
Console.WriteLine("  `new Funcionario(\"Fantasma\", \"X-000\")` nem compila:");
Console.WriteLine("  erro CS0144 — não é possível criar instância de classe abstrata.");
Console.WriteLine("  No PHP o mesmo erro só aparece quando a linha executa.");

static decimal FolhaDePagamento(IEnumerable<Funcionario> funcionarios)
{
    decimal total = 0m;

    foreach (Funcionario funcionario in funcionarios)
    {
        /*
         * Aqui está o polimorfismo: o laço não sabe — nem precisa saber —
         * qual é o tipo de cada um. Chama CalcularSalario() e cada objeto
         * responde com a própria regra.
         *
         * Acrescentar um quarto tipo de funcionário amanhã não muda uma
         * linha desta função.
         */
        total += funcionario.CalcularSalario();

        // Teste contra a INTERFACE, não contra as classes. Assim não há
        // lista de tipos para manter atualizada.
        if (funcionario is IBonificavel bonificavel)
        {
            total += bonificavel.Bonus();
        }
    }

    return total;
}

public abstract class Funcionario(string nome, string matricula)
{
    public string Nome { get; } = nome;

    public string Matricula { get; } = matricula;

    /*
     * Sem corpo: cada subclasse é obrigada a escrever o seu. Tentar
     * instanciar `new Funcionario(...)` nem compila — o erro é pego pelo
     * compilador, não em produção.
     */
    public abstract decimal CalcularSalario();

    /** Concreto, e de propósito: a forma do resumo é a mesma para todos. */
    public string Resumo(CultureInfo cultura)
    {
        // GetType().Name devolve a classe REAL do objeto, não a abstrata.
        return $"{Nome,-12} {Matricula,-8} {GetType().Name,-14} {CalcularSalario().ToString("C", cultura),14}";
    }
}

public interface IBonificavel
{
    decimal Bonus();
}

public sealed class Mensalista(string nome, string matricula, decimal salarioMensal)
    : Funcionario(nome, matricula), IBonificavel
{
    // `override` é obrigatório em C#. No PHP basta redeclarar o método, e
    // um erro de digitação no nome cria um método novo em silêncio.
    public override decimal CalcularSalario() => salarioMensal;

    public decimal Bonus() => Math.Round(salarioMensal * 0.10m, 2);
}

public sealed class Horista(string nome, string matricula, decimal valorDaHora, int horasTrabalhadas)
    : Funcionario(nome, matricula)
{
    private const int HorasNormaisNoMes = 180;

    private const decimal AdicionalDeHoraExtra = 1.5m;

    public override decimal CalcularSalario()
    {
        int normais = Math.Min(horasTrabalhadas, HorasNormaisNoMes);
        int extras = Math.Max(0, horasTrabalhadas - HorasNormaisNoMes);

        return Math.Round(
            normais * valorDaHora + extras * valorDaHora * AdicionalDeHoraExtra,
            2
        );
    }

    /*
     * Repare no que NÃO está aqui: o Horista não implementa IBonificavel,
     * então não tem Bonus(). É a interface fazendo o trabalho dela — a
     * folha consegue perguntar "este aqui tem bônus?" sem um if por tipo.
     */
}

public sealed class Comissionado(
    string nome,
    string matricula,
    decimal salarioBase,
    decimal totalVendido,
    decimal percentualDeComissao
) : Funcionario(nome, matricula), IBonificavel
{
    public override decimal CalcularSalario()
    {
        return Math.Round(salarioBase + totalVendido * (percentualDeComissao / 100m), 2);
    }

    public decimal Bonus() => Math.Round(totalVendido * 0.05m, 2);
}
