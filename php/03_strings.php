<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 03 — Strings
|--------------------------------------------------------------------------
|
| Conteúdo: funções de string do PHP (strrev, strtolower, str_replace,
| explode, implode, mb_strlen) e a diferença entre as versões `mb_` e as
| comuns quando há acento.
|
| 1) function inverter(string $texto): string
|    inverter("obra") -> "arbo"
|
| 2) function ehPalindromo(string $texto): bool
|    Ignora maiúsculas e espaços.
|
|    ehPalindromo("arara")          -> true
|    ehPalindromo("Ana")            -> true
|    ehPalindromo("socorram me")    -> false
|    ehPalindromo("A mala nada na lama") -> true
|
| 3) function contarVogais(string $texto): int
|    Conta a, e, i, o, u — maiúsculas e minúsculas.
|
|    contarVogais("concreto") -> 3
|    contarVogais("XYZ")      -> 0
|
| 4) function iniciais(string $nomeCompleto): string
|    Devolve as iniciais em maiúsculas, separadas por ponto.
|
|    iniciais("marcus bomfim silva") -> "M.B.S."
|    iniciais("Ana")                 -> "A."
|
| 5) function mascararEmail(string $email): string
|    Esconde o usuário, deixando só a primeira letra.
|
|    mascararEmail("marcus@obra.dev") -> "m*****@obra.dev"
|    O número de asteriscos é o tamanho do resto do usuário.
|
| Rode com:  php php/03_strings.php
|
*/

function inverter(string $texto): string
{
    // TODO: implemente
    return '';
}

function ehPalindromo(string $texto): bool
{
    // TODO: implemente
    return false;
}

function contarVogais(string $texto): int
{
    // TODO: implemente
    return 0;
}

function iniciais(string $nomeCompleto): string
{
    // TODO: implemente
    return '';
}

function mascararEmail(string $email): string
{
    // TODO: implemente
    return '';
}

// ---------------------------------------------------------------- saída

echo "--- Inverter ---\n";
echo 'inverter("obra") = ' . inverter('obra') . "\n";

echo "\n--- Palíndromo ---\n";

foreach (['arara', 'Ana', 'socorram me', 'A mala nada na lama'] as $texto) {
    echo '"' . $texto . '": ' . (ehPalindromo($texto) ? 'sim' : 'não') . "\n";
}

echo "\n--- Vogais ---\n";
echo 'contarVogais("concreto") = ' . contarVogais('concreto') . "\n";

echo "\n--- Iniciais ---\n";
echo 'iniciais("marcus bomfim silva") = ' . iniciais('marcus bomfim silva') . "\n";

echo "\n--- E-mail mascarado ---\n";
echo mascararEmail('marcus@obra.dev') . "\n";
