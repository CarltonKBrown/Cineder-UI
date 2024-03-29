using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.Extensions.Options;
using PreventR;

namespace Cineder_UI.Web.Services;

public class MovieService : HttpServiceBase, IMovieService
{
    private readonly CinederApiOptions _apiOptions;

    public MovieService(HttpClient httpClient, IOptionsSnapshot<CinederApiOptions> apiOptions) : base(httpClient)
    {
        _apiOptions = apiOptions.Value.Prevent(nameof(apiOptions)).Value;
    }

    public async Task<MovieDetail> GetMovieByIdAsync(GetMovieIdRequest request)
    {
        var url = $"{_apiOptions.BaseUrl}/movies/{request.Id}";

        var response = await GetAsync<MovieDetail>(url);

        return response ?? new();
    }

    public async Task<SearchResult<MoviesResult>> GetMovies(GetMoviesRequest request)
    {
        var url = $"{_apiOptions.BaseUrl}/movies?search={request.SearchText}&page={request.Page}";

        var response = await GetAsync<SearchResult<MoviesResult>>(url);

        return response ?? new();
    }

    public async Task<SearchResult<MoviesResult>> GetMoviesSimilar(GetMoviesSimilarRequest request)
    {
        var url = $"{_apiOptions.BaseUrl}/movies/similar/{request.MovieId}?page={request.Page}";

        var response = await GetAsync<SearchResult<MoviesResult>>(url);

        return response ?? new();
    }

}