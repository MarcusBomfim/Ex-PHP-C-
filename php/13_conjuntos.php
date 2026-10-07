<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 13 — Conjuntos
|--------------------------------------------------------------------------
|
| Conteúdo: array_unique, array_values, array_intersect, array_diff,
| array_merge, array_count_values — e a ideia de conjunto, que o PHP não
| tem como tipo nativo.
|
| Um conjunto é uma coleção SEM repetição e SEM ordem. O PHP não tem a
| classe; o que se usa é um array comum com cuidado manual, ou um array
| associativo cujas CHAVES são os elementos (chave não repete, por
| definição). O C# tem HashSet<T> pronto — compare os dois lados.
|
| Todas as funções devem devolver arrays reindexados a partir de 0.
|
| 1) function semRepetidos(array $itens): array
|    semRepetidos(["a", "b", "a", "c", "b"]) -> ["a", "b", "c"]
|    A ordem da primeira aparição é mantida.
|
| 2) function uniao(array $a, array $b): array
|    uniao([1, 2, 3], [3, 4]) -> [1, 2, 3, 4]
|
| 3) function intersecao(array $a, array $b): array
|    intersecao([1, 2, 3], [2, 3, 4]) -> [2, 3]
|
| 4) function diferenca(array $a, array $b): array
|    O que está em $a e NÃO está em $b.
|    diferenca([1, 2, 3], [2]) -> [1, 3]
|
| 5) function diferencaSimetrica(array $a, array $b): array
|    O que está em um ou no outro, mas não nos dois.
|    diferencaSimetrica([1, 2, 3], [3, 4]) -> [1, 2, 4]
|
| 6) function contemTodos(array $conjunto, array $itens): bool
|    contemTodos([1, 2, 3], [1, 3])    -> true
|    contemTodos([1, 2, 3], [1, 9])    -> false
|    contemTodos([1, 2, 3], [])        -> true   (conjunto vazio cabe em tudo)
|
| 7) function maisFrequentes(array $itens, int $quantos): array
|    Os que mais se repetem, do mais para o menos frequente.
|    Devolve [valor => quantidade].
|
|    maisFrequentes(["a","b","a","c","a","b"], 2) -> ["a" => 3, "b" => 2]
|
| Um caso de uso real: comparar as ferramentas que saíram do almoxarifado
| com as que voltaram. A diferença é o que sumiu.
|
| Rode com:  php php/13_conjuntos.php
|
*/

/**
 * @param  array<int, string|int> $itens
 * @return array<int, string|int>
 */
function semRepetidos(array $itens): array
{
    // TODO: implemente
    return [];
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function uniao(array $a, array $b): array
{
    // TODO: implemente
    return [];
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function intersecao(array $a, array $b): array
{
    // TODO: implemente
    return [];
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function diferenca(array $a, array $b): array
{
    // TODO: implemente
    return [];
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function diferencaSimetrica(array $a, array $b): array
{
    // TODO: implemente
    return [];
}

/**
 * @param array<int, string|int> $conjunto
 * @param array<int, string|int> $itens
 */
function contemTodos(array $conjunto, array $itens): bool
{
    // TODO: implemente
    return false;
}

/**
 * @param  string[] $itens
 * @return array<string, int>
 */
function maisFrequentes(array $itens, int $quantos): array
{
    // TODO: implemente
    return [];
}

// ---------------------------------------------------------------- saída

$lista = static fn (array $itens): string => '[' . implode(', ', array_map(strval(...), $itens)) . ']';

echo "--- Sem repetidos ---\n";
echo '  ' . $lista(semRepetidos(['a', 'b', 'a', 'c', 'b'])) . "\n";

$saiu = ['martelo', 'trena', 'nível', 'furadeira', 'martelo'];
$voltou = ['trena', 'martelo', 'esquadro'];

echo "\n--- Almoxarifado ---\n";
echo '  saiu ...: ' . $lista($saiu) . "\n";
echo '  voltou .: ' . $lista($voltou) . "\n";
echo '  união ..: ' . $lista(uniao($saiu, $voltou)) . "\n";
echo '  nos dois: ' . $lista(intersecao($saiu, $voltou)) . "\n";
echo '  sumiu ..: ' . $lista(diferenca($saiu, $voltou)) . "\n";
echo '  a mais .: ' . $lista(diferenca($voltou, $saiu)) . "  (voltou o que não saiu)\n";
echo '  simétr .: ' . $lista(diferencaSimetrica($saiu, $voltou)) . "\n";

echo "\n--- Contém todos ---\n";

foreach ([[1, 3], [1, 9], []] as $procurados) {
    echo '  ' . $lista([1, 2, 3]) . ' contém ' . $lista($procurados) . ': '
        . (contemTodos([1, 2, 3], $procurados) ? 'sim' : 'não') . "\n";
}

echo "\n--- Mais frequentes ---\n";

$apontamentos = ['pedreiro', 'servente', 'pedreiro', 'carpinteiro', 'pedreiro', 'servente'];

foreach (maisFrequentes($apontamentos, 2) as $funcao => $vezes) {
    echo "  {$funcao}: {$vezes}\n";
}
