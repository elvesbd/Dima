using MudBlazor;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;

namespace Dima.Web.Pages.Categories;

public partial class ListCategoriesPage : ComponentBase
{
    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    #endregion
    
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Category> Categories { get; set; } = [];

    public string SearchTerm { get; set; } = string.Empty;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
       IsBusy = true;

       try
       {
           var request = new GetAllCategoriesRequest();
           var result = await Handler.GetAllAsync(request);
           if (result.IsSuccess)
               Categories = result.Data ?? [];
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

    #region Methods

    public Func<Category, bool> Filter => category =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
    
        return category.Id.ToString().Contains(SearchTerm, comparison) ||
               category.Title.Contains(SearchTerm, comparison) ||
               category.Description?.Contains(SearchTerm, comparison) == true;
    };

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir a categoria {title} será excluída. Deseja continuar?",
            "Excluir",
            "Cancelar");

        if (result is true) await OnDeleteAsync(id, title);
        
        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            var request = new DeleteCategoryRequest { Id = id };
            await Handler.DeleteAsync(request);
            Categories.RemoveAll(x => x.Id == id);
            Snackbar.Add($"Categoria {title} excluída com sucesso!", Severity.Success);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }

    #endregion
}