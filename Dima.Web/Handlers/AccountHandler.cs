using Dima.Core.Handlers;
using Dima.Core.Responses;
using System.Net.Http.Json;
using System.Text;
using Dima.Core.Requests.Account;

namespace Dima.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await _client
            .PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        
        return result.IsSuccessStatusCode
            ? new Response<string>("Login Realizado com sucesso!", 200, "Login Realizado com sucesso!")
            : new Response<string>("", 400, "Não foi possível realizar o login!");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _client
            .PostAsJsonAsync("v1/identity/register", request);
        
        return result.IsSuccessStatusCode
            ? new Response<string>("Registro Realizado com sucesso!", 201, "Registro Realizado com sucesso!")
            : new Response<string>("", 400, "Não foi possível realizar o cadastro!");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _client.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }
}