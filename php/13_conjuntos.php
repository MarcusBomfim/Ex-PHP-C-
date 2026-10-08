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
| 1) semRepetidos(["a","b","a","c","b"]) -> ["a","b","c"]
|    A ordem da primeira aparição é mantida.
|
| 2) uniao([1,2,3], [3,4])              -> [1,2,3,4]
| 3) intersecao([1,2,3], [2,3,4])       -> [2,3]
| 4) diferenca([1,2,3], [2])            -> [1,3]
| 5) diferencaSimetrica([1,2,3], [3,4]) -> [1,2,4]
|
| 6) contemTodos([1,2,3], [1,3]) -> true
|    contemTodos([1,2,3], [1,9]) -> false
|    contemTodos([1,2,3], [])    -> true   (conjunto vazio cabe em tudo)
|
| 7) maisFrequentes(["a","b","a","c","a","b"], 2) -> ["a" => 3, "b" => 2]
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
    /*
     * array_unique mantém a PRIMEIRA aparição de cada valor e preserva as
     * chaves originais — então sobram buracos. O array_values reindexa.
     *
     * Sem ele, o resultado serializado para JSON viraria objeto
     * {"0":"a","1":"b","3":"c"} em vez de lista. É a mesma armadilha do
     * array_filter no exercício 04.
     */
    return array_values(array_unique($itens));
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function uniao(array $a, array $b): array
{
    // Junta tudo e tira as repetições. A ordem fica: primeiro os de $a,
    // depois os de $b que ainda não apareceram.
    return semRepetidos(array_merge($a, $b));
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function intersecao(array $a, array $b): array
{
    /*
     * array_intersect devolve os elementos de $a que existem em $b —
     * inclusive repetidos, se $a os tiver. Daí o semRepetidos por cima:
     * conjunto não tem elemento repetido.
     */
    return semRepetidos(array_intersect($a, $b));
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function diferenca(array $a, array $b): array
{
    return semRepetidos(array_diff($a, $b));
}

/**
 * @param  array<int, string|int> $a
 * @param  array<int, string|int> $b
 * @return array<int, string|int>
 */
function diferencaSimetrica(array $a, array $b): array
{
    // "O que só está em A" mais "o que só está em B". Montada a partir das
    // duas funções acima em vez de refazer a conta.
    return uniao(diferenca($a, $b), diferenca($b, $a));
}

/**
 * @param array<int, string|int> $conjunto
 * @param array<int, string|int> $itens
 */
function contemTodos(array $conjunto, array $itens): bool
{
    /*
     * "Nada em $itens está fora de $conjunto." Escrito assim, o caso do
     * array vazio sai de graça: nada fora de nada, logo true — que é a
     * definição matemática de subconjunto.
     *
     * Um foreach com flag daria o mesmo resultado, mas exigiria lembrar
     * de inicializar a flag como true.
     */
    return array_diff($itens, $conjunto) === [];
}

/**
 * @param  string[] $itens
 * @return array<string, int>
 */
function maisFrequentes(array $itens, int $quantos): array
{
    // array_count_values já devolve [valor => quantidade].
    $contagem = array_count_values($itens);

    /*
     * arsort ordena pelo VALOR, do maior para o menor, preservando a
     * ligação chave => valor. Um sort() comum jogaria as chaves fora e
     * sobrariam só os números.
     */
    arsort($contagem);

    // O `true` preserva as chaves; sem ele, array_slice reindexaria para
    // 0, 1, 2 e os nomes se perderiam.
    return array_slice($contagem, 0, max(0, $quantos), true);
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
