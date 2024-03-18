using System.Text.Json;
using Contracts.Client.Models;
using Contracts.Client.Services;

namespace TrainingToolsApp.Services;

public class MauiCookiesProvider : ICookiesProvider
{
    public async Task ToRequest(HttpRequest request)
    {
        var cookies = JsonSerializer.Deserialize<List<string>>(await SecureStorage.GetAsync("cookies") ?? "[]") ?? [];
        request.Headers.Add("Cookie", cookies);
    }

    public async Task FromResponse(HttpResponse response)
    {
        var responseCookies = response.Headers.FirstOrDefault(h => h.Key == "Set-Cookie");
        if (responseCookies.Key != null)
        {
            await SecureStorage.SetAsync("cookies", JsonSerializer.Serialize(responseCookies.Value));
        }
    }
}