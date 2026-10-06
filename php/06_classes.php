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
    private readonly string $titular;

    private float $saldo;

    /** @var string[] */
    private array $extrato = [];

    public function __construct(string $titular, float $saldoInicial = 0.0)
    {
        $titular = trim($titular);

        if ($titular === '') {
            throw new InvalidArgumentException('O titular da conta é obrigatório.');
        }

        if ($saldoInicial < 0) {
            throw new InvalidArgumentException('O saldo inicial não pode ser negativo.');
        }

        $this->titular = $titular;
        $this->saldo = $saldoInicial;

        if ($saldoInicial > 0) {
            $this->registrar('deposito', $saldoInicial);
        }
    }

    public function depositar(float $valor): void
    {
        $this->exigirValorPositivo($valor);

        $this->saldo += $valor;
        $this->registrar('deposito', $valor);
    }

    public function sacar(float $valor): void
    {
        $this->exigirValorPositivo($valor);

        /*
         * A conferência vem ANTES de mexer no saldo. Se fosse depois, a
         * conta ficaria negativa por um instante — e bastaria uma exceção
         * no meio do caminho para ela ficar assim de vez.
         */
        if ($valor > $this->saldo) {
            throw new DomainException(sprintf(
                'Saldo insuficiente: o saque é de R$ %s e há R$ %s disponíveis.',
                number_format($valor, 2, ',', '.'),
                number_format($this->saldo, 2, ',', '.'),
            ));
        }

        $this->saldo -= $valor;
        $this->registrar('saque', $valor);
    }

    public function saldo(): float
    {
        return $this->saldo;
    }

    public function titular(): string
    {
        return $this->titular;
    }

    /** @return string[] */
    public function extrato(): array
    {
        /*
         * Devolver $this->extrato direto não é problema aqui porque array
         * em PHP é copiado na atribuição — quem recebe mexe na própria
         * cópia. Em linguagens onde a lista é referência (C#, Java), isto
         * exigiria devolver uma cópia ou uma visão só de leitura.
         */
        return $this->extrato;
    }

    public function transferirPara(self $destino, float $valor): void
    {
        /*
         * A ordem resolve sozinha o "se o saque falhar, nada acontece no
         * destino": sacar() lança antes de o depósito ser chamado, e a
         * execução nem chega na linha de baixo.
         */
        $this->sacar($valor);
        $destino->depositar($valor);
    }

    private function exigirValorPositivo(float $valor): void
    {
        if ($valor <= 0) {
            throw new InvalidArgumentException('O valor precisa ser maior que zero.');
        }
    }

    private function registrar(string $tipo, float $valor): void
    {
        $this->extrato[] = sprintf('%s R$ %.2f', $tipo, $valor);
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
