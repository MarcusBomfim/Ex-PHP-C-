<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 11 — Arquivos e JSON
|--------------------------------------------------------------------------
|
| Conteúdo: file_put_contents, file_get_contents, json_encode, json_decode,
| JSON_THROW_ON_ERROR, CSV e por que conferir o retorno de toda operação de
| arquivo.
|
| 1) function salvarTexto(string $caminho, string $conteudo): void
|    Lança RuntimeException se não conseguir gravar.
|
| 2) function lerLinhas(string $caminho): array
|    Devolve as linhas sem o \n e sem as vazias. Arquivo inexistente lança.
|
| 3) function salvarComoJson(string $caminho, array $dados): void
|    JSON legível (indentado) e sem escapar acento.
|
| 4) function lerJson(string $caminho): array
|    Lança se o arquivo não existir OU se o conteúdo não for JSON válido.
|
| 5) function lerCsv(string $caminho): array
|    Primeira linha é o cabeçalho. Devolve lista de arrays associativos.
|
| 6) function anexarLinha(string $caminho, string $linha): void
|    Acrescenta no fim, sem apagar o que já estava.
|
| Tudo é escrito num diretório temporário e apagado no fim — exercício não
| deve deixar sujeira no repositório.
|
| Rode com:  php php/11_arquivos_e_json.php
|
*/

function salvarTexto(string $caminho, string $conteudo): void
{
    /*
     * file_put_contents devolve o número de bytes gravados, ou FALSE se
     * falhar. Ignorar esse retorno é o erro mais comum com arquivo: o
     * disco enche, a permissão falha, e o programa segue como se tivesse
     * salvado.
     *
     * O === false é obrigatório: gravar string vazia devolve 0, e 0 é
     * "falso" numa comparação solta.
     */
    if (file_put_contents($caminho, $conteudo) === false) {
        throw new RuntimeException("Não foi possível gravar em {$caminho}.");
    }
}

/** @return string[] */
function lerLinhas(string $caminho): array
{
    if (!is_file($caminho)) {
        throw new RuntimeException("Arquivo não encontrado: {$caminho}.");
    }

    $linhas = file($caminho, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);

    if ($linhas === false) {
        throw new RuntimeException("Não foi possível ler {$caminho}.");
    }

    return $linhas;
}

/** @param array<mixed> $dados */
function salvarComoJson(string $caminho, array $dados): void
{
    /*
     * As três flags, e o que cada uma resolve:
     *
     *  PRETTY_PRINT      — indentado, para dar para ler e versionar.
     *  UNESCAPED_UNICODE — sem isso, "concretagem" com acento vira
     *                      "ç" e o arquivo fica ilegível.
     *  THROW_ON_ERROR    — sem isso, json_encode devolve false em silêncio
     *                      quando encontra algo que não sabe serializar.
     */
    $json = json_encode(
        $dados,
        JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES | JSON_THROW_ON_ERROR,
    );

    salvarTexto($caminho, $json);
}

/** @return array<mixed> */
function lerJson(string $caminho): array
{
    if (!is_file($caminho)) {
        throw new RuntimeException("Arquivo não encontrado: {$caminho}.");
    }

    $conteudo = file_get_contents($caminho);

    if ($conteudo === false) {
        throw new RuntimeException("Não foi possível ler {$caminho}.");
    }

    // O `true` devolve array associativo em vez de stdClass.
    // JSON_THROW_ON_ERROR troca o "devolve null em silêncio" por exceção.
    return json_decode($conteudo, true, 512, JSON_THROW_ON_ERROR);
}

