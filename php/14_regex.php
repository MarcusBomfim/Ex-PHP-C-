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
| 1) function temFormatoDeCpf(string $texto): bool
|    Aceita exatamente 000.000.000-00.
|
|    temFormatoDeCpf("123.456.789-09")  -> true
|    temFormatoDeCpf("12345678909")     -> false  (sem pontuação)
|    temFormatoDeCpf("123.456.789-091") -> false  (sobrou dígito)
|
|    Dica: sem o ^ e o $, a regex casa um pedaço no meio de qualquer
|    texto — e é por isso que a terceira linha acima costuma passar por
|    engano.
|
| 2) function extrairNumeros(string $texto): array
|    Todos os números inteiros do texto, como int.
|
|    extrairNumeros("laje 3, pilar 12 e 7 vigas") -> [3, 12, 7]
|
| 3) function mascararTelefone(string $texto): string
|    Troca todo telefone no formato (11) 91234-5678 por (11) *****-5678.
|
| 4) function separarPorPontuacao(string $texto): array
|    Divide em palavras, descartando vírgula, ponto, ponto e vírgula e
|    espaços repetidos. Sem itens vazios no resultado.
|
|    separarPorPontuacao("cimento, areia;  brita.") -> ["cimento","areia","brita"]
|
| 5) function problemasDaSenha(string $senha): array
|    Lista o que falta, em vez de só dizer "senha fraca":
|      - "ao menos 8 caracteres"
|      - "ao menos uma letra maiúscula"
|      - "ao menos uma letra minúscula"
|      - "ao menos um número"
|      - "ao menos um símbolo"
|    Senha boa devolve [].
|
| 6) function lerLinhaDeLog(string $linha): ?array
|    Extrai de "[2026-10-06 14:32:01] ERRO Falha ao gravar o diário"
|    o array ['data' => '2026-10-06 14:32:01', 'nivel' => 'ERRO',
|             'mensagem' => 'Falha ao gravar o diário'].
|    Linha fora do formato devolve null.
|
|    Dica: use grupos NOMEADOS — (?<nivel>ERRO|AVISO|INFO) — para ler o
|    resultado por nome em vez de por número. Regex com oito grupos
|    numerados é impossível de manter.
|
| Rode com:  php php/14_regex.php
|
*/

function temFormatoDeCpf(string $texto): bool
{
    // TODO: implemente
    return false;
}

/** @return int[] */
function extrairNumeros(string $texto): array
{
    // TODO: implemente
    return [];
}

function mascararTelefone(string $texto): string
{
    // TODO: implemente
    return '';
}

/** @return string[] */
function separarPorPontuacao(string $texto): array
{
    // TODO: implemente
    return [];
}

/** @return string[] */
function problemasDaSenha(string $senha): array
{
    // TODO: implemente
    return [];
}

/** @return array{data: string, nivel: string, mensagem: string}|null */
function lerLinhaDeLog(string $linha): ?array
{
    // TODO: implemente
    return null;
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
