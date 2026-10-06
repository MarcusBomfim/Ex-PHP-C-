<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 02 — Condicionais
|--------------------------------------------------------------------------
|
| Conteúdo: if/elseif/else, match, operadores lógicos (&&, ||, !) e
| comparação estrita (===).
|
| 1) function classificarIdade(int $idade): string
|    0 a 11   -> "criança"
|    12 a 17  -> "adolescente"
|    18 a 59  -> "adulto"
|    60 ou +  -> "idoso"
|    Idade negativa -> "idade inválida"
|
| 2) function ehBissexto(int $ano): bool
|    Um ano é bissexto quando é divisível por 4, EXCETO se for divisível
|    por 100 — a menos que também seja divisível por 400.
|
|    ehBissexto(2024) -> true
|    ehBissexto(1900) -> false   (divisível por 100, não por 400)
|    ehBissexto(2000) -> true    (divisível por 400)
|
| 3) function maiorDeTres(int $a, int $b, int $c): int
|    Devolve o maior dos três. Não use max() — a ideia é praticar o if.
|
| 4) function custoDoFrete(float $pesoEmKg, bool $ehExpresso): float
|    Até 1 kg ............ R$ 10,00
|    Acima de 1 até 10 kg  R$ 20,00
|    Acima de 10 kg ...... R$ 35,00
|    Entrega expressa dobra o valor final.
|
|    custoDoFrete(0.5, false)  -> 10.0
|    custoDoFrete(5.0, true)   -> 40.0
|    custoDoFrete(12.0, false) -> 35.0
|
| Rode com:  php php/02_condicionais.php
|
*/

function classificarIdade(int $idade): string
{
    /*
     * `match(true)` compara cada condição contra true e para na primeira
     * verdadeira — é o if/elseif escrito como expressão. A ordem importa:
     * a negativa vem antes de tudo, senão 0 a 11 nunca seria alcançado por
     * um número negativo... mas -5 também não é <= 11? É. Por isso a
     * checagem do inválido vem PRIMEIRO.
     */
    return match (true) {
        $idade < 0 => 'idade inválida',
        $idade <= 11 => 'criança',
        $idade <= 17 => 'adolescente',
        $idade <= 59 => 'adulto',
        default => 'idoso',
    };
}

function ehBissexto(int $ano): bool
{
    /*
     * A regra em uma linha: divisível por 4 e não por 100, OU divisível
     * por 400.
     *
     * 1900 passa no "divisível por 4" mas cai no "e não por 100" — e não é
     * divisível por 400. Resultado: comum. 2000 é divisível por 400, então
     * o segundo lado do OU resolve sozinho.
     */
    return ($ano % 4 === 0 && $ano % 100 !== 0) || $ano % 400 === 0;
}

function maiorDeTres(int $a, int $b, int $c): int
{
    $maior = $a;

    if ($b > $maior) {
        $maior = $b;
    }

    if ($c > $maior) {
        $maior = $c;
    }

    return $maior;
}

function custoDoFrete(float $pesoEmKg, bool $ehExpresso): float
{
    $base = match (true) {
        $pesoEmKg <= 1.0 => 10.0,
        $pesoEmKg <= 10.0 => 20.0,
        default => 35.0,
    };

    // "Expresso dobra o valor FINAL": a conta da faixa vem primeiro.
    return $ehExpresso ? $base * 2 : $base;
}

// ---------------------------------------------------------------- saída

echo "--- Faixa etária ---\n";

foreach ([5, 15, 30, 70, -1] as $idade) {
    echo $idade . ' anos: ' . classificarIdade($idade) . "\n";
}

echo "\n--- Ano bissexto ---\n";

foreach ([2024, 1900, 2000, 2026] as $ano) {
    echo $ano . ': ' . (ehBissexto($ano) ? 'bissexto' : 'comum') . "\n";
}

echo "\n--- Maior de três ---\n";
echo 'maiorDeTres(3, 9, 7) = ' . maiorDeTres(3, 9, 7) . "\n";

echo "\n--- Frete ---\n";
echo '0,5 kg normal   = R$ ' . custoDoFrete(0.5, false) . "\n";
echo '5 kg expresso   = R$ ' . custoDoFrete(5.0, true) . "\n";
echo '12 kg normal    = R$ ' . custoDoFrete(12.0, false) . "\n";