/** @return array<int, array<string, string>> */
function lerCsv(string $caminho): array
{
    $arquivo = fopen($caminho, 'r');

    if ($arquivo === false) {
        throw new RuntimeException("Não foi possível abrir {$caminho}.");
    }

    try {
        $cabecalho = fgetcsv($arquivo);

        if ($cabecalho === false) {
            return [];
        }

        $linhas = [];

        while (($campos = fgetcsv($arquivo)) !== false) {
            // Linha em branco vem como [null] — não é dado.
            if ($campos === [null]) {
                continue;
            }

            // array_combine casa cabeçalho com valores: ['nome' => 'Ana', ...]
            $linhas[] = array_combine($cabecalho, $campos);
        }

        return $linhas;
    } finally {
        /*
         * O finally garante o fclose mesmo se o laço lançar no meio.
         * Arquivo aberto e não fechado é descritor vazando — num processo
         * de longa duração, acaba em "too many open files".
         */
        fclose($arquivo);
    }
}

function anexarLinha(string $caminho, string $linha): void
{
    /*
     * FILE_APPEND escreve no fim em vez de truncar. LOCK_EX trava o
     * arquivo durante a escrita: sem ele, dois processos gravando ao
     * mesmo tempo produzem linhas embaralhadas.
     */
    if (file_put_contents($caminho, $linha . PHP_EOL, FILE_APPEND | LOCK_EX) === false) {
        throw new RuntimeException("Não foi possível anexar em {$caminho}.");
    }
}

// ---------------------------------------------------------------- saída

$pasta = sys_get_temp_dir() . DIRECTORY_SEPARATOR . 'ex-php-csharp-' . bin2hex(random_bytes(4));
mkdir($pasta, 0o755, true);

try {
    echo "--- Texto ---\n";
    $texto = $pasta . DIRECTORY_SEPARATOR . 'anotacoes.txt';

    salvarTexto($texto, "primeira linha\nsegunda linha\n");
    anexarLinha($texto, 'terceira linha');

    foreach (lerLinhas($texto) as $numero => $linha) {
        echo '  ' . ($numero + 1) . ': ' . $linha . "\n";
    }

    echo "\n--- JSON ---\n";
    $json = $pasta . DIRECTORY_SEPARATOR . 'obra.json';

    salvarComoJson($json, [
        'codigo' => 'OBR-2026-007',
        'nome' => 'Edifício Vista Serra',
        'concretagens' => 6,
        'elementos' => ['laje', 'pilar', 'viga'],
    ]);

    echo file_get_contents($json) . "\n";

    $lido = lerJson($json);
    echo 'Lido de volta: ' . $lido['nome'] . ' com ' . $lido['concretagens'] . " concretagens\n";
    echo 'Elementos: ' . implode(', ', $lido['elementos']) . "\n";

    echo "\n--- CSV ---\n";
    $csv = $pasta . DIRECTORY_SEPARATOR . 'equipe.csv';

    salvarTexto($csv, "nome,funcao,horas\nAna,Engenheira,180\nBruno,Pedreiro,200\nCarla,Mestre,190\n");

    foreach (lerCsv($csv) as $pessoa) {
        printf("  %-8s %-12s %s h\n", $pessoa['nome'], $pessoa['funcao'], $pessoa['horas']);
    }

    echo "\n--- O que deve dar erro ---\n";

    try {
        lerJson($pasta . DIRECTORY_SEPARATOR . 'nao-existe.json');
        echo "ERRO: leu arquivo inexistente!\n";
    } catch (RuntimeException $erro) {
        echo 'Recusado, como esperado: ' . $erro->getMessage() . "\n";
    }

    $quebrado = $pasta . DIRECTORY_SEPARATOR . 'quebrado.json';
    salvarTexto($quebrado, '{ isto não é json }');

    try {
        lerJson($quebrado);
        echo "ERRO: aceitou JSON inválido!\n";
    } catch (JsonException $erro) {
        echo 'Recusado, como esperado: ' . $erro->getMessage() . "\n";
    }
} finally {
    // Limpeza: o exercício não deixa arquivo para trás.
    foreach (glob($pasta . DIRECTORY_SEPARATOR . '*') ?: [] as $arquivo) {
        unlink($arquivo);
    }

    rmdir($pasta);

    echo "\n(arquivos temporários removidos)\n";
}
