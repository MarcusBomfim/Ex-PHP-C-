<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 01 — Variáveis, operadores e laços
|--------------------------------------------------------------------------
|
| Conteúdo: tipos (int, float, string), interpolação de string, laço `for`,
| round() e echo.
|
| 1) function tabuada(int $numero): array
|    Devolve um array com as 10 linhas da tabuada, no formato "3 x 1 = 3".
|    Use um laço `for` de 1 até 10.
|
|    tabuada(3)[0]  ->  "3 x 1 = 3"
|    tabuada(3)[9]  ->  "3 x 10 = 30"
|
| 2) function somaAte(int $limite): int
|    Soma todos os inteiros de 1 até $limite (inclusive).
|
|    somaAte(5) -> 15    (1+2+3+4+5)
|    somaAte(0) -> 0
|
| 3) function precoComDesconto(float $preco, float $percentual): float
|    Aplica um desconto percentual e arredonda para 2 casas decimais.
|
|    precoComDesconto(100.0, 10.0) -> 90.0
|    precoComDesconto(59.9, 15.0)  -> 50.92
|
| Rode com:  php php/01_variaveis_e_lacos.php
|
*/

/** @return string[] */
function tabuada(int $numero): array
{
    // TODO: implemente
    return [];
}

function somaAte(int $limite): int
{
    // TODO: implemente
    return 0;
}

function precoComDesconto(float $preco, float $percentual): float
{
    // TODO: implemente
    return 0.0;
}

// ---------------------------------------------------------------- saída

echo "--- Tabuada do 7 ---\n";

foreach (tabuada(7) as $linha) {
    echo $linha . "\n";
}

echo "\n--- Soma ---\n";
echo 'somaAte(5)  = ' . somaAte(5) . "\n";
echo 'somaAte(100) = ' . somaAte(100) . "\n";

echo "\n--- Desconto ---\n";
echo 'R$ 100,00 com 10% = ' . precoComDesconto(100.0, 10.0) . "\n";
echo 'R$ 59,90 com 15%  = ' . precoComDesconto(59.9, 15.0) . "\n";
