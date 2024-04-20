namespace Cineder_UI.Web.Models.Api
{
    public record MoviesResult(long Id, string Name, DateTime ReleaseDate, string PosterPath, string Overview, IEnumerable<long> GenreIds, double VoteAverage, int Idx, int SearchType)
    {
        public MoviesResult() : this(0, string.Empty, DateTime.MinValue, string.Empty, string.Empty, [], 0.0, 0, 0) { }
    }

    public record MovieDetail(long Id, string Name, double Budget, IEnumerable<Genre> Genres, string Overview, string PosterPath, IEnumerable<ProductionCompany> ProductionCompanies, DateTime ReleaseDate, double Revenue, double Runtime, double VoteAverage, IEnumerable<Video> Videos, IEnumerable<Cast> Casts)
    {
        public MovieDetail() : this(0, string.Empty, 0.0, [], string.Empty, string.Empty, [], DateTime.MinValue, 0.0, 0.0, 0.0, [], []) { }
    }

    public record GetMoviesRequest(string SearchText, int Page)
    {
        public GetMoviesRequest() : this(string.Empty, 0) { }
    }

    public record GetMovieByIdRequest(long Id)
    {
        public GetMovieByIdRequest() : this(0) { }
    }

    public record GetMoviesSimilarRequest(long MovieId, int Page)
    {
        public GetMoviesSimilarRequest() : this(0, 0) { }
    }
}
