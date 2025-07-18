using MudBlazor;
using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;

namespace Dima.Web.Components.Report;

public class IncomesByCategoryChartComponent : ComponentBase
{
    #region Properties

    public List<double> Data { get; set; } = [];
    public List<string> Labels { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; }  = null!;
    
    [Inject]
    public IReportsHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await GetIncomesByCategoryAsync();
    }

    private async Task GetIncomesByCategoryAsync()
    {
        var request = new GetIncomesByCategoryRequest();
        var result = await Handler.GetIncomesByCategoryReportAsync(request);
        if (!result.IsSuccess || result.Data == null)
        {
            Snackbar.Add("Falha ao obter os dados do relatório", Severity.Error);
            return;
        }

        foreach (var item in result.Data)
        {
            Labels.Add($"{item.Category} ({item.Incomes:C})");
            Data.Add((double)item.Incomes);
        }
    }

    #endregion
}