# Exercícios — PHP e C#

Seis exercícios, **os mesmos problemas nas duas linguagens**. A graça está aí: resolver a mesma coisa em PHP e em C# mostra o que é lógica de programação e o que é particularidade de linguagem.

Os arquivos têm só o enunciado e a assinatura das funções. As implementações estão por fazer.

## Exercícios

| Nº | Tema | PHP | C# |
| --- | --- | --- | --- |
| 01 | Variáveis, operadores e laços | [01_variaveis_e_lacos.php](php/01_variaveis_e_lacos.php) | [01_VariaveisELacos](csharp/01_VariaveisELacos/Program.cs) |
| 02 | Condicionais | [02_condicionais.php](php/02_condicionais.php) | [02_Condicionais](csharp/02_Condicionais/Program.cs) |
| 03 | Strings | [03_strings.php](php/03_strings.php) | [03_Strings](csharp/03_Strings/Program.cs) |
| 04 | Arrays, listas e dicionários | [04_arrays.php](php/04_arrays.php) | [04_ArraysEListas](csharp/04_ArraysEListas/Program.cs) |
| 05 | Funções e recursão | [05_funcoes_e_recursao.php](php/05_funcoes_e_recursao.php) | [05_MetodosERecursao](csharp/05_MetodosERecursao/Program.cs) |
| 06 | Classes e objetos | [06_classes.php](php/06_classes.php) | [06_Classes](csharp/06_Classes/Program.cs) |

Cada arquivo já tem, no fim, as chamadas que imprimem os resultados. Enquanto as funções não estiverem implementadas, a saída vem vazia ou zerada — é assim que se sabe que ainda falta trabalho.

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

## Sugestão de ordem

Faça o exercício em PHP, depois o mesmo em C#, e só então compare. Fazer os dois em sequência é o que torna a diferença visível — fazer todos de uma linguagem e depois todos da outra perde o efeito.
