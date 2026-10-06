# Exercícios — PHP e C#

Seis exercícios, **os mesmos problemas nas duas linguagens**. A graça está aí: resolver a mesma coisa em PHP e em C# mostra o que é lógica de programação e o que é particularidade de linguagem.

Cada arquivo tem o enunciado no topo e a solução comentada logo abaixo — os comentários explicam as decisões, não o que a linha faz.

## Exercícios

| Nº | Tema | PHP | C# |
| --- | --- | --- | --- |
| 01 | Variáveis, operadores e laços | [01_variaveis_e_lacos.php](php/01_variaveis_e_lacos.php) | [01_VariaveisELacos](csharp/01_VariaveisELacos/Program.cs) |
| 02 | Condicionais | [02_condicionais.php](php/02_condicionais.php) | [02_Condicionais](csharp/02_Condicionais/Program.cs) |
| 03 | Strings | [03_strings.php](php/03_strings.php) | [03_Strings](csharp/03_Strings/Program.cs) |
| 04 | Arrays, listas e dicionários | [04_arrays.php](php/04_arrays.php) | [04_ArraysEListas](csharp/04_ArraysEListas/Program.cs) |
| 05 | Funções e recursão | [05_funcoes_e_recursao.php](php/05_funcoes_e_recursao.php) | [05_MetodosERecursao](csharp/05_MetodosERecursao/Program.cs) |
| 06 | Classes e objetos | [06_classes.php](php/06_classes.php) | [06_Classes](csharp/06_Classes/Program.cs) |

Cada arquivo tem, no fim, as chamadas que imprimem os resultados — os mesmos nas duas linguagens, para dar para comparar a saída lado a lado.

## Como rodar

**PHP 8.1+** — um arquivo por vez:

```bash
php php/01_variaveis_e_lacos.php
```

**.NET 8+** — a partir da pasta `csharp`:

```bash
dotnet run --project 01_VariaveisELacos
```

## O que comparar entre as duas

Resolvendo o mesmo problema dos dois lados, algumas diferenças aparecem sozinhas:

- **Tipagem.** O PHP aceita `declare(strict_types=1)` e tipos nas assinaturas, mas confere em tempo de execução. O C# confere na compilação: o código nem vira programa se os tipos não baterem.
- **Array.** No PHP um array faz tudo — lista, mapa, pilha. Em C# são três tipos diferentes: `array` (tamanho fixo), `List<T>` (cresce) e `Dictionary<K,V>`. O exercício 04 é onde isso fica evidente.
- **String imutável.** Em C#, todo método de string devolve uma string nova; o original nunca muda. No PHP a diferença existe, mas é menos visível.
- **Dinheiro.** O exercício 06 usa `decimal` em C# de propósito. `0.1 + 0.2` em ponto flutuante não dá exatamente `0.3`, e num extrato bancário isso vira centavo sumido.
- **Exceções.** Os dois têm, mas os nomes mudam: `InvalidArgumentException` e `DomainException` no PHP, `ArgumentException` e `InvalidOperationException` no C#.

## O que apareceu ao resolver

Algumas diferenças só ficam claras depois de escrever os dois lados:

**O `array_values` do exercício 04.** No PHP, `array_filter` preserva as chaves originais: filtrar `[1,2,3,4,5,6]` deixa as posições 1, 3 e 5, com buracos. Serializado para JSON, isso vira objeto `{"1":2,...}` em vez de lista `[2,4,6]`. O `Where(...).ToList()` do C# não tem esse problema — a lista já sai reindexada.

**O dicionário que ignora maiúsculas.** Em C#, `new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)` faz o próprio dicionário tratar `"obra"` e `"Obra"` como a mesma chave. No PHP não existe equivalente: a chave tem que ser normalizada antes, a cada acesso.

**O `[]` que o PHP cria sozinho.** `$grupos[$inicial][] = $nome` cria o array interno na primeira vez. Em C#, a mesma linha lança `KeyNotFoundException` — é preciso criar a lista antes, e o `TryGetValue` existe para isso.

**O arredondamento do .NET.** `Math.Round` usa, por padrão, arredondamento "para o par mais próximo": `Math.Round(2.5)` dá **2**, não 3. É correto para estatística e errado para preço. Os exercícios usam `MidpointRounding.AwayFromZero`, que é o comportamento que o `round()` do PHP já tem.

**O extrato que dá para adulterar.** No exercício 06, devolver a `List<string>` direto permitiria que quem recebe desse `Add` e inventasse uma movimentação. A lista é referência. Por isso o retorno é `IReadOnlyList<string>` via `AsReadOnly()`. No PHP o problema não existe: array é copiado na atribuição.

## Sugestão de ordem

Faça o exercício em PHP, depois o mesmo em C#, e só então compare. Fazer os dois em sequência é o que torna a diferença visível — fazer todos de uma linguagem e depois todos da outra perde o efeito.
