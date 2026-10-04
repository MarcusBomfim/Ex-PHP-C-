<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 06 — Classes e objetos
|--------------------------------------------------------------------------
|
| Conteúdo: classe, construtor, propriedades privadas, métodos públicos,
| encapsulamento, exceções e constantes de classe.
|
| A ideia central: o saldo é PRIVADO. Ninguém de fora consegue mexer nele
| direto — só pelos métodos, que fazem a conferência antes. É isso que
| impede uma conta de ficar negativa por acidente.
|
| Implemente a classe ContaBancaria com:
|
| 1) __construct(string $titular, float $saldoInicial = 0.0)
|    Recusa titular vazio e saldo inicial negativo (lance InvalidArgumentException).
|
| 2) depositar(float $valor): void
|    Recusa valor menor ou igual a zero.
|    Registra a movimentação no extrato.
|
| 3) sacar(float $valor): void
|    Recusa valor menor ou igual a zero.
|    Recusa saque maior que o saldo (lance DomainException com uma
|    mensagem que diga quanto há disponível).
|    Registra a movimentação no extrato.
|
| 4) saldo(): float
|    Devolve o saldo atual. Só leitura — não existe setSaldo().
|
| 5) titular(): string
|
| 6) extrato(): array
|    Lista de strings, uma por movimentação, no formato:
|      "deposito R$ 500.00"
|      "saque R$ 120.50"
|
| 7) transferirPara(ContaBancaria $destino, float $valor): void
|    Saca daqui e deposita lá. Se o saque falhar, nada acontece no destino.
|
| Rode com:  php php/06_classes.php
|
*/

final class ContaBancaria
{
    // TODO: declare as propriedades privadas ($titular, $saldo, $extrato)

    public function __construct(string $titular, float $saldoInicial = 0.0)
    {
        // TODO: implemente
    }

    public function depositar(float $valor): void
    {
        // TODO: implemente
    }

    public function sacar(float $valor): void
    {
        // TODO: implemente
    }

    public function saldo(): float
    {
        // TODO: implemente
        return 0.0;
    }

    public function titular(): string
    {
        // TODO: implemente
        return '';
    }

    /** @return string[] */
    public function extrato(): array
    {
        // TODO: implemente
        return [];
    }

    public function transferirPara(self $destino, float $valor): void
    {
        // TODO: implemente
    }
}

// ---------------------------------------------------------------- saída

$conta = new ContaBancaria('Marcus', 1000.0);
$outra = new ContaBancaria('Ana');

$conta->depositar(500.0);
$conta->sacar(120.50);
$conta->transferirPara($outra, 200.0);

echo "--- {$conta->titular()} ---\n";
echo 'Saldo: R$ ' . number_format($conta->saldo(), 2, ',', '.') . "\n";

foreach ($conta->extrato() as $linha) {
    echo '  ' . $linha . "\n";
}

echo "\n--- {$outra->titular()} ---\n";
echo 'Saldo: R$ ' . number_format($outra->saldo(), 2, ',', '.') . "\n";

echo "\n--- O que deve dar erro ---\n";

try {
    $conta->sacar(999999.0);
    echo "ERRO: o saque acima do saldo passou!\n";
} catch (DomainException $erro) {
    echo 'Recusado, como esperado: ' . $erro->getMessage() . "\n";
}

try {
    new ContaBancaria('', 100.0);
    echo "ERRO: titular vazio passou!\n";
} catch (InvalidArgumentException $erro) {
    echo 'Recusado, como esperado: ' . $erro->getMessage() . "\n";
}
