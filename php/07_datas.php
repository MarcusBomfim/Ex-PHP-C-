<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 07 — Datas e horas
|--------------------------------------------------------------------------
|
| Conteúdo: DateTimeImmutable, DateInterval, diff(), format() e por que
| "somar 30 dias" quase nunca é o mesmo que "somar um mês".
|
| 1) function diasEntre(DateTimeImmutable $inicio, DateTimeImmutable $fim): int
|    Número de dias inteiros entre as duas datas. Sempre positivo.
|
|    diasEntre(2026-01-01, 2026-01-31) -> 30
|    diasEntre(2026-01-31, 2026-01-01) -> 30   (a ordem não importa)
|
| 2) function ehFimDeSemana(DateTimeImmutable $data): bool
|
| 3) function adicionarDiasUteis(DateTimeImmutable $data, int $dias): DateTimeImmutable
|    Pula sábados e domingos.
|
|    Sexta 2026-01-02 + 1 dia útil -> segunda 2026-01-05
|    Sexta 2026-01-02 + 5 dias úteis -> sexta 2026-01-09
|
| 4) function idadeEm(DateTimeImmutable $nascimento, DateTimeImmutable $referencia): int
|    Idade em anos completos. Quem ainda não fez aniversário no ano tem
|    um ano a menos.
|
| 5) function formatarDuracao(int $minutos): string
|    0    -> "0min"
|    45   -> "45min"
|    60   -> "1h"
|    150  -> "2h 30min"
|
| Rode com:  php php/07_datas.php
|
*/

function diasEntre(DateTimeImmutable $inicio, DateTimeImmutable $fim): int
{
    /*
     * setTime(0,0) nos dois: sem isso, 01/01 às 23h e 02/01 às 01h dariam
     * 0 dias de diferença, porque diff() conta períodos completos de 24h,
     * não viradas de calendário.
     *
     * ->days vem do DateInterval e já é o total absoluto de dias; ->d seria
     * só a parte "dias" de um resultado tipo "1 mês e 2 dias".
     */
    $de = $inicio->setTime(0, 0);
    $ate = $fim->setTime(0, 0);

    return (int) $de->diff($ate)->days;
}

function ehFimDeSemana(DateTimeImmutable $data): bool
{
    // 'N' é o dia da semana no padrão ISO: 1 = segunda ... 7 = domingo.
    // Usar 'w' (0 = domingo) também funciona, mas troca a conta.
    $diaDaSemana = (int) $data->format('N');

    return $diaDaSemana >= 6;
}

function adicionarDiasUteis(DateTimeImmutable $data, int $dias): DateTimeImmutable
{
    $resultado = $data;

    /*
     * Avança um dia por vez e só conta quando cai em dia útil. Parece
     * ingênuo, mas é o jeito certo: a fórmula "dias/5*7" erra sempre que o
     * ponto de partida é fim de semana — e quebra de vez quando entram
     * feriados, que é o próximo passo natural deste exercício.
     */
    while ($dias > 0) {
        $resultado = $resultado->modify('+1 day');

        if (!ehFimDeSemana($resultado)) {
            $dias--;
        }
    }

    return $resultado;
}

function idadeEm(DateTimeImmutable $nascimento, DateTimeImmutable $referencia): int
{
    /*
     * O diff()->y já resolve o "ainda não fez aniversário": ele conta anos
     * COMPLETOS. Quem nasceu em 10/12/2000 e é consultado em 09/12/2026
     * recebe 25, não 26.
     *
     * Fazer na mão — subtrair os anos e corrigir com if — é onde mora o
     * bug clássico de sistema de cadastro.
     */
    return (int) $nascimento->setTime(0, 0)->diff($referencia->setTime(0, 0))->y;
}

function formatarDuracao(int $minutos): string
{
    if ($minutos <= 0) {
        return '0min';
    }

    $horas = intdiv($minutos, 60);
    $resto = $minutos % 60;

    if ($horas === 0) {
        return $resto . 'min';
    }

    // Hora cheia não mostra "0min" pendurado no fim.
    return $resto === 0 ? $horas . 'h' : $horas . 'h ' . $resto . 'min';
}

// ---------------------------------------------------------------- saída

$primeiroDeJaneiro = new DateTimeImmutable('2026-01-01');
$ultimoDeJaneiro = new DateTimeImmutable('2026-01-31');

echo "--- Dias entre ---\n";
echo '01/01 a 31/01 = ' . diasEntre($primeiroDeJaneiro, $ultimoDeJaneiro) . " dias\n";
echo '31/01 a 01/01 = ' . diasEntre($ultimoDeJaneiro, $primeiroDeJaneiro) . " dias (mesma coisa)\n";

echo "\n--- Fim de semana ---\n";

foreach (['2026-01-02', '2026-01-03', '2026-01-04', '2026-01-05'] as $dia) {
    $data = new DateTimeImmutable($dia);
    echo $data->format('d/m/Y (D)') . ': ' . (ehFimDeSemana($data) ? 'fim de semana' : 'dia útil') . "\n";
}

echo "\n--- Dias úteis ---\n";
$sexta = new DateTimeImmutable('2026-01-02');

foreach ([1, 5, 10] as $dias) {
    echo "sexta 02/01 + {$dias} dia(s) útil(eis) = "
        . adicionarDiasUteis($sexta, $dias)->format('d/m/Y (D)') . "\n";
}

echo "\n--- Idade ---\n";
$nascimento = new DateTimeImmutable('2000-12-10');

foreach (['2026-12-09', '2026-12-10'] as $quando) {
    echo 'em ' . $quando . ': ' . idadeEm($nascimento, new DateTimeImmutable($quando)) . " anos\n";
}

echo "\n--- Duração ---\n";

foreach ([0, 45, 60, 150, 1440] as $minutos) {
    echo str_pad((string) $minutos, 5, ' ', STR_PAD_LEFT) . ' min = ' . formatarDuracao($minutos) . "\n";
}
