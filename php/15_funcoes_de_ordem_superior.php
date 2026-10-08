<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 15 — Funções de ordem superior e closures
|--------------------------------------------------------------------------
|
| Conteúdo: callable, closures, `use` por valor e por referência, funções
| que recebem e devolvem funções, e o que é estado capturado.
|
| Função de ordem superior é a que recebe outra função como argumento, ou
| devolve uma. `usort` é uma: recebe a comparação. Este exercício é sobre
| escrever as suas.
|
| Nos três primeiros, IMPLEMENTADO À MÃO — nada de array_map, array_filter
| ou array_reduce. O objetivo é entender o que eles fazem por dentro.
|
| 1) aplicarEmTodos([1,2,3], fn($n) => $n * 2) -> [2, 4, 6]
| 2) filtrar([1,2,3,4], fn($n) => $n % 2 === 0) -> [2, 4]
| 3) reduzir([1,2,3], fn($t, $n) => $t + $n, 0) -> 6
|
| 4) compor(fn($n) => $n + 1, fn($n) => $n * 2)(5) -> 11
| 5) criarContador(): $c() -> 0, 1, 2...  (dois contadores não se misturam)
| 6) memoizar: chamar duas vezes com o mesmo argumento executa uma vez só
| 7) agruparPor(["ana","bruno","alice"], fn($n) => $n[0])
|      -> ["a" => ["ana","alice"], "b" => ["bruno"]]
|
| Rode com:  php php/15_funcoes_de_ordem_superior.php
|
*/

/**
 * @param  array<int, mixed> $itens
 * @param  callable(mixed): mixed $transformacao
 * @return array<int, mixed>
 */
function aplicarEmTodos(array $itens, callable $transformacao): array
{
    $resultado = [];

    // É literalmente isto que o array_map faz: percorre e guarda o que a
    // função devolveu, na mesma ordem.
    foreach ($itens as $item) {
        $resultado[] = $transformacao($item);
    }

    return $resultado;
}

/**
 * @param  array<int, mixed> $itens
 * @param  callable(mixed): bool $criterio
 * @return array<int, mixed>
 */
function filtrar(array $itens, callable $criterio): array
{
    $resultado = [];

    foreach ($itens as $item) {
        if ($criterio($item)) {
            // $resultado[] já reindexa sozinho. É por isso que esta versão
            // não precisa do array_values que o array_filter exige: lá as
            // chaves originais são preservadas, aqui nascem novas.
            $resultado[] = $item;
        }
    }

    return $resultado;
}

/**
 * @param array<int, mixed> $itens
 * @param callable(mixed, mixed): mixed $acumulador
 */
function reduzir(array $itens, callable $acumulador, mixed $inicial): mixed
{
    $acumulado = $inicial;

    /*
     * O valor inicial não é detalhe: ele define o tipo do resultado e o
     * que sai quando a lista é vazia. Somar começa em 0, concatenar começa
     * em "", multiplicar começa em 1.
     *
     * Reduzir sem valor inicial — usando o primeiro item — quebra na lista
     * vazia, e é por isso que esta assinatura o exige.
     */
    foreach ($itens as $item) {
        $acumulado = $acumulador($acumulado, $item);
    }

    return $acumulado;
}

/**
 * @param  callable(mixed): mixed $depois
 * @param  callable(mixed): mixed $antes
 * @return callable(mixed): mixed
 */
function compor(callable $depois, callable $antes): callable
{
    /*
     * A ordem de leitura é de dentro para fora: $antes roda primeiro, e o
     * resultado dele alimenta $depois. É a mesma convenção da matemática,
     * onde (f ∘ g)(x) significa f(g(x)).
     */
    return static fn (mixed $valor): mixed => $depois($antes($valor));
}

/** @return callable(): int */
function criarContador(int $inicio = 0): callable
{
    $proximo = $inicio;

    /*
     * `use (&$proximo)` — por REFERÊNCIA. É o ponto do exercício.
     *
     * Com `use ($proximo)`, a closure guardaria uma CÓPIA do valor no
     * momento em que foi criada, e o $proximo++ lá dentro incrementaria a
     * cópia. O contador devolveria sempre o mesmo número.
     *
     * Cada chamada a criarContador() cria um $proximo novo, e é por isso
     * que dois contadores não compartilham estado.
     */
    return static function () use (&$proximo): int {
        return $proximo++;
    };
}

/**
 * @param  callable $funcao
 * @return callable
 */
function memoizar(callable $funcao): callable
{
    $cache = [];

    return static function (mixed ...$argumentos) use ($funcao, &$cache): mixed {
        /*
         * serialize transforma os argumentos numa string única — é o que
         * permite usar array, objeto ou vários argumentos como chave, e
         * não só escalares.
         *
         * array_key_exists em vez de isset: um resultado que seja null é
         * um resultado válido, e o isset o trataria como "não está no
         * cache", recalculando para sempre.
         */
        $chave = serialize($argumentos);

        if (!array_key_exists($chave, $cache)) {
            $cache[$chave] = $funcao(...$argumentos);
        }

        return $cache[$chave];
    };
}

/**
 * @param  array<int, mixed> $itens
 * @param  callable(mixed): string $chave
 * @return array<string, array<int, mixed>>
 */
function agruparPor(array $itens, callable $chave): array
{
    $grupos = [];

    foreach ($itens as $item) {
        // $grupos[$k][] cria o array interno na primeira vez — detalhe do
        // PHP que o C# não tem (lá é preciso criar a lista antes).
        $grupos[$chave($item)][] = $item;
    }

    return $grupos;
}

// ---------------------------------------------------------------- saída

$lista = static fn (array $itens): string => '[' . implode(', ', array_map(strval(...), $itens)) . ']';

echo "--- Aplicar em todos ---\n";
echo '  dobrar [1,2,3] = ' . $lista(aplicarEmTodos([1, 2, 3], static fn (int $n): int => $n * 2)) . "\n";
echo '  nomes em maiúsculas = ' . $lista(
    aplicarEmTodos(['ana', 'bruno'], static fn (string $s): string => mb_strtoupper($s)),
) . "\n";

echo "\n--- Filtrar ---\n";
echo '  pares de [1,2,3,4,5,6] = ' . $lista(
    filtrar([1, 2, 3, 4, 5, 6], static fn (int $n): bool => $n % 2 === 0),
) . "\n";

echo "\n--- Reduzir ---\n";
echo '  soma de [1,2,3,4] = ' . reduzir([1, 2, 3, 4], static fn (int $t, int $n): int => $t + $n, 0) . "\n";
echo '  juntar ["a","b","c"] = "'
    . reduzir(['a', 'b', 'c'], static fn (string $t, string $s): string => $t . $s, '') . "\"\n";
echo '  maior de [3,9,1] = ' . reduzir([3, 9, 1], static fn (int $t, int $n): int => max($t, $n), 0) . "\n";

echo "\n--- Composição ---\n";
$dobrarEIncrementar = compor(
    static fn (int $n): int => $n + 1,
    static fn (int $n): int => $n * 2,
);
echo '  compor(+1, *2)(5) = ' . $dobrarEIncrementar(5) . "   (5*2 = 10, depois +1)\n";

echo "\n--- Contador com estado ---\n";
$primeiro = criarContador();
$segundo = criarContador(100);

echo '  primeiro: ' . $primeiro() . ', ' . $primeiro() . ', ' . $primeiro() . "\n";
echo '  segundo : ' . $segundo() . ', ' . $segundo() . "\n";
echo '  primeiro de novo: ' . $primeiro() . "   (os dois não se misturam)\n";

echo "\n--- Memoização ---\n";
$vezesChamada = 0;

$lenta = function (int $n) use (&$vezesChamada): int {
    $vezesChamada++;
    usleep(1000);

    return $n * $n;
};

$rapida = memoizar($lenta);

echo '  rapida(9) = ' . $rapida(9) . "\n";
echo '  rapida(9) = ' . $rapida(9) . "\n";
echo '  rapida(4) = ' . $rapida(4) . "\n";
echo "  a função original foi chamada {$vezesChamada} vez(es) — deveria ser 2\n";

echo "\n--- Agrupar por ---\n";

foreach (agruparPor(['ana', 'bruno', 'alice', 'carlos', 'bia'], static fn (string $n): string => $n[0]) as $letra => $nomes) {
    echo "  {$letra}: " . implode(', ', $nomes) . "\n";
}

echo "\n--- O que acontece com `use` por valor ---\n";

$porValor = static function (): callable {
    $n = 0;

    // Sem o & — a closure guarda uma cópia.
    return static function () use ($n): int {
        return $n++;
    };
};

$travado = $porValor();
echo '  contador sem &: ' . $travado() . ', ' . $travado() . ', ' . $travado() . "   (trava no 0)\n";
