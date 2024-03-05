using Cineder_UI.Web.Models.Common;
using PreventR;
using System.Net.Http.Json;

namespace Cineder_UI.Web.Services;

public abstract class HttpServiceBase
{
    private readonly HttpClient _httpClient;

    public HttpServiceBase(HttpClient httpClient)
    {
        _httpClient = httpClient.Prevent(nameof(httpClient)).Null().Value;
    }

    protected async Task<ApiResponse<T>> GetAsync<T>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        return await SendRequest<T>(request);
    }

    private async Task<ApiResponse<T>> SendRequest<T>(HttpRequestMessage request)
    {
        try
        {
            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>(response.StatusCode);
            }

            var data = await response.Content.ReadFromJsonAsync<T>();

            return new(data!, response.StatusCode);
        }
        catch (Exception)
        {
            return default!;
        }
    }
}
