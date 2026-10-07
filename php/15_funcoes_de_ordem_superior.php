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
| Nos três primeiros, IMPLEMENTE À MÃO — nada de array_map, array_filter
| ou array_reduce. O objetivo é entender o que eles fazem por dentro.
|
| 1) function aplicarEmTodos(array $itens, callable $transformacao): array
|    aplicarEmTodos([1,2,3], fn($n) => $n * 2) -> [2, 4, 6]
|
| 2) function filtrar(array $itens, callable $criterio): array
|    Reindexa o resultado a partir de 0.
|    filtrar([1,2,3,4], fn($n) => $n % 2 === 0) -> [2, 4]
|
| 3) function reduzir(array $itens, callable $acumulador, mixed $inicial): mixed
|    O acumulador recebe (acumulado, item) e devolve o novo acumulado.
|    reduzir([1,2,3], fn($t, $n) => $t + $n, 0) -> 6
|    reduzir(["a","b"], fn($t, $s) => $t . $s, "") -> "ab"
|
| 4) function compor(callable $depois, callable $antes): callable
|    Devolve uma função que roda $antes e passa o resultado para $depois.
|
|    $dobrarEIncrementar = compor(fn($n) => $n + 1, fn($n) => $n * 2);
|    $dobrarEIncrementar(5) -> 11    (5*2 = 10, depois +1)
|
| 5) function criarContador(int $inicio = 0): callable
|    Devolve uma função que, a cada chamada, devolve o próximo número.
|    Dois contadores criados separadamente NÃO compartilham estado.
|
|    $c = criarContador();  $c() -> 0;  $c() -> 1;  $c() -> 2
|
|    Dica: é aqui que entra o `use (&$variavel)` — por REFERÊNCIA. Com
|    `use ($variavel)` a closure guarda uma cópia e o contador trava no
|    valor inicial para sempre.
|
| 6) function memoizar(callable $funcao): callable
|    Devolve uma versão que guarda os resultados já calculados. Chamar duas
|    vezes com o mesmo argumento executa a função original uma vez só.
|
| 7) function agruparPor(array $itens, callable $chave): array
|    Agrupa pelo resultado da função.
|
|    agruparPor(["ana","bruno","alice"], fn($n) => $n[0])
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
    // TODO: implemente à mão, com foreach — sem array_map
    return [];
}

/**
 * @param  array<int, mixed> $itens
 * @param  callable(mixed): bool $criterio
 * @return array<int, mixed>
 */
function filtrar(array $itens, callable $criterio): array
{
    // TODO: implemente à mão, com foreach — sem array_filter
    return [];
}

/**
 * @param array<int, mixed> $itens
 * @param callable(mixed, mixed): mixed $acumulador
 */
function reduzir(array $itens, callable $acumulador, mixed $inicial): mixed
{
    // TODO: implemente à mão, com foreach — sem array_reduce
    return $inicial;
}

/**
 * @param  callable(mixed): mixed $depois
 * @param  callable(mixed): mixed $antes
 * @return callable(mixed): mixed
 */
function compor(callable $depois, callable $antes): callable
{
    // TODO: devolva uma closure que encadeia as duas
    return static fn (mixed $valor): mixed => $valor;
}

/** @return callable(): int */
function criarContador(int $inicio = 0): callable
{
    // TODO: devolva uma closure com estado próprio
    return static fn (): int => 0;
}

/**
 * @param  callable $funcao
 * @return callable
 */
function memoizar(callable $funcao): callable
{
    // TODO: devolva uma versão que guarda os resultados
    return $funcao;
}

/**
 * @param  array<int, mixed> $itens
 * @param  callable(mixed): string $chave
 * @return array<string, array<int, mixed>>
 */
function agruparPor(array $itens, callable $chave): array
{
    // TODO: implemente
    return [];
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
