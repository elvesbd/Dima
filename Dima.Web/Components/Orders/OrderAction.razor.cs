using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Requests.Stripe;
using Dima.Web.Pages.Orders;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dima.Web.Components.Orders;

public partial class OrderActionComponent : ComponentBase
{
    #region Parameters

    [Parameter, EditorRequired]
    public Order Order { get; set; } = null!;
    
    [CascadingParameter]
    public DetailsPage Parent { get; set; } = null!;

    #endregion

    #region Services

    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public IJSRuntime JsRuntime { get; set; } = null!;
    
    [Inject]
    public IOrderHandler OrderHandler { get; set; } =  null!;
    
    [Inject]
    public IStripeHandler StripeHandler { get; set; } = null!;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Methods

    public async void OnCancelButtonClickedAsync()
    {
        var result = await DialogService.ShowMessageBox(
            "Atenção",
            "Deseja realmente cancelar este pedido?",
            yesText: "Sim",
            cancelText: "Não");
        
        if (result is not null && result == true)
            await CancelOrderAsync();
    }

    public async void OnPayButtonClickedAsync()
    {
        await PayOrderAsync();
    }
    
    public async void OnRefundButtonClickedAsync()
    {
        var result = await DialogService.ShowMessageBox(
            "Atenção",
            "Deseja realmente estornar este pedido?",
            yesText: "Sim",
            cancelText: "Não");
        
        if (result is not null && result == true)
            await RefundOrderAsync();
    }
    
    #endregion

    #region Private Methods

    private async Task CancelOrderAsync()
    {
        var request = new CancelOrderRequest
        {
            Id = Order.Id
        };
        
        var result = await OrderHandler.CancelAsync(request);
        
        if (result.IsSuccess)
            Parent.RefreshPage(result.Data!);
        else
            Snackbar.Add(result.Message!, Severity.Error);
    }

    private async Task PayOrderAsync()
    {
        var request = new CreateSessionRequest
        {
            OrderNumber = Order.Number,
            OrderTotal = (int)Math.Round(Order.Total * 100, 2),
            ProductTitle = Order.Product.Title,
            ProductDescription = Order.Product.Description
        };

        try
        {
            var result = await StripeHandler.CreateSessionAsync(request);
            if (result.IsSuccess == false || result.Data is null)
            {
                Snackbar.Add(result.Message!, Severity.Error);
                return;
            }

            await JsRuntime.InvokeVoidAsync("checkout", Configuration.StripePublicKey, result.Data);
        }
        catch
        {
            Snackbar.Add("Não foi possível iniciar a sessão com o Stripe", Severity.Error);
        }
    }
    
    private async Task RefundOrderAsync()
    {
        var request = new RefundOrderRequest
        {
            Id = Order.Id
        };
        
        var result = await OrderHandler.RefundAsync(request);
        
        if (result.IsSuccess)
            Parent.RefreshPage(result.Data!);
        else
            Snackbar.Add(result.Message!, Severity.Error);
    }

    #endregion
}