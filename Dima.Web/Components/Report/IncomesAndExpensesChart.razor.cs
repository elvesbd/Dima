using System.Globalization;
using MudBlazor;
using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;

namespace Dima.Web.Components.Report;

public class IncomesAndExpensesChartComponent : ComponentBase
{
    #region Properties

    public List<string> Labels { get; set; } = [];
    public List<ChartSeries>? Series { get; set; }
    public ChartOptions Options { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public IReportsHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var request = new GetIncomesAndExpensesRequest();
        var result = await Handler.GetIncomesAndExpensesReportAsync(request);
        if (!result.IsSuccess || result.Data == null)
        {
            Snackbar.Add("Falha ao obter os dados do relatório", Severity.Error);
            return;
        }

        var incomes = new List<double>();
        var expenses = new List<double>();

        foreach (var item in result.Data)
        {
            incomes.Add((double)item.Incomes);
            expenses.Add(-(double)item.Incomes);
            Labels.Add(GetMonthName(item.Month));
        }

        Options.YAxisTicks = 1000;
        Options.LineStrokeWidth = 5;
        Options.ChartPalette = ["#76FF01", Colors.Red.Default];

        Series =
        [
            new ChartSeries { Name = "Receitas", Data = incomes.ToArray() },
            new ChartSeries { Name = "Saídas", Data = expenses.ToArray() }
        ];
        
        StateHasChanged();
    }

    #endregion

    private static string GetMonthName(int month)
        => new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", CultureInfo.CurrentCulture);
}