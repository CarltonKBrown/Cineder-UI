using Cineder_UI.Web.Models.Api;

namespace Cineder_UI.Web.Interfaces.Services
{
    public interface IMovieService
    {
        Task<SearchResult<MoviesResult>> GetMovies(GetMoviesRequest request);

        Task<MovieDetail> GetMovieByIdAsync(GetMovieIdRequest request);

        Task<SearchResult<MoviesResult>> GetMoviesSimilar(GetMoviesSimilarRequest request);
    }
}
