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
    /*
     * strrev inverte BYTE a byte, não letra a letra. Para "obra" dá certo;
     * para "ação" não, porque o "ç" ocupa dois bytes em UTF-8 e eles saem
     * trocados. Com acento, o caminho é separar em caracteres de verdade:
     *
     *   implode('', array_reverse(mb_str_split($texto)))
     */
    return strrev($texto);
}

function ehPalindromo(string $texto): bool
{
    // Primeiro normaliza — minúsculas e sem espaço —, depois compara com
    // a própria inversão. Comparar antes de normalizar reprovaria "Ana".
    $limpo = str_replace(' ', '', mb_strtolower($texto));

    return $limpo === strrev($limpo);
}

function contarVogais(string $texto): int
{
    $vogais = ['a', 'e', 'i', 'o', 'u'];
    $total = 0;

    foreach (mb_str_split(mb_strtolower($texto)) as $letra) {
        if (in_array($letra, $vogais, true)) {
            $total++;
        }
    }

    return $total;
}

function iniciais(string $nomeCompleto): string
{
    $iniciais = '';

    // explode(' ') devolve entrada vazia quando há dois espaços seguidos;
    // o array_filter tira essas, para " marcus  silva " não virar "..M..S.".
    $partes = array_filter(explode(' ', trim($nomeCompleto)), static fn (string $p): bool => $p !== '');

    foreach ($partes as $parte) {
        $iniciais .= mb_strtoupper(mb_substr($parte, 0, 1)) . '.';
    }

    return $iniciais;
}

function mascararEmail(string $email): string
{
    $arroba = mb_strpos($email, '@');

    // Sem arroba não é e-mail: devolve como veio em vez de inventar.
    if ($arroba === false || $arroba === 0) {
        return $email;
    }

    $usuario = mb_substr($email, 0, $arroba);
    $dominio = mb_substr($email, $arroba);

    return mb_substr($usuario, 0, 1)
        . str_repeat('*', mb_strlen($usuario) - 1)
        . $dominio;
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
