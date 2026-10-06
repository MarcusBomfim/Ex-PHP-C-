<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 04 — Arrays
|--------------------------------------------------------------------------
|
| Conteúdo: arrays indexados e associativos, foreach, array_filter,
| array_map, array_sum, sort e a diferença entre array vazio e null.
|
| 1) function media(array $notas): float
|    Média aritmética, arredondada para 2 casas. Array vazio devolve 0.0.
|
|    media([8.0, 6.0, 10.0]) -> 8.0
|    media([])               -> 0.0
|
| 2) function maiorEMenor(array $numeros): array
|    Devolve ['menor' => x, 'maior' => y]. Array vazio devolve
|    ['menor' => null, 'maior' => null].
|
|    maiorEMenor([3, 9, 1, 7]) -> ['menor' => 1, 'maior' => 9]
|
| 3) function apenasPares(array $numeros): array
|    Devolve só os pares, reindexando as chaves a partir de 0.
|    (Atenção: array_filter preserva as chaves originais.)
|
|    apenasPares([1, 2, 3, 4, 5, 6]) -> [2, 4, 6]
|
| 4) function contarOcorrencias(array $palavras): array
|    Mapa palavra => quantas vezes aparece, sem diferenciar maiúsculas.
|
|    contarOcorrencias(["obra", "Obra", "laje"]) -> ["obra" => 2, "laje" => 1]
|
| 5) function agruparPorInicial(array $nomes): array
|    Mapa letra inicial (maiúscula) => lista de nomes com aquela inicial.
|
|    agruparPorInicial(["ana", "bruno", "alice"])
|      -> ["A" => ["ana", "alice"], "B" => ["bruno"]]
|
| Rode com:  php php/04_arrays.php
|
*/

/** @param float[] $notas */
function media(array $notas): float
{
    // A guarda não é frescura: sem ela, a divisão por count([]) é divisão
    // por zero — DivisionByZeroError em PHP 8.
    if ($notas === []) {
        return 0.0;
    }

    return round(array_sum($notas) / count($notas), 2);
}

/**
 * @param  int[] $numeros
 * @return array{menor: int|null, maior: int|null}
 */
function maiorEMenor(array $numeros): array
{
    if ($numeros === []) {
        return ['menor' => null, 'maior' => null];
    }

    return ['menor' => min($numeros), 'maior' => max($numeros)];
}

/**
 * @param  int[] $numeros
 * @return int[]
 */
function apenasPares(array $numeros): array
{
    /*
     * O array_values é a parte que se esquece. array_filter PRESERVA as
     * chaves: de [1,2,3,4,5,6] sobrariam as posições 1, 3 e 5, e o array
     * resultante teria buracos. Ao serializar para JSON, um array com
     * buracos vira objeto {"1":2,...} em vez de lista [2,4,6].
     */
    return array_values(array_filter($numeros, static fn (int $n): bool => $n % 2 === 0));
}

/**
 * @param  string[] $palavras
 * @return array<string, int>
 */
function contarOcorrencias(array $palavras): array
{
    $contagem = [];

    foreach ($palavras as $palavra) {
        $chave = mb_strtolower($palavra);

        // ?? 0 cobre a primeira vez que a palavra aparece: sem isso, o
        // ++ num índice inexistente emite aviso.
        $contagem[$chave] = ($contagem[$chave] ?? 0) + 1;
    }

    return $contagem;
}

/**
 * @param  string[] $nomes
 * @return array<string, string[]>
 */
function agruparPorInicial(array $nomes): array
{
    $grupos = [];

    foreach ($nomes as $nome) {
        if ($nome === '') {
            continue;
        }

        $inicial = mb_strtoupper(mb_substr($nome, 0, 1));

        // Em PHP, atribuir a $grupos[$inicial][] cria o array interno
        // sozinho na primeira vez — não precisa inicializar antes.
        $grupos[$inicial][] = $nome;
    }

    return $grupos;
}

// ---------------------------------------------------------------- saída

echo "--- Média ---\n";
echo 'media([8, 6, 10]) = ' . media([8.0, 6.0, 10.0]) . "\n";

echo "\n--- Maior e menor ---\n";
print_r(maiorEMenor([3, 9, 1, 7]));

echo "\n--- Apenas pares ---\n";
print_r(apenasPares([1, 2, 3, 4, 5, 6]));

echo "\n--- Ocorrências ---\n";
print_r(contarOcorrencias(['obra', 'Obra', 'laje']));

echo "\n--- Agrupado por inicial ---\n";
print_r(agruparPorInicial(['ana', 'bruno', 'alice', 'carlos', 'bia']));
