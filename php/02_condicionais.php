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
    // TODO: implemente
    return '';
}

function ehBissexto(int $ano): bool
{
    // TODO: implemente
    return false;
}

function maiorDeTres(int $a, int $b, int $c): int
{
    // TODO: implemente
    return 0;
}

function custoDoFrete(float $pesoEmKg, bool $ehExpresso): float
{
    // TODO: implemente
    return 0.0;
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
