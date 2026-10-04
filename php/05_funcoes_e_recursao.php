<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 05 — Funções e recursão
|--------------------------------------------------------------------------
|
| Conteúdo: parâmetros com valor padrão, recursão, caso-base e o que
| acontece quando ele falta.
|
| Todas abaixo devem ser RECURSIVAS — sem laço for/while.
|
| 1) function fatorial(int $n): int
|    fatorial(0) -> 1    (caso-base)
|    fatorial(5) -> 120
|
| 2) function fibonacci(int $n): int
|    A sequência começa em 0: 0, 1, 1, 2, 3, 5, 8, 13...
|
|    fibonacci(0) -> 0
|    fibonacci(1) -> 1
|    fibonacci(7) -> 13
|
| 3) function potencia(int $base, int $expoente): int
|    potencia(2, 10) -> 1024
|    potencia(5, 0)  -> 1
|
| 4) function somaDosDigitos(int $numero): int
|    somaDosDigitos(1234) -> 10    (1+2+3+4)
|    somaDosDigitos(7)    -> 7
|
| 5) function inverterRecursivo(string $texto): string
|    Como o exercício 03, mas sem strrev e sem laço.
|    inverterRecursivo("obra") -> "arbo"
|
| Pergunta para pensar (não precisa escrever a resposta):
| fibonacci(40) demora bastante com a versão recursiva simples. Por quê?
| O que mudaria se você guardasse os resultados já calculados?
|
| Rode com:  php php/05_funcoes_e_recursao.php
|
*/

function fatorial(int $n): int
{
    // TODO: implemente de forma recursiva
    return 0;
}

function fibonacci(int $n): int
{
    // TODO: implemente de forma recursiva
    return 0;
}

function potencia(int $base, int $expoente): int
{
    // TODO: implemente de forma recursiva
    return 0;
}

function somaDosDigitos(int $numero): int
{
    // TODO: implemente de forma recursiva
    return 0;
}

function inverterRecursivo(string $texto): string
{
    // TODO: implemente de forma recursiva
    return '';
}

// ---------------------------------------------------------------- saída

echo "--- Fatorial ---\n";

foreach ([0, 1, 5, 10] as $n) {
    echo "fatorial({$n}) = " . fatorial($n) . "\n";
}

echo "\n--- Fibonacci ---\n";

for ($i = 0; $i <= 10; $i++) {
    echo fibonacci($i) . ' ';
}

echo "\n\n--- Potência ---\n";
echo 'potencia(2, 10) = ' . potencia(2, 10) . "\n";
echo 'potencia(5, 0)  = ' . potencia(5, 0) . "\n";

echo "\n--- Soma dos dígitos ---\n";
echo 'somaDosDigitos(1234) = ' . somaDosDigitos(1234) . "\n";

echo "\n--- Inverter recursivo ---\n";
echo 'inverterRecursivo("obra") = ' . inverterRecursivo('obra') . "\n";
