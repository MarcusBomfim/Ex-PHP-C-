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
    $linhas = [];

    for ($i = 1; $i <= 10; $i++) {
        // Chaves em volta da expressão: sem elas, "$numero * $i" dentro da
        // string não é calculado — só $numero seria substituído.
        $linhas[] = "{$numero} x {$i} = " . ($numero * $i);
    }

    return $linhas;
}

function somaAte(int $limite): int
{
    $soma = 0;

    // Com $limite igual a 0 (ou negativo) o laço não entra nenhuma vez e a
    // soma fica em 0, que é a resposta certa.
    for ($i = 1; $i <= $limite; $i++) {
        $soma += $i;
    }

    return $soma;
}

function precoComDesconto(float $preco, float $percentual): float
{
    /*
     * Multiplicar pelo que SOBRA (1 - 15/100 = 0.85) em vez de calcular o
     * desconto e subtrair. Dá no mesmo, com uma operação a menos.
     *
     * O round() no fim não é enfeite: 59.9 * 0.85 dá 50.914999... em ponto
     * flutuante, e preço com sete casas decimais não existe.
     */
    return round($preco * (1 - $percentual / 100), 2);
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
