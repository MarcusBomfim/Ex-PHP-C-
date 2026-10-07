<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 12 — Enums
|--------------------------------------------------------------------------
|
| Conteúdo: enum com valor (backed enum), métodos dentro do enum, match
| exaustivo, from() e tryFrom().
|
| O cenário: prioridade de um chamado de suporte.
|
| O enum já está declarado abaixo com os quatro casos. Implemente os
| métodos.
|
| 1) rotulo(): string
|    Baixa -> "Baixa"   Media -> "Média"   Alta -> "Alta"   Critica -> "Crítica"
|
| 2) prazoEmHoras(): int
|    Baixa 72, Media 24, Alta 8, Critica 2
|
| 3) ehUrgente(): bool
|    Verdadeiro para Alta e Critica.
|
| 4) descricao(): string
|    "Crítica — atendimento em até 2h"
|    Use os dois métodos acima em vez de repetir os valores.
|
| 5) static deTexto(string $texto): self
|    Aceita o valor em qualquer caixa ("ALTA", "alta", " Alta ").
|    Texto desconhecido lança ValueError com uma mensagem que liste as
|    opções válidas.
|
|    deTexto("ALTA")   -> Prioridade::Alta
|    deTexto("urgente") -> ValueError
|
| 6) static ordenadasPorUrgencia(): array
|    Da mais urgente para a menos: [Critica, Alta, Media, Baixa]
|
| 7) function chamadosUrgentes(array $chamados): array   (fora do enum)
|    Recebe uma lista de ['titulo' => string, 'prioridade' => Prioridade]
|    e devolve só os títulos dos urgentes, da maior urgência para a menor.
|
| Dica sobre o `match`: dentro de um enum, `match($this)` cobrindo todos os
| casos dispensa o `default`. E é melhor sem ele — se um caso novo for
| acrescentado ao enum amanhã, o PHP lança UnhandledMatchError em vez de
| devolver silenciosamente o valor padrão errado.
|
| Rode com:  php php/12_enums.php
|
*/

enum Prioridade: string
{
    case Baixa = 'baixa';
    case Media = 'media';
    case Alta = 'alta';
    case Critica = 'critica';

    public function rotulo(): string
    {
        // TODO: implemente
        return '';
    }

    public function prazoEmHoras(): int
    {
        // TODO: implemente
        return 0;
    }

    public function ehUrgente(): bool
    {
        // TODO: implemente
        return false;
    }

    public function descricao(): string
    {
        // TODO: implemente usando rotulo() e prazoEmHoras()
        return '';
    }

    public static function deTexto(string $texto): self
    {
        // TODO: implemente
        return self::Baixa;
    }

    /** @return self[] */
    public static function ordenadasPorUrgencia(): array
    {
        // TODO: implemente
        return [];
    }
}

/**
 * @param  array<int, array{titulo: string, prioridade: Prioridade}> $chamados
 * @return string[]
 */
function chamadosUrgentes(array $chamados): array
{
    // TODO: implemente
    return [];
}

// ---------------------------------------------------------------- saída

echo "--- Todos os casos ---\n";

foreach (Prioridade::cases() as $prioridade) {
    printf(
        "  %-8s valor=%-8s %s\n",
        $prioridade->name,
        $prioridade->value,
        $prioridade->descricao(),
    );
}

echo "\n--- Urgentes ---\n";

foreach (Prioridade::cases() as $prioridade) {
    echo '  ' . $prioridade->name . ': ' . ($prioridade->ehUrgente() ? 'sim' : 'não') . "\n";
}

echo "\n--- Ordenadas por urgência ---\n";
echo '  ' . implode(
    ' > ',
    array_map(static fn (Prioridade $p): string => $p->rotulo(), Prioridade::ordenadasPorUrgencia()),
) . "\n";

echo "\n--- Conversão de texto ---\n";

foreach (['ALTA', ' critica ', 'media'] as $texto) {
    echo "  \"{$texto}\" -> " . Prioridade::deTexto($texto)->name . "\n";
}

try {
    Prioridade::deTexto('urgentíssimo');
    echo "  ERRO: aceitou texto inválido!\n";
} catch (ValueError $erro) {
    echo '  Recusado, como esperado: ' . $erro->getMessage() . "\n";
}

echo "\n--- Fila de chamados ---\n";

$fila = [
    ['titulo' => 'Impressora sem toner', 'prioridade' => Prioridade::Baixa],
    ['titulo' => 'Sistema fora do ar', 'prioridade' => Prioridade::Critica],
    ['titulo' => 'Lentidão no relatório', 'prioridade' => Prioridade::Media],
    ['titulo' => 'Erro ao fechar medição', 'prioridade' => Prioridade::Alta],
];

foreach (chamadosUrgentes($fila) as $titulo) {
    echo '  ' . $titulo . "\n";
}

echo "\n--- tryFrom x from ---\n";
// tryFrom devolve null; from lança. Os dois já vêm prontos no enum.
var_dump(Prioridade::tryFrom('alta'));
var_dump(Prioridade::tryFrom('inexistente'));
