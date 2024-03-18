using Contracts.Client.Models;
using Contracts.Client.Services;
using Services.Client;

namespace TrainingToolsApp.Services;

public class CrossPlatformFetchService : IFetchService
{
    private readonly IFetchService _fetchService;
    public CrossPlatformFetchService(IHttpClientFactory httpClientFactory)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            _fetchService = new DefaultFetchService(httpClientFactory);
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            _fetchService = new AndroidFetchService();
        }
        else
        {
            _fetchService = new AndroidFetchService();
        }
    }
    public Task<HttpResponse> Fetch(HttpRequest request)
    { 
        return _fetchService.Fetch(request);
    }
}