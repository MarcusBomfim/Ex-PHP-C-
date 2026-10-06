/*
|--------------------------------------------------------------------------
| EXERCÍCIO 06 — Classes e objetos
|--------------------------------------------------------------------------
|
| Mesmo problema do php/06_classes.php.
|
| Conteúdo: class, construtor, campos privados, propriedades somente
| leitura, métodos públicos, exceções e IReadOnlyList<T>.
|
| A ideia central: o saldo é PRIVADO. Ninguém de fora mexe nele direto —
| só pelos métodos, que conferem antes. É isso que impede uma conta de
| ficar negativa por acidente.
|
| Implemente a classe ContaBancaria com:
|
| 1) ContaBancaria(string titular, decimal saldoInicial = 0m)
|    Recusa titular vazio e saldo inicial negativo (ArgumentException).
|
|    Repare no `decimal`: dinheiro não se guarda em double. 0.1 + 0.2 em
|    double não dá exatamente 0.3, e num extrato isso vira centavo sumido.
|
| 2) void Depositar(decimal valor)
|    Recusa valor menor ou igual a zero. Registra no extrato.
|
| 3) void Sacar(decimal valor)
|    Recusa valor menor ou igual a zero.
|    Recusa saque maior que o saldo (InvalidOperationException, com uma
|    mensagem que diga quanto há disponível). Registra no extrato.
|
| 4) decimal Saldo { get; }
|    Propriedade só de leitura — não existe `set`.
|
| 5) string Titular { get; }
|
| 6) IReadOnlyList<string> Extrato()
|    Uma string por movimentação:
|      "deposito R$ 500.00"
|      "saque R$ 120.50"
|
|    Devolve IReadOnlyList para quem recebe não conseguir dar Add na lista
|    interna da conta por fora.
|
| 7) void TransferirPara(ContaBancaria destino, decimal valor)
|    Saca daqui e deposita lá. Se o saque falhar, nada acontece no destino.
|
| Rode com:  dotnet run --project 06_Classes
|
*/

using System.Globalization;

var conta = new ContaBancaria("Marcus", 1000m);
var outra = new ContaBancaria("Ana");

conta.Depositar(500m);
conta.Sacar(120.50m);
conta.TransferirPara(outra, 200m);

Console.WriteLine($"--- {conta.Titular} ---");
Console.WriteLine($"Saldo: R$ {conta.Saldo:N2}");

foreach (string linha in conta.Extrato())
{
    Console.WriteLine($"  {linha}");
}

Console.WriteLine($"\n--- {outra.Titular} ---");
Console.WriteLine($"Saldo: R$ {outra.Saldo:N2}");

Console.WriteLine("\n--- O que deve dar erro ---");

try
{
    conta.Sacar(999999m);
    Console.WriteLine("ERRO: o saque acima do saldo passou!");
}
catch (InvalidOperationException erro)
{
    Console.WriteLine($"Recusado, como esperado: {erro.Message}");
}

try
{
    _ = new ContaBancaria("", 100m);
    Console.WriteLine("ERRO: titular vazio passou!");
}
catch (ArgumentException erro)
{
    Console.WriteLine($"Recusado, como esperado: {erro.Message}");
}

// E a prova de que o extrato devolvido não dá para adulterar de fora:
Console.WriteLine($"\nO extrato devolvido é só de leitura? {conta.Extrato() is not List<string>}");

public sealed class ContaBancaria
{
    private decimal _saldo;

    private readonly List<string> _movimentacoes = [];

    public ContaBancaria(string titular, decimal saldoInicial = 0m)
    {
        if (string.IsNullOrWhiteSpace(titular))
        {
            throw new ArgumentException("O titular da conta é obrigatório.", nameof(titular));
        }

        if (saldoInicial < 0m)
        {
            throw new ArgumentException("O saldo inicial não pode ser negativo.", nameof(saldoInicial));
        }

        Titular = titular.Trim();
        _saldo = saldoInicial;

        if (saldoInicial > 0m)
        {
            Registrar("deposito", saldoInicial);
        }
    }

    /*
     * Propriedade com `get` só: dá para ler de fora, nunca para escrever.
     * O compilador recusa `conta.Titular = "outro"` — não é convenção, é
     * regra. É o que o PHP faz com `private readonly` mais um getter.
     */
    public string Titular { get; }

    public decimal Saldo => _saldo;

    public void Depositar(decimal valor)
    {
        ExigirValorPositivo(valor);

        _saldo += valor;
        Registrar("deposito", valor);
    }

    public void Sacar(decimal valor)
    {
        ExigirValorPositivo(valor);

        /*
         * A conferência vem ANTES de mexer no saldo. Se fosse depois, a
         * conta ficaria negativa por um instante — e bastaria uma exceção
         * no meio do caminho para ela ficar assim de vez.
         */
        if (valor > _saldo)
        {
            throw new InvalidOperationException(
                $"Saldo insuficiente: o saque é de R$ {valor:N2} e há R$ {_saldo:N2} disponíveis."
            );
        }

        _saldo -= valor;
        Registrar("saque", valor);
    }

    /*
     * Devolver a List direto seria um furo no encapsulamento: quem recebe
     * poderia dar Add e inventar uma movimentação que nunca aconteceu. A
     * lista é referência, não cópia.
     *
     * AsReadOnly() devolve uma visão sem Add nem Remove sobre a mesma
     * lista — sem copiar nada.
     *
     * No PHP isso não aparece, porque array é copiado na atribuição.
     */
    public IReadOnlyList<string> Extrato() => _movimentacoes.AsReadOnly();

    public void TransferirPara(ContaBancaria destino, decimal valor)
    {
        ArgumentNullException.ThrowIfNull(destino);

        /*
         * A ordem resolve sozinha o "se o saque falhar, nada acontece no
         * destino": Sacar lança antes de Depositar ser chamado, e a
         * execução nem chega na linha de baixo.
         */
        Sacar(valor);
        destino.Depositar(valor);
    }

    private static void ExigirValorPositivo(decimal valor)
    {
        if (valor <= 0m)
        {
            throw new ArgumentException("O valor precisa ser maior que zero.", nameof(valor));
        }
    }

    private void Registrar(string tipo, decimal valor)
    {
        // InvariantCulture no extrato: o enunciado pede "R$ 500.00" com
        // ponto. Sem isso, a saída mudaria conforme a máquina de quem roda.
        _movimentacoes.Add($"{tipo} R$ {valor.ToString("F2", CultureInfo.InvariantCulture)}");
    }
}
