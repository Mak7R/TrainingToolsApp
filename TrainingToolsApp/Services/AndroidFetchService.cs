using Flurl.Http;
using Contracts.Client.Models;
using Contracts.Client.Services;
using Flurl;

namespace TrainingToolsApp.Services;

public class AndroidFetchService : IFetchService
{
    public async Task<HttpResponse> Fetch(HttpRequest request)
    {
        try
        {
            var flurlRequest = new FlurlRequest(new Url(request.Url));
            flurlRequest = request.Headers.Aggregate(flurlRequest, (current, header) => current.WithHeader(header.Key, string.Join(", ", header.Value)));
            
            IFlurlResponse response;
            if (request.Method == "GET" && string.IsNullOrWhiteSpace(request.Content))
            {
                response = await flurlRequest.GetAsync();
            }
            else if (request.Method == "GET")
            {
                flurlRequest.Content = new StringContent(request.Content, request.Encoding, request.ContentType);
                response = await flurlRequest.GetAsync();
            }
            else
            {
                response = await flurlRequest.SendAsync(HttpMethod.Parse(request.Method), new StringContent(request.Content, request.Encoding, request.ContentType));
            }
            
            return new HttpResponse(
                isSuccessStatusCode: response.ResponseMessage.IsSuccessStatusCode,
                statusCode: response.StatusCode,
                content: await response.ResponseMessage.Content.ReadAsStringAsync(),
                headers: response.ResponseMessage.Headers
            );
        }
        catch (Exception ex)
        {
            return new HttpResponse(
                isSuccessStatusCode: false,
                statusCode: 0,
                content: ex.Message,
                headers: new Dictionary<string, IEnumerable<string>>()
            );
        }
    }
}