<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 10 — Ordenação e busca
|--------------------------------------------------------------------------
|
| Conteúdo: usort, o operador <=> (nave espacial), ordenação por vários
| critérios, estabilidade e busca binária.
|
| Para os exercícios, uma lista de produtos: nome, categoria e preço.
|
| 1) function ordenarPorPreco(array $produtos, bool $crescente = true): array
|    Não altera o array recebido — devolve um novo ordenado.
|
| 2) function ordenarPorCategoriaEPreco(array $produtos): array
|    Categoria em ordem alfabética; dentro dela, preço do maior para o
|    menor. É o caso que o <=> resolve em uma linha.
|
| 3) function buscaBinaria(array $ordenados, int $alvo): int
|    Devolve o índice do alvo, ou -1 se não achar. O array JÁ vem ordenado.
|
|    buscaBinaria([1, 3, 5, 7, 9, 11], 7)  -> 3
|    buscaBinaria([1, 3, 5, 7, 9, 11], 4)  -> -1
|
| 4) function maisBaratos(array $produtos, int $quantos): array
|
| 5) function agruparPorCategoria(array $produtos): array
|    Categoria => lista de nomes, com as categorias em ordem alfabética.
|
| Sobre busca binária: ela corta a lista pela metade a cada passo. Numa
| lista de 1 milhão de itens, são no máximo 20 comparações — contra 1
| milhão da busca linear. O preço é exigir a lista ordenada antes.
|
| Rode com:  php php/10_ordenacao.php
|
*/

final class Produto
{
    public function __construct(
        public readonly string $nome,
        public readonly string $categoria,
        public readonly float $preco,
    ) {
    }
}

/**
 * @param  Produto[] $produtos
 * @return Produto[]
 */
function ordenarPorPreco(array $produtos, bool $crescente = true): array
{
    /*
     * usort ordena NO LUGAR e devolve bool. Por isso a cópia: sem ela, a
     * função alteraria o array de quem chamou — e "ordenar" viraria um
     * efeito colateral escondido.
     *
     * Em PHP, array é copiado na atribuição, então esta linha basta.
     */
    $copia = $produtos;

    usort(
        $copia,
        static fn (Produto $a, Produto $b): int => $crescente
            ? $a->preco <=> $b->preco
            : $b->preco <=> $a->preco,
    );

    return $copia;
}

/**
 * @param  Produto[] $produtos
 * @return Produto[]
 */
function ordenarPorCategoriaEPreco(array $produtos): array
{
    $copia = $produtos;

    usort($copia, static function (Produto $a, Produto $b): int {
        /*
         * O <=> devolve -1, 0 ou 1. O truque dos vários critérios é: se o
         * primeiro empatar (0), o `?:` cai para o segundo.
         *
         * Repare na inversão no preço ($b antes de $a): é o que faz a
         * ordem ser decrescente ali, sem precisar de outro parâmetro.
         */
        return ($a->categoria <=> $b->categoria)
            ?: ($b->preco <=> $a->preco);
    });

    return $copia;
}

/** @param int[] $ordenados */
function buscaBinaria(array $ordenados, int $alvo): int
{
    $inicio = 0;
    $fim = count($ordenados) - 1;

    while ($inicio <= $fim) {
        /*
         * intdiv($inicio + $fim, 2) é o meio. Em linguagens com inteiro de
         * tamanho fixo, essa soma pode estourar em arrays gigantes — o
         * jeito à prova disso é $inicio + intdiv($fim - $inicio, 2).
         * Em PHP o int é de 64 bits e não há risco prático, mas o hábito
         * vale: foi um bug real na biblioteca padrão do Java por 9 anos.
         */
        $meio = $inicio + intdiv($fim - $inicio, 2);

        if ($ordenados[$meio] === $alvo) {
            return $meio;
        }

        if ($ordenados[$meio] < $alvo) {
            // O alvo só pode estar na metade de cima.
            $inicio = $meio + 1;
        } else {
            $fim = $meio - 1;
        }
    }

    return -1;
}

/**
 * @param  Produto[] $produtos
 * @return Produto[]
 */
function maisBaratos(array $produtos, int $quantos): array
{
    return array_slice(ordenarPorPreco($produtos), 0, max(0, $quantos));
}

/**
 * @param  Produto[] $produtos
 * @return array<string, string[]>
 */
function agruparPorCategoria(array $produtos): array
{
    $grupos = [];

    foreach ($produtos as $produto) {
        $grupos[$produto->categoria][] = $produto->nome;
    }

    // ksort ordena pelas CHAVES, preservando a ligação chave => valor.
    // sort() aqui jogaria fora os nomes das categorias.
    ksort($grupos);

    return $grupos;
}

// ---------------------------------------------------------------- saída

$catalogo = [
    new Produto('Cimento CP-II 50 kg', 'material', 38.90),
    new Produto('Trena 5 m', 'ferramenta', 24.50),
    new Produto('Areia média m³', 'material', 110.00),
    new Produto('Capacete', 'epi', 32.00),
    new Produto('Nível a laser', 'ferramenta', 289.90),
    new Produto('Luva de raspa', 'epi', 18.75),
];

/**
 * Preenche até a largura pedida contando LETRAS, não bytes.
 *
 * `printf("%-22s")` e `str_pad` contam bytes. "Nível" tem 5 letras e 6
 * bytes em UTF-8, então a coluna sai um caractere mais curta e a tabela
 * desalinha — exatamente o problema do exercício 03, agora na formatação.
 */
$coluna = static function (string $texto, int $largura): string {
    return $texto . str_repeat(' ', max(0, $largura - mb_strlen($texto)));
};

$mostrar = static function (array $produtos) use ($coluna): void {
    foreach ($produtos as $produto) {
        echo '  '
            . $coluna($produto->nome, 22)
            . $coluna($produto->categoria, 12)
            . 'R$ ' . $coluna(number_format($produto->preco, 2, ',', '.'), 9)
            . "\n";
    }
};

echo "--- Por preço (crescente) ---\n";
$mostrar(ordenarPorPreco($catalogo));

echo "\n--- Por preço (decrescente) ---\n";
$mostrar(ordenarPorPreco($catalogo, false));

echo "\n--- O original não foi alterado ---\n";
echo '  primeiro item ainda é: ' . $catalogo[0]->nome . "\n";

echo "\n--- Por categoria, e dentro dela do mais caro ---\n";
$mostrar(ordenarPorCategoriaEPreco($catalogo));

echo "\n--- Os 3 mais baratos ---\n";
$mostrar(maisBaratos($catalogo, 3));

echo "\n--- Agrupado por categoria ---\n";

foreach (agruparPorCategoria($catalogo) as $categoria => $nomes) {
    echo '  ' . $categoria . ': ' . implode(', ', $nomes) . "\n";
}

echo "\n--- Busca binária ---\n";
$numeros = [1, 3, 5, 7, 9, 11, 13, 15];

foreach ([7, 1, 15, 4] as $alvo) {
    $indice = buscaBinaria($numeros, $alvo);

    echo "  procurando {$alvo}: "
        . ($indice === -1 ? 'não encontrado' : "índice {$indice}")
        . "\n";
}
