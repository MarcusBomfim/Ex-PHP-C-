<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 14 — Expressões regulares
|--------------------------------------------------------------------------
|
| Conteúdo: preg_match, preg_match_all, preg_replace, preg_split, grupos de
| captura, grupos nomeados e âncoras (^ e $).
|
| Aviso que vale para o exercício inteiro: regex é ótima para RECONHECER
| formato e péssima para VALIDAR significado. "000.000.000-00" passa em
| qualquer regex de CPF e não é um CPF válido — os dígitos verificadores
| são conta, não padrão. Aqui se confere formato; a validação de verdade é
| outro assunto.
|
| 1) temFormatoDeCpf("123.456.789-09")  -> true
|    temFormatoDeCpf("12345678909")     -> false  (sem pontuação)
|    temFormatoDeCpf("123.456.789-091") -> false  (sobrou dígito)
|
| 2) extrairNumeros("laje 3, pilar 12 e 7 vigas") -> [3, 12, 7]
|
| 3) mascararTelefone: (11) 91234-5678 -> (11) *****-5678
|
| 4) separarPorPontuacao("cimento, areia;  brita.") -> ["cimento","areia","brita"]
|
| 5) problemasDaSenha: lista o que falta, em vez de só "senha fraca".
|
| 6) lerLinhaDeLog("[2026-10-06 14:32:01] ERRO Falha ao gravar o diário")
|      -> ['data' => '2026-10-06 14:32:01', 'nivel' => 'ERRO',
|          'mensagem' => 'Falha ao gravar o diário']
|
| Rode com:  php php/14_regex.php
|
*/

function temFormatoDeCpf(string $texto): bool
{
    /*
     * O ^ e o $ são a parte que mais se esquece. Sem eles, a regex casa um
     * PEDAÇO de qualquer texto: "123.456.789-091" passaria, porque os 14
     * primeiros caracteres formam o padrão e o "1" sobrando é ignorado.
     *
     * preg_match devolve 1, 0 ou false (em erro de sintaxe da regex). O
     * === 1 cobre os três casos; um `if (preg_match(...))` trataria o
     * false como verdadeiro.
     */
    return preg_match('/^\d{3}\.\d{3}\.\d{3}-\d{2}$/', $texto) === 1;
}

/** @return int[] */
function extrairNumeros(string $texto): array
{
    // preg_match_all enche $achados por referência; o retorno é a
    // quantidade de casamentos, não a lista.
    preg_match_all('/\d+/', $texto, $achados);

    return array_map(intval(...), $achados[0]);
}

function mascararTelefone(string $texto): string
{
    /*
     * Os parênteses do telefone precisam ser escapados — \( e \) —, porque
     * parêntese solto em regex abre grupo de captura.
     *
     * $1 e $2 na substituição são os grupos capturados: o DDD e os quatro
     * últimos dígitos. O miolo some atrás dos asteriscos.
     */
    return preg_replace('/\((\d{2})\)\s*\d{5}-(\d{4})/', '($1) *****-$2', $texto) ?? $texto;
}

/** @return string[] */
function separarPorPontuacao(string $texto): array
{
    /*
     * O + depois da classe é o que resolve o "espaços repetidos": ele faz
     * a regex consumir a sequência inteira de separadores de uma vez.
     * Sem ele, "areia;  brita" produziria dois itens vazios no meio.
     *
     * PREG_SPLIT_NO_EMPTY descarta o que sobra nas pontas — o ponto final
     * da frase, por exemplo, geraria um item vazio no fim.
     */
    return preg_split('/[\s,;.]+/', trim($texto), -1, PREG_SPLIT_NO_EMPTY) ?: [];
}

/** @return string[] */
function problemasDaSenha(string $senha): array
{
    $problemas = [];

    // mb_strlen, e não strlen: senha com acento tem mais bytes que letras,
    // e contar bytes deixaria passar uma senha mais curta do que parece.
    if (mb_strlen($senha) < 8) {
        $problemas[] = 'ao menos 8 caracteres';
    }

    $exigencias = [
        '/[A-Z]/' => 'ao menos uma letra maiúscula',
        '/[a-z]/' => 'ao menos uma letra minúscula',
        '/\d/' => 'ao menos um número',
        // "Símbolo" definido pela negativa: o que não é letra nem número.
        // Listar os símbolos aceitos deixaria de fora os que ninguém lembrou.
        '/[^a-zA-Z0-9]/' => 'ao menos um símbolo',
    ];

    foreach ($exigencias as $padrao => $descricao) {
        if (preg_match($padrao, $senha) !== 1) {
            $problemas[] = $descricao;
        }
    }

    return $problemas;
}

/** @return array{data: string, nivel: string, mensagem: string}|null */
function lerLinhaDeLog(string $linha): ?array
{
    /*
     * Grupos NOMEADOS: (?<data>...), (?<nivel>...), (?<mensagem>...).
     *
     * A alternativa é $achados[1], [2], [3] — e aí basta alguém inserir um
     * grupo no meio da regex para todos os índices de baixo saírem do
     * lugar em silêncio. Com nome, inserir grupo não quebra nada.
     *
     * O nível é uma lista fechada (ERRO|AVISO|INFO) em vez de \w+: assim a
     * função recusa uma linha com nível inventado em vez de aceitar
     * qualquer palavra ali.
     */
    $padrao = '/^\[(?<data>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\] '
        . '(?<nivel>ERRO|AVISO|INFO) '
        . '(?<mensagem>.+)$/u';

    if (preg_match($padrao, $linha, $achados) !== 1) {
        return null;
    }

    return [
        'data' => $achados['data'],
        'nivel' => $achados['nivel'],
        'mensagem' => $achados['mensagem'],
    ];
}

// ---------------------------------------------------------------- saída

echo "--- Formato de CPF ---\n";

foreach (['123.456.789-09', '12345678909', '123.456.789-091', 'abc.def.ghi-jk'] as $texto) {
    printf("  %-18s %s\n", $texto, temFormatoDeCpf($texto) ? 'formato ok' : 'formato inválido');
}

echo "\n--- Extrair números ---\n";
echo '  ' . implode(', ', extrairNumeros('laje 3, pilar 12 e 7 vigas')) . "\n";

echo "\n--- Mascarar telefone ---\n";
echo '  ' . mascararTelefone('Ligar para (11) 91234-5678 ou (13) 99876-5432.') . "\n";

echo "\n--- Separar por pontuação ---\n";
echo '  [' . implode(' | ', separarPorPontuacao('cimento, areia;  brita.')) . "]\n";

echo "\n--- Força da senha ---\n";

foreach (['abc', 'Senha123', 'Senha@123'] as $senha) {
    $problemas = problemasDaSenha($senha);

    echo "  \"{$senha}\": " . ($problemas === [] ? 'ok' : implode('; ', $problemas)) . "\n";
}

echo "\n--- Linha de log ---\n";

$linhas = [
    '[2026-10-06 14:32:01] ERRO Falha ao gravar o diário',
    '[2026-10-06 14:35:40] INFO Medição 7 fechada',
    'isto não é uma linha de log',
];

foreach ($linhas as $linha) {
    $partes = lerLinhaDeLog($linha);

    if ($partes === null) {
        echo "  (fora do formato) {$linha}\n";

        continue;
    }

    printf("  %s [%s] %s\n", $partes['data'], $partes['nivel'], $partes['mensagem']);
}
