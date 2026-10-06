<?php

declare(strict_types=1);

/*
|--------------------------------------------------------------------------
| EXERCÍCIO 09 — Exceções e tratamento de erros
|--------------------------------------------------------------------------
|
| Conteúdo: try/catch/finally, exceções próprias, hierarquia de exceções,
| encadeamento com $previous e quando NÃO usar exceção.
|
| 1) class ErroDeValidacao extends InvalidArgumentException
|    Guarda uma lista de problemas, não um só. Formulário com três campos
|    errados deve mostrar os três de uma vez — não um por tentativa.
|    - construtor recebe string[] $problemas
|    - problemas(): array
|
| 2) function validarCadastro(array $dados): void
|    Confere nome (obrigatório, mín. 3), e-mail (obrigatório, formato) e
|    idade (obrigatória, inteiro entre 0 e 130). JUNTA todos os problemas
|    e lança um ErroDeValidacao só no fim.
|
| 3) function dividir(float $a, float $b): float
|    Lança DivisionByZeroError quando $b é zero.
|    (Em PHP 8, `/` já faz isso sozinho — o exercício é entender a diferença
|    entre Error e Exception.)
|
| 4) function converterParaInteiro(string $texto): int
|    Lança ErroDeConversao (própria) com a exceção original em $previous.
|    Mostra por que o encadeamento importa: a causa raiz não se perde.
|
| 5) function lerComFallback(callable $tentativa, int $padrao): int
|    Executa a função; se lançar qualquer coisa, devolve o padrão. O
|    `finally` roda nos dois casos.
|
| Quando NÃO usar exceção: "usuário digitou e-mail errado" é fluxo normal
| de formulário e dá para prever. Exceção é para o que foge do esperado.
| Este exercício usa exceção na validação de propósito, para mostrar a
| mecânica — num sistema real, retornar a lista de erros costuma ser melhor.
|
| Rode com:  php php/09_excecoes.php
|
*/

/**
 * Estende InvalidArgumentException, e não Exception direto, por um motivo
 * prático: quem só quer saber "deu erro de entrada" captura a classe do
 * PHP e pega esta junto, sem conhecer o nome dela.
 */
final class ErroDeValidacao extends InvalidArgumentException
{
    /** @param string[] $problemas */
    public function __construct(private readonly array $problemas)
    {
        parent::__construct(sprintf(
            '%d problema(s) no cadastro: %s',
            count($problemas),
            implode('; ', $problemas),
        ));
    }

    /** @return string[] */
    public function problemas(): array
    {
        return $this->problemas;
    }
}

final class ErroDeConversao extends RuntimeException
{
}

/** @param array<string, mixed> $dados */
function validarCadastro(array $dados): void
{
    $problemas = [];

    $nome = trim((string) ($dados['nome'] ?? ''));

    if ($nome === '') {
        $problemas[] = 'o nome é obrigatório';
    } elseif (mb_strlen($nome) < 3) {
        $problemas[] = 'o nome precisa ter ao menos 3 caracteres';
    }

    $email = trim((string) ($dados['email'] ?? ''));

    if ($email === '') {
        $problemas[] = 'o e-mail é obrigatório';
    } elseif (filter_var($email, FILTER_VALIDATE_EMAIL) === false) {
        $problemas[] = "o e-mail \"{$email}\" não tem formato válido";
    }

    $idade = $dados['idade'] ?? null;

    if ($idade === null || $idade === '') {
        $problemas[] = 'a idade é obrigatória';
    } elseif (!is_int($idade) && !ctype_digit((string) $idade)) {
        $problemas[] = 'a idade precisa ser um número inteiro';
    } elseif ((int) $idade > 130) {
        $problemas[] = 'a idade informada não é plausível';
    }

    /*
     * Lança UMA vez, no fim, com tudo junto. Lançar na primeira falha
     * obrigaria a pessoa a corrigir um campo, enviar, descobrir o
     * segundo, enviar de novo...
     */
    if ($problemas !== []) {
        throw new ErroDeValidacao($problemas);
    }
}

