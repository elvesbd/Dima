using MudBlazor;
using Dima.Core.Models;
using Dima.Core.Handlers;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;

namespace Dima.Web.Pages.Products;

public partial class ListProductsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public List<Product> Products { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IProductHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllProductsRequest();
            var result = await Handler.GetAllAsync(request);
            if (!result.IsSuccess)
            {
                Snackbar.Add(result.Message ?? "Não foi possível obter os produtos", Severity.Success);
                return;
            }
            
            Products = result.Data ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}