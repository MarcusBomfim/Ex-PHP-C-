/*
|--------------------------------------------------------------------------
| EXERCÍCIO 12 — Enums
|--------------------------------------------------------------------------
|
| Mesmo problema do php/12_enums.php — e aqui está uma das maiores
| diferenças entre as duas linguagens.
|
| No PHP, o enum é um OBJETO: ele tem métodos dentro, e `Prioridade::Alta
| ->prazoEmHoras()` é uma chamada normal.
|
| Em C#, enum é um INTEIRO com nome. Não cabe método dentro. O jeito é uma
| classe estática de métodos de extensão — que o compilador deixa chamar
| como se fossem do enum: `Prioridade.Alta.PrazoEmHoras()`.
|
| Consequência prática: no PHP, esquecer de tratar um caso novo dá erro
| (UnhandledMatchError). Em C#, um `switch` sem o caso novo só devolve o
| `_ =>`. É por isso que os switches abaixo pedem para NÃO usar `_` quando
| der para listar todos os casos.
|
| Outra: `(Prioridade)99` é um cast válido em C# e produz um enum que não
| existe. O Enum.IsDefined serve para isso. No PHP, `from()` lança.
|
| O enum já está declarado. Implemente os métodos de extensão.
|
| 1) static string Rotulo(this Prioridade p)
|    Baixa -> "Baixa"   Media -> "Média"   Alta -> "Alta"   Critica -> "Crítica"
|
| 2) static int PrazoEmHoras(this Prioridade p)
|    Baixa 72, Media 24, Alta 8, Critica 2
|
| 3) static bool EhUrgente(this Prioridade p)
|    Verdadeiro para Alta e Critica.
|
| 4) static string Descricao(this Prioridade p)
|    "Crítica — atendimento em até 2h"
|    Use os dois métodos acima em vez de repetir os valores.
|
| 5) static Prioridade DeTexto(string texto)
|    Aceita qualquer caixa ("ALTA", "alta", " Alta "). Texto desconhecido
|    lança ArgumentException listando as opções válidas.
|    Dica: veja Enum.TryParse com ignoreCase — mas lembre que ele aceita
|    também o número ("3"), o que talvez você não queira.
|
| 6) static IReadOnlyList<Prioridade> OrdenadasPorUrgencia()
|    [Critica, Alta, Media, Baixa]
|
| 7) static List<string> ChamadosUrgentes(IEnumerable<Chamado> chamados)
|    Só os títulos dos urgentes, da maior urgência para a menor.
|
| Rode com:  dotnet run --project 12_Enums
|
*/

Console.WriteLine("--- Todos os casos ---");

foreach (Prioridade prioridade in Enum.GetValues<Prioridade>())
{
    Console.WriteLine($"  {prioridade,-8} valor={(int)prioridade,-3} {prioridade.Descricao()}");
}

Console.WriteLine("\n--- Urgentes ---");

foreach (Prioridade prioridade in Enum.GetValues<Prioridade>())
{
    Console.WriteLine($"  {prioridade}: {(prioridade.EhUrgente() ? "sim" : "não")}");
}

Console.WriteLine("\n--- Ordenadas por urgência ---");
Console.WriteLine("  " + string.Join(" > ", PrioridadeExtensoes.OrdenadasPorUrgencia().Select(p => p.Rotulo())));

Console.WriteLine("\n--- Conversão de texto ---");

foreach (string texto in new[] { "ALTA", " critica ", "media" })
{
    Console.WriteLine($"  \"{texto}\" -> {PrioridadeExtensoes.DeTexto(texto)}");
}

try
{
    PrioridadeExtensoes.DeTexto("urgentíssimo");
    Console.WriteLine("  ERRO: aceitou texto inválido!");
}
catch (ArgumentException erro)
{
    Console.WriteLine($"  Recusado, como esperado: {erro.Message}");
}

Console.WriteLine("\n--- Fila de chamados ---");

Chamado[] fila =
[
    new("Impressora sem toner", Prioridade.Baixa),
    new("Sistema fora do ar", Prioridade.Critica),
    new("Lentidão no relatório", Prioridade.Media),
    new("Erro ao fechar medição", Prioridade.Alta),
];

foreach (string titulo in PrioridadeExtensoes.ChamadosUrgentes(fila))
{
    Console.WriteLine($"  {titulo}");
}