function dividir(float $a, float $b): float
{
    /*
     * DivisionByZeroError é um Error, não uma Exception. A diferença: Error
     * sinaliza defeito de programação (passaram zero onde não podiam),
     * Exception sinaliza situação prevista do negócio.
     *
     * Os dois descendem de Throwable, então `catch (Throwable)` pega ambos —
     * e é por isso que capturar Throwable às cegas esconde bug.
     */
    if ($b === 0.0) {
        throw new DivisionByZeroError('Divisão por zero.');
    }

    return $a / $b;
}

function converterParaInteiro(string $texto): int
{
    try {
        $limpo = trim($texto);

        if (!preg_match('/^-?\d+$/', $limpo)) {
            throw new ValueError("O texto \"{$texto}\" não é um número inteiro.");
        }

        return (int) $limpo;
    } catch (ValueError $causa) {
        /*
         * O terceiro argumento é o $previous: a exceção original vai junto.
         * Sem ele, a mensagem de alto nível substitui a causa raiz e o
         * rastro se perde — é o motivo de tantos "erro ao processar" que
         * não dizem nada.
         */
        throw new ErroDeConversao("Não foi possível converter \"{$texto}\".", 0, $causa);
    }
}

/** @param callable(): int $tentativa */
function lerComFallback(callable $tentativa, int $padrao): int
{
    try {
        return $tentativa();
    } catch (Throwable) {
        // Sem variável: o `catch` sem nome é válido desde o PHP 8 quando
        // não se vai usar o objeto.
        return $padrao;
    } finally {
        /*
         * O finally roda SEMPRE — inclusive depois do `return` do try e do
         * `return` do catch. É onde vai o que precisa acontecer de qualquer
         * jeito: fechar arquivo, soltar conexão, liberar trava.
         */
        echo "  (finally: sempre executa)\n";
    }
}

// ---------------------------------------------------------------- saída

echo "--- Validação com vários problemas ---\n";

try {
    validarCadastro(['nome' => 'Jo', 'email' => 'nao-e-email', 'idade' => 999]);
} catch (ErroDeValidacao $erro) {
    echo 'Mensagem: ' . $erro->getMessage() . "\n";
    echo "Lista:\n";

    foreach ($erro->problemas() as $problema) {
        echo '  - ' . $problema . "\n";
    }
}

echo "\n--- Cadastro válido ---\n";

try {
    validarCadastro(['nome' => 'Marcus', 'email' => 'marcus@obra.dev', 'idade' => 30]);
    echo "Passou sem erro.\n";
} catch (ErroDeValidacao $erro) {
    echo 'Não deveria falhar: ' . $erro->getMessage() . "\n";
}

echo "\n--- Error x Exception ---\n";

try {
    dividir(10.0, 0.0);
} catch (DivisionByZeroError $erro) {
    echo 'Capturado como Error: ' . $erro->getMessage() . "\n";
}

echo 'dividir(10, 4) = ' . dividir(10.0, 4.0) . "\n";

echo "\n--- Encadeamento de exceções ---\n";

try {
    converterParaInteiro('vinte');
} catch (ErroDeConversao $erro) {
    echo 'Topo:  ' . $erro->getMessage() . "\n";
    echo 'Causa: ' . ($erro->getPrevious()?->getMessage() ?? '(nenhuma)') . "\n";
}

echo 'converterParaInteiro("  42 ") = ' . converterParaInteiro('  42 ') . "\n";

echo "\n--- Fallback com finally ---\n";

echo 'sucesso: ' . lerComFallback(static fn (): int => 7, 0) . "\n";
echo 'falha:   ' . lerComFallback(static fn (): int => throw new RuntimeException('quebrou'), -1) . "\n";
