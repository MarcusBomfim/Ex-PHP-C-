<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 08 — Herança, classes abstratas e interfaces
|--------------------------------------------------------------------------
|
| Conteúdo: abstract class, extends, implements, polimorfismo e a diferença
| entre "é um" (herança) e "sabe fazer" (interface).
|
| O problema: uma folha de pagamento com três tipos de funcionário, cada um
| com uma regra de salário diferente.
|
| 1) abstract class Funcionario
|    - nome e matrícula
|    - abstract calcularSalario(): float
|    - resumo(): string  — concreto, usa calcularSalario()
|
| 2) class Mensalista extends Funcionario
|    Salário fixo por mês.
|
| 3) class Horista extends Funcionario
|    Valor da hora x horas trabalhadas. Acima de 180 h no mês, as horas
|    excedentes valem 50% a mais.
|
| 4) class Comissionado extends Funcionario
|    Salário base + percentual sobre o que vendeu.
|
| 5) interface Bonificavel { function bonus(): float; }
|    Implementada só por quem tem direito a bônus — Mensalista (10% do
|    salário) e Comissionado (5% das vendas). O Horista NÃO implementa.
|
| 6) function folhaDePagamento(array $funcionarios): float
|    Soma o salário de todos, mais o bônus de quem for Bonificavel.
|
| A pergunta que o exercício responde: por que `abstract` em vez de uma
| classe comum com um `calcularSalario()` que devolve 0? Porque com
| abstract o PHP RECUSA instanciar um Funcionario genérico, e obriga toda
| subclasse nova a implementar a regra. O "devolve 0" seria um salário
| errado esperando para acontecer.
|
| Rode com:  php php/08_heranca.php
|
*/

abstract class Funcionario
{
    public function __construct(
        public readonly string $nome,
        public readonly string $matricula,
    ) {
    }

    /*
     * Sem corpo: cada subclasse é obrigada a escrever o seu. Tentar
     * instanciar `new Funcionario(...)` é erro fatal, não bug silencioso.
     */
    abstract public function calcularSalario(): float;

    /** Concreto, e de propósito: a forma do resumo é a mesma para todos. */
    public function resumo(): string
    {
        return sprintf(
            '%-12s %-8s %-14s R$ %10s',
            $this->nome,
            $this->matricula,
            // static::class devolve a classe REAL do objeto, não a abstrata.
            static::class,
            number_format($this->calcularSalario(), 2, ',', '.'),
        );
    }
}

interface Bonificavel
{
    public function bonus(): float;
}

final class Mensalista extends Funcionario implements Bonificavel
{
    public function __construct(
        string $nome,
        string $matricula,
        private readonly float $salarioMensal,
    ) {
        parent::__construct($nome, $matricula);
    }

    public function calcularSalario(): float
    {
        return $this->salarioMensal;
    }

    public function bonus(): float
    {
        return round($this->salarioMensal * 0.10, 2);
    }
}

final class Horista extends Funcionario
{
    private const HORAS_NORMAIS_NO_MES = 180;

    private const ADICIONAL_DE_HORA_EXTRA = 1.5;

    public function __construct(
        string $nome,
        string $matricula,
        private readonly float $valorDaHora,
        private readonly int $horasTrabalhadas,
    ) {
        parent::__construct($nome, $matricula);
    }

    public function calcularSalario(): float
    {
        $normais = min($this->horasTrabalhadas, self::HORAS_NORMAIS_NO_MES);
        $extras = max(0, $this->horasTrabalhadas - self::HORAS_NORMAIS_NO_MES);

        return round(
            $normais * $this->valorDaHora
            + $extras * $this->valorDaHora * self::ADICIONAL_DE_HORA_EXTRA,
            2,
        );
    }

    /*
     * Repare no que NÃO está aqui: o Horista não implementa Bonificavel,
     * então não tem bonus(). É a interface fazendo o trabalho dela — a
     * folha de pagamento consegue perguntar "este aqui tem bônus?" sem
     * precisar de um if por tipo.
     */
}

final class Comissionado extends Funcionario implements Bonificavel
{
    public function __construct(
        string $nome,
        string $matricula,
        private readonly float $salarioBase,
        private readonly float $totalVendido,
        private readonly float $percentualDeComissao,
    ) {
        parent::__construct($nome, $matricula);
    }

    public function calcularSalario(): float
    {
        return round(
            $this->salarioBase + $this->totalVendido * ($this->percentualDeComissao / 100),
            2,
        );
    }

    public function bonus(): float
    {
        return round($this->totalVendido * 0.05, 2);
    }
}

/** @param Funcionario[] $funcionarios */
function folhaDePagamento(array $funcionarios): float
{
    $total = 0.0;

    foreach ($funcionarios as $funcionario) {
        /*
         * Aqui está o polimorfismo: o laço não sabe — nem precisa saber —
         * qual é o tipo de cada um. Chama calcularSalario() e cada objeto
         * responde com a própria regra.
         *
         * Acrescentar um quarto tipo de funcionário amanhã não muda uma
         * linha desta função.
         */
        $total += $funcionario->calcularSalario();

        // `instanceof` contra a INTERFACE, não contra as classes. Assim
        // não há lista de tipos para manter atualizada.
        if ($funcionario instanceof Bonificavel) {
            $total += $funcionario->bonus();
        }
    }

    return round($total, 2);
}

// ---------------------------------------------------------------- saída

$equipe = [
    new Mensalista('Ana', 'M-001', 4500.00),
    new Horista('Bruno', 'H-014', 32.50, 200),
    new Comissionado('Carla', 'C-007', 2000.00, 48000.00, 3.0),
];

echo "--- Folha ---\n";

foreach ($equipe as $funcionario) {
    echo $funcionario->resumo();
    echo $funcionario instanceof Bonificavel
        ? '  + bônus R$ ' . number_format($funcionario->bonus(), 2, ',', '.')
        : '  (sem bônus)';
    echo "\n";
}

echo "\nTotal da folha: R$ " . number_format(folhaDePagamento($equipe), 2, ',', '.') . "\n";

echo "\n--- Horas extras ---\n";
$semExtra = new Horista('Bruno', 'H-014', 32.50, 180);
$comExtra = new Horista('Bruno', 'H-014', 32.50, 200);

echo '180 h = R$ ' . number_format($semExtra->calcularSalario(), 2, ',', '.') . "\n";
echo '200 h = R$ ' . number_format($comExtra->calcularSalario(), 2, ',', '.')
    . "  (as 20 h extras valem 1,5x)\n";

echo "\n--- O que a classe abstrata impede ---\n";

try {
    // @phpstan-ignore-next-line  — o erro é o ponto do exemplo.
    $reflexao = new ReflectionClass(Funcionario::class);
    $reflexao->newInstance('Fantasma', 'X-000');
    echo "ERRO: instanciou a classe abstrata!\n";
} catch (Error $erro) {
    echo 'Recusado, como esperado: ' . $erro->getMessage() . "\n";
}