Console.WriteLine("\n--- O enum que não existe ---");
var inventada = (Prioridade)99;
Console.WriteLine($"  (Prioridade)99 = {inventada}");
Console.WriteLine($"  Enum.IsDefined diz que existe? {Enum.IsDefined(inventada)}");
Console.WriteLine("  No PHP isso não acontece: Prioridade::from('99') lança.");

public enum Prioridade
{
    Baixa = 1,
    Media = 2,
    Alta = 3,
    Critica = 4,
}

public sealed record Chamado(string Titulo, Prioridade Prioridade);

public static class PrioridadeExtensoes
{
    public static string Rotulo(this Prioridade prioridade)
    {
        /*
         * Sem o `_ =>` final, de propósito — e aqui está a diferença mais
         * importante para o PHP.
         *
         * Com os quatro casos listados e sem descarte, o compilador avisa
         * (CS8509) que o switch pode não cobrir todo valor possível. Esse
         * aviso é o que lembra de voltar aqui quando um caso novo entrar
         * no enum.
         *
         * Com `_ => ""`, o aviso some e o caso novo passa a devolver
         * string vazia em silêncio. No PHP, o match sem default LANÇA em
         * tempo de execução; em C#, o alerta vem antes, na compilação —
         * mas é só um aviso, e por isso some com facilidade.
         */
        return prioridade switch
        {
            Prioridade.Baixa => "Baixa",
            Prioridade.Media => "Média",
            Prioridade.Alta => "Alta",
            Prioridade.Critica => "Crítica",
            _ => throw new ArgumentOutOfRangeException(nameof(prioridade), prioridade, "Prioridade desconhecida."),
        };
    }

    public static int PrazoEmHoras(this Prioridade prioridade)
    {
        return prioridade switch
        {
            Prioridade.Baixa => 72,
            Prioridade.Media => 24,
            Prioridade.Alta => 8,
            Prioridade.Critica => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(prioridade), prioridade, "Prioridade desconhecida."),
        };
    }

    public static bool EhUrgente(this Prioridade prioridade)
    {
        // `is ... or ...` em vez de dois ==. Lê melhor e não deixa escrever
        // `prioridade == Prioridade.Alta || Prioridade.Critica`, que compila
        // em algumas linguagens e faz outra coisa.
        return prioridade is Prioridade.Alta or Prioridade.Critica;
    }

    public static string Descricao(this Prioridade prioridade)
    {
        // Montada a partir dos outros dois. Repetir "Crítica" e "2" aqui
        // criaria um segundo lugar para errar quando o prazo mudar.
        return $"{prioridade.Rotulo()} — atendimento em até {prioridade.PrazoEmHoras()}h";
    }

    public static Prioridade DeTexto(string texto)
    {
        string limpo = texto.Trim();

        /*
         * Enum.TryParse sozinho não basta, por dois motivos:
         *
         *  - ele aceita o NÚMERO: TryParse("3") devolve Prioridade.Alta,
         *    o que quase nunca é o que se quer num campo de formulário;
         *  - ele aceita valor fora do enum: TryParse("99") devolve
         *    (Prioridade)99 e diz que deu certo.
         *
         * O primeiro filtro resolve o número, o Enum.IsDefined resolve o
         * valor inventado. No PHP nada disso é preciso: tryFrom só aceita
         * os valores declarados.
         */
        if (!limpo.Any(char.IsDigit)
            && Enum.TryParse(limpo, ignoreCase: true, out Prioridade prioridade)
            && Enum.IsDefined(prioridade))
        {
            return prioridade;
        }

        throw new ArgumentException(
            $"Prioridade \"{limpo}\" não existe. Use uma destas: "
            + string.Join(", ", Enum.GetNames<Prioridade>()) + ".",
            nameof(texto)
        );
    }

    public static IReadOnlyList<Prioridade> OrdenadasPorUrgencia()
    {
        /*
         * Montada a partir do próprio enum, e não escrita à mão. Os valores
         * numéricos (1 a 4) crescem com a urgência, então ordenar
         * decrescente dá a ordem certa — e uma prioridade nova entra no
         * lugar certo sozinha, desde que receba o número coerente.
         */
        return Enum.GetValues<Prioridade>()
            .OrderByDescending(p => (int)p)
            .ToList();
    }

    public static List<string> ChamadosUrgentes(IEnumerable<Chamado> chamados)
    {
        return chamados
            .Where(c => c.Prioridade.EhUrgente())
            .OrderByDescending(c => (int)c.Prioridade)
            .Select(c => c.Titulo)
            .ToList();
    }
}
