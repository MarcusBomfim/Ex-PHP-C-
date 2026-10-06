/*
|--------------------------------------------------------------------------
| EXERCÍCIO 07 — Datas e horas
|--------------------------------------------------------------------------
|
| Mesmo problema do php/07_datas.php.
|
| Conteúdo: DateOnly, DateTime, TimeSpan, DayOfWeek e por que "somar 30
| dias" quase nunca é o mesmo que "somar um mês".
|
| Diferença grande para o PHP: aqui existe DateOnly, um tipo que guarda SÓ
| a data, sem hora nenhuma. No PHP é preciso zerar a hora na mão
| (setTime(0,0)) toda vez, e esquecer disso é fonte clássica de erro de um
| dia a mais ou a menos.
|
| 1) static int DiasEntre(DateOnly inicio, DateOnly fim)
|    Dias inteiros entre as duas. Sempre positivo.
|
|    DiasEntre(2026-01-01, 2026-01-31) -> 30
|    DiasEntre(2026-01-31, 2026-01-01) -> 30
|
| 2) static bool EhFimDeSemana(DateOnly data)
|
| 3) static DateOnly AdicionarDiasUteis(DateOnly data, int dias)
|    Sexta 2026-01-02 + 1 dia útil -> segunda 2026-01-05
|
| 4) static int IdadeEm(DateOnly nascimento, DateOnly referencia)
|    Anos completos.
|
| 5) static string FormatarDuracao(int minutos)
|    0 -> "0min"   45 -> "45min"   60 -> "1h"   150 -> "2h 30min"
|
| Rode com:  dotnet run --project 07_Datas
|
*/

using System.Globalization;

var primeiroDeJaneiro = new DateOnly(2026, 1, 1);
var ultimoDeJaneiro = new DateOnly(2026, 1, 31);

Console.WriteLine("--- Dias entre ---");
Console.WriteLine($"01/01 a 31/01 = {DiasEntre(primeiroDeJaneiro, ultimoDeJaneiro)} dias");
Console.WriteLine($"31/01 a 01/01 = {DiasEntre(ultimoDeJaneiro, primeiroDeJaneiro)} dias (mesma coisa)");

Console.WriteLine("\n--- Fim de semana ---");

foreach (int dia in new[] { 2, 3, 4, 5 })
{
    var data = new DateOnly(2026, 1, dia);
    string tipo = EhFimDeSemana(data) ? "fim de semana" : "dia útil";

    Console.WriteLine($"{data:dd/MM/yyyy} ({data.DayOfWeek}): {tipo}");
}

Console.WriteLine("\n--- Dias úteis ---");
var sexta = new DateOnly(2026, 1, 2);

foreach (int dias in new[] { 1, 5, 10 })
{
    var destino = AdicionarDiasUteis(sexta, dias);
    Console.WriteLine($"sexta 02/01 + {dias} dia(s) útil(eis) = {destino:dd/MM/yyyy} ({destino.DayOfWeek})");
}

Console.WriteLine("\n--- Idade ---");
var nascimento = new DateOnly(2000, 12, 10);

foreach (var quando in new[] { new DateOnly(2026, 12, 9), new DateOnly(2026, 12, 10) })
{
    Console.WriteLine($"em {quando:yyyy-MM-dd}: {IdadeEm(nascimento, quando)} anos");
}

Console.WriteLine("\n--- Duração ---");

foreach (int minutos in new[] { 0, 45, 60, 150, 1440 })
{
    Console.WriteLine($"{minutos,5} min = {FormatarDuracao(minutos)}");
}

static int DiasEntre(DateOnly inicio, DateOnly fim)
{
    /*
     * DayNumber é a contagem de dias desde 01/01/0001 — subtrair os dois
     * dá a diferença direta, sem passar por TimeSpan.
     *
     * Aqui não é preciso zerar hora nenhuma: DateOnly simplesmente não
     * tem hora. É a vantagem sobre o setTime(0,0) do PHP, que é fácil de
     * esquecer e vira erro de um dia.
     */
    return Math.Abs(fim.DayNumber - inicio.DayNumber);
}

static bool EhFimDeSemana(DateOnly data)
{
    // DayOfWeek é enum, não número. `data.DayOfWeek == 6` nem compila —
    // o que elimina a dúvida de "domingo é 0 ou é 7?".
    return data.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}

static DateOnly AdicionarDiasUteis(DateOnly data, int dias)
{
    DateOnly resultado = data;

    /*
     * Avança um dia por vez e só conta quando cai em dia útil. Parece
     * ingênuo, mas é o jeito certo: a fórmula "dias/5*7" erra sempre que o
     * ponto de partida é fim de semana — e quebra de vez quando entram
     * feriados, que é o próximo passo natural deste exercício.
     */
    while (dias > 0)
    {
        resultado = resultado.AddDays(1);

        if (!EhFimDeSemana(resultado))
        {
            dias--;
        }
    }

    return resultado;
}

static int IdadeEm(DateOnly nascimento, DateOnly referencia)
{
    int idade = referencia.Year - nascimento.Year;

    /*
     * Diferente do PHP, aqui não há um diff()->y pronto para DateOnly: a
     * correção do "ainda não fez aniversário" é na mão.
     *
     * AddYears(idade) sobre a data de nascimento dá o aniversário deste
     * ano. Se ele ainda não chegou, desconta um. Fazer a comparação assim
     * — em vez de comparar mês e dia separados — resolve de graça o 29 de
     * fevereiro, porque AddYears já trata isso.
     */
    if (nascimento.AddYears(idade) > referencia)
    {
        idade--;
    }

    return idade;
}

static string FormatarDuracao(int minutos)
{
    if (minutos <= 0)
    {
        return "0min";
    }

    /*
     * TimeSpan.FromMinutes dá a conta pronta, mas repare em TotalHours x
     * Hours: Hours zera a cada 24 (1440 min daria 0), TotalHours não. Por
     * isso a divisão direta, que é o que o enunciado pede.
     */
    int horas = minutos / 60;
    int resto = minutos % 60;

    if (horas == 0)
    {
        return $"{resto}min";
    }

    // Hora cheia não mostra "0min" pendurado no fim.
    return resto == 0
        ? $"{horas}h"
        : string.Create(CultureInfo.InvariantCulture, $"{horas}h {resto}min");
}
