using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.Extensions.Options;
using PreventR;

namespace Cineder_UI.Web.Services
{
    public class SeriesService : HttpServiceBase, ISeriesService
    {
        private readonly CinederApiOptions _apiOptions;

        public SeriesService(HttpClient httpClient, IOptionsSnapshot<CinederApiOptions> apiOptions) : base(httpClient)
        {
            _apiOptions = apiOptions.Value.Prevent(nameof(apiOptions)).Value;
        }

        public async Task<SearchResult<SeriesResult>> GetSeries(GetSeriesRequest request)
        {
            var url = $"{_apiOptions.BaseUrl}/series?search={request.SearchText}&page={request.Page}";

            return await GetAsync<SearchResult<SeriesResult>>(url);
        }

        public async Task<SeriesDetail> GetSeriesByIdAsync(GetSeriesByIdRequest request)
        {
            var url = $"{_apiOptions.BaseUrl}/series/{request.Id}";

            return await GetAsync<SeriesDetail>(url);
        }

        public async Task<SearchResult<SeriesResult>> GetSeriesSimilar(GetSeriesSimilarRequest request)
        {
            var url = $"{_apiOptions.BaseUrl}/series/similar/{request.SeriesId}?page={request.Page}";

            return await GetAsync<SearchResult<SeriesResult>>(url);
        }
    }
}