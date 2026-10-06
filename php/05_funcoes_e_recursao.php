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
| RESPOSTA: cada chamada dispara outras duas, então o número de chamadas
| dobra a cada nível — fibonacci(40) faz mais de 300 milhões delas, e quase
| todas recalculam algo que já foi calculado. fibonacci(35) é computado
| milhares de vezes. Guardando cada resultado num array (memoização), cada
| valor é calculado uma vez só e o custo cai de exponencial para linear.
| A versão memoizada está no fim do arquivo.
|
| Rode com:  php php/05_funcoes_e_recursao.php
|
*/

function fatorial(int $n): int
{
    /*
     * O caso-base é o que encerra a recursão. Sem ele — ou com ele errado —
     * a função chama a si mesma até estourar a pilha.
     *
     * Vale para 0 e para 1 porque 0! = 1 e 1! = 1.
     */
    if ($n <= 1) {
        return 1;
    }

    return $n * fatorial($n - 1);
}

function fibonacci(int $n): int
{
    // Dois casos-base, porque a conta depende dos DOIS anteriores.
    if ($n <= 0) {
        return 0;
    }

    if ($n === 1) {
        return 1;
    }

    return fibonacci($n - 1) + fibonacci($n - 2);
}

function potencia(int $base, int $expoente): int
{
    // Qualquer número elevado a 0 é 1 — e é esse o caso-base.
    if ($expoente <= 0) {
        return 1;
    }

    return $base * potencia($base, $expoente - 1);
}

function somaDosDigitos(int $numero): int
{
    $numero = abs($numero);

    // Um dígito só: não há o que somar.
    if ($numero < 10) {
        return $numero;
    }

    // % 10 pega o último dígito; intdiv por 10 descarta ele e segue.
    return ($numero % 10) + somaDosDigitos(intdiv($numero, 10));
}

function inverterRecursivo(string $texto): string
{
    // String vazia já está invertida — caso-base.
    if ($texto === '') {
        return '';
    }

    // Tira a primeira letra, inverte o resto, e joga a primeira no fim.
    return inverterRecursivo(mb_substr($texto, 1)) . mb_substr($texto, 0, 1);
}

/**
 * A mesma conta do fibonacci, guardando o que já foi calculado.
 *
 * O array é `static`: ele sobrevive entre as chamadas, inclusive entre as
 * chamadas recursivas. Com ele, fibonacci(40) responde na hora.
 */
function fibonacciMemoizado(int $n): int
{
    static $jaCalculados = [];

    if ($n <= 0) {
        return 0;
    }

    if ($n === 1) {
        return 1;
    }

    if (isset($jaCalculados[$n])) {
        return $jaCalculados[$n];
    }

    return $jaCalculados[$n] = fibonacciMemoizado($n - 1) + fibonacciMemoizado($n - 2);
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

echo "\n--- Memoização ---\n";

$inicio = hrtime(true);
$valor = fibonacciMemoizado(40);
$msDecorridos = (hrtime(true) - $inicio) / 1_000_000;

echo 'fibonacciMemoizado(40) = ' . $valor . "\n";
echo 'tempo: ' . round($msDecorridos, 3) . " ms\n";
echo "(a versão sem memoização levaria segundos para o mesmo número)\n";
