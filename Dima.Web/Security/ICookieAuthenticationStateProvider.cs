using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public interface ICookieAuthenticationStateProvider
{
    Task<bool> CheckAuthenticatedAsync();
    Task<AuthenticationState> GetAuthenticationStateAsync();
    void NotifyAuthenticationStateChanged();
}

public class CookieAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}