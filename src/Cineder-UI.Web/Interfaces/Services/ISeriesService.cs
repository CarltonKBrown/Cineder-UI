using Cineder_UI.Web.Models.Api;

namespace Cineder_UI.Web.Interfaces.Services
{
    public interface ISeriesService
    {
        Task<SearchResult<SeriesResult>> GetSeries(GetSeriesRequest request);

        Task<SeriesDetail> GetSeriesByIdAsync(GetSeriesByIdRequest request);

        Task<SearchResult<SeriesResult>> GetSeriesSimilar(GetSeriesSimilarRequest request);
    }
}