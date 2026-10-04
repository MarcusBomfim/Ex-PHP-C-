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

public sealed class ContaBancaria
{
    // TODO: declare os campos privados (saldo e a lista do extrato)

    public ContaBancaria(string titular, decimal saldoInicial = 0m)
    {
        // TODO: implemente
        Titular = "";
    }

    public string Titular { get; }

    public decimal Saldo
    {
        // TODO: implemente
        get => 0m;
    }

    public void Depositar(decimal valor)
    {
        // TODO: implemente
    }

    public void Sacar(decimal valor)
    {
        // TODO: implemente
    }

    public IReadOnlyList<string> Extrato()
    {
        // TODO: implemente
        return [];
    }

    public void TransferirPara(ContaBancaria destino, decimal valor)
    {
        // TODO: implemente
    }
}
