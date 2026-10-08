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
| 6) static ordenadasPorUrgencia(): array
|    Da mais urgente para a menos: [Critica, Alta, Media, Baixa]
|
| 7) function chamadosUrgentes(array $chamados): array   (fora do enum)
|    Recebe uma lista de ['titulo' => string, 'prioridade' => Prioridade]
|    e devolve só os títulos dos urgentes, da maior urgência para a menor.
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
        /*
         * Sem `default`, de propósito. Com os quatro casos listados, o
         * match é exaustivo. Se um quinto caso entrar no enum amanhã e
         * esquecerem deste método, o PHP lança UnhandledMatchError na hora
         * — em vez de deixar um rótulo vazio circular pelo sistema.
         *
         * É o oposto do `default => ''`, que esconde o esquecimento.
         */
        return match ($this) {
            self::Baixa => 'Baixa',
            self::Media => 'Média',
            self::Alta => 'Alta',
            self::Critica => 'Crítica',
        };
    }

    public function prazoEmHoras(): int
    {
        return match ($this) {
            self::Baixa => 72,
            self::Media => 24,
            self::Alta => 8,
            self::Critica => 2,
        };
    }

    public function ehUrgente(): bool
    {
        // Também sem default: acrescentar um caso novo obriga a decidir se
        // ele é urgente, em vez de herdar um "false" por omissão.
        return match ($this) {
            self::Baixa, self::Media => false,
            self::Alta, self::Critica => true,
        };
    }

    public function descricao(): string
    {
        // Montada a partir dos outros dois. Repetir "Crítica" e "2" aqui
        // criaria um segundo lugar para errar quando o prazo mudar.
        return sprintf('%s — atendimento em até %dh', $this->rotulo(), $this->prazoEmHoras());
    }

    public static function deTexto(string $texto): self
    {
        /*
         * tryFrom devolve null em vez de lançar, e é isso que permite
         * trocar o erro do PHP por uma mensagem que diz as opções.
         *
         * `from()` lançaria "is not a valid backing value for enum", que
         * não ajuda quem está preenchendo um formulário.
         */
        return self::tryFrom(mb_strtolower(trim($texto)))
            ?? throw new ValueError(sprintf(
                'Prioridade "%s" não existe. Use uma destas: %s.',
                trim($texto),
                implode(', ', array_column(self::cases(), 'value')),
            ));
    }

    /** @return self[] */
    public static function ordenadasPorUrgencia(): array
    {
        return [self::Critica, self::Alta, self::Media, self::Baixa];
    }
}

/**
 * @param  array<int, array{titulo: string, prioridade: Prioridade}> $chamados
 * @return string[]
 */
function chamadosUrgentes(array $chamados): array
{
    $urgentes = array_values(array_filter(
        $chamados,
        static fn (array $chamado): bool => $chamado['prioridade']->ehUrgente(),
    ));

    /*
     * O peso de cada prioridade vem da posição em ordenadasPorUrgencia().
     * Escrever a ordem de novo aqui criaria um segundo lugar para manter —
     * e os dois sairiam do ar juntos no dia em que uma prioridade nova
     * aparecesse.
     */
    $ordem = array_flip(array_map(
        static fn (Prioridade $p): string => $p->value,
        Prioridade::ordenadasPorUrgencia(),
    ));

    usort(
        $urgentes,
        static fn (array $a, array $b): int => $ordem[$a['prioridade']->value] <=> $ordem[$b['prioridade']->value],
    );

    return array_column($urgentes, 'titulo');
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
