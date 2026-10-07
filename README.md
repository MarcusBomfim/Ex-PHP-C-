# Exercícios — PHP e C#

Quinze exercícios, **os mesmos problemas nas duas linguagens**. A graça está aí: resolver a mesma coisa em PHP e em C# mostra o que é lógica de programação e o que é particularidade de linguagem.

Todos têm o enunciado no topo do arquivo. Os de **01 a 11** vêm com a solução comentada logo abaixo — os comentários explicam as decisões, não o que a linha faz. Os de **12 a 15** estão só com o enunciado e as assinaturas, para resolver.

## Exercícios

### Resolvidos

| Nº | Tema | PHP | C# |
| --- | --- | --- | --- |
| 01 | Variáveis, operadores e laços | [01_variaveis_e_lacos.php](php/01_variaveis_e_lacos.php) | [01_VariaveisELacos](csharp/01_VariaveisELacos/Program.cs) |
| 02 | Condicionais | [02_condicionais.php](php/02_condicionais.php) | [02_Condicionais](csharp/02_Condicionais/Program.cs) |
| 03 | Strings | [03_strings.php](php/03_strings.php) | [03_Strings](csharp/03_Strings/Program.cs) |
| 04 | Arrays, listas e dicionários | [04_arrays.php](php/04_arrays.php) | [04_ArraysEListas](csharp/04_ArraysEListas/Program.cs) |
| 05 | Funções e recursão | [05_funcoes_e_recursao.php](php/05_funcoes_e_recursao.php) | [05_MetodosERecursao](csharp/05_MetodosERecursao/Program.cs) |
| 06 | Classes e objetos | [06_classes.php](php/06_classes.php) | [06_Classes](csharp/06_Classes/Program.cs) |
| 07 | Datas e horas | [07_datas.php](php/07_datas.php) | [07_Datas](csharp/07_Datas/Program.cs) |
| 08 | Herança, abstratas e interfaces | [08_heranca.php](php/08_heranca.php) | [08_Heranca](csharp/08_Heranca/Program.cs) |
| 09 | Exceções e tratamento de erros | [09_excecoes.php](php/09_excecoes.php) | [09_Excecoes](csharp/09_Excecoes/Program.cs) |
| 10 | Ordenação e busca | [10_ordenacao.php](php/10_ordenacao.php) | [10_Ordenacao](csharp/10_Ordenacao/Program.cs) |
| 11 | Arquivos e JSON | [11_arquivos_e_json.php](php/11_arquivos_e_json.php) | [11_ArquivosEJson](csharp/11_ArquivosEJson/Program.cs) |

### A resolver

| Nº | Tema | PHP | C# |
| --- | --- | --- | --- |
| 12 | Enums | [12_enums.php](php/12_enums.php) | [12_Enums](csharp/12_Enums/Program.cs) |
| 13 | Conjuntos | [13_conjuntos.php](php/13_conjuntos.php) | [13_Conjuntos](csharp/13_Conjuntos/Program.cs) |
| 14 | Expressões regulares | [14_regex.php](php/14_regex.php) | [14_Regex](csharp/14_Regex/Program.cs) |
| 15 | Funções de ordem superior | [15_funcoes_de_ordem_superior.php](php/15_funcoes_de_ordem_superior.php) | [15_FuncoesDeOrdemSuperior](csharp/15_FuncoesDeOrdemSuperior/Program.cs) |

Nesses quatro, as funções estão com `// TODO` e devolvem valor vazio. Rodar o arquivo mostra a saída zerada — é assim que se sabe o que ainda falta. O 15 em C# tem uma exceção: o experimento final sobre captura de variável já roda, porque não depende de nenhuma implementação.

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

**O `DateOnly` que o PHP não tem.** No exercício 07, o C# tem um tipo que guarda só a data, sem hora. No PHP é preciso zerar a hora na mão com `setTime(0, 0)` em toda comparação — esquecer disso é a origem clássica do erro de "um dia a mais".

**O `printf` que conta bytes.** Ainda no 10, a tabela do PHP saía torta nas linhas com acento: `printf("%-22s")` e `str_pad` contam **bytes**, e "Nível" tem 5 letras em 6 bytes. A correção usa `mb_strlen`. Em C# o alinhamento de interpolação (`,-22`) conta caracteres e o problema não aparece.

**O `double` dividido por zero não lança em C#.** No exercício 09, `10.0 / 0.0` devolve `∞` em vez de erro — a norma IEEE 754 manda isso, e o programa segue com um número que contamina toda conta depois dele. Com `int` o comportamento é outro: aí sim lança. No PHP, `/` lança nos dois casos.

**`TryParse` em vez de exceção.** Também no 09: em C#, montar o stack trace de uma exceção custa caro, e por isso a biblioteca padrão oferece `int.TryParse`, que devolve `bool`. Num laço sobre mil linhas de CSV a diferença é de ordem de grandeza. É a resposta da linguagem para "não use exceção em fluxo esperado".

**`usort` ordena no lugar; `OrderBy` não.** No exercício 10, a versão PHP precisa copiar o array antes de ordenar para não alterar o do chamador. O LINQ devolve uma sequência nova e o cuidado some — mas `Array.Sort` e `List.Sort` voltam a ordenar no lugar, então a atenção continua necessária em C#, só que em outros métodos.

**JSON tipado.** No exercício 11, `LerJson<Obra>` devolve um objeto `Obra` com campos conferidos pelo compilador. No PHP o retorno é array associativo, e `$dados['nomo']` — com erro de digitação — só falha quando a linha roda.

**O retorno que dá para ignorar.** Ainda no 11: `file_put_contents` do PHP devolve `false` em caso de falha, e ignorar esse retorno é o erro mais comum com arquivo — o disco enche e o programa segue achando que salvou. `File.WriteAllText` lança, e não há como deixar passar sem querer.

## Sugestão de ordem

Faça o exercício em PHP, depois o mesmo em C#, e só então compare. Fazer os dois em sequência é o que torna a diferença visível — fazer todos de uma linguagem e depois todos da outra perde o efeito.
