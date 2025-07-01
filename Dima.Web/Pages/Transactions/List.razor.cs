using MudBlazor;
using Dima.Core.Common;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;

namespace Dima.Web.Pages.Transactions;

public class ListTransactionsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    protected List<Transaction> Transactions { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public int CurrentYear { get; set; } = DateTime.Now.Year;
    public int CurrentMonth { get; set; } = DateTime.Now.Month;

    protected int[] Years { get; set; } =
    {
        DateTime.Now.Year,
        DateTime.Now.AddYears(-1).Year,
        DateTime.Now.AddYears(-2).Year,
        DateTime.Now.AddYears(-3).Year,
    };

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public ITransactionHandler Handler { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnSearchAsync()
    {
        await GetTransactionsAsync();
        StateHasChanged();
    }

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o lançamento {title} será excluido. Deseja continuar?",
            "Excluir",
            "Cancelar");
        if (result is true) await OnDeleteAsync(id, title);
        
        StateHasChanged();
    }

    public Func<Transaction, bool> Filter => transaction =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
        
        return transaction.Id.ToString().Contains(SearchTerm, comparison) ||
               transaction.Title.Contains(SearchTerm, comparison);
    };

    private async Task OnDeleteAsync(long id, string title)
    {
        IsBusy = true;

        try
        {
            var request = new DeleteTransactionRequest { Id = id };
            var result = await Handler.DeleteAsync(request);
            if (result.IsSuccess)
            {
                Snackbar.Add($"Lançamento {title} excluido!", Severity.Success);
                Transactions.RemoveAll(x => x.Id == id);
            }
            else
            {
                Snackbar.Add(result.Message!, Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GetTransactionsAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetTransactionsByPeriodRequest
            {
                StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                PageNumber = 1,
                PageSize = 1000
            };

            var result = await Handler.GetByPeriodAsync(request);
            if (result.IsSuccess) Transactions = result.Data ?? [];
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
        => await GetTransactionsAsync();

    #endregion
}