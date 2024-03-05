namespace Cineder_UI.Web.Models.Api
{
    public record SeriesDetail(long Id, string Name, IEnumerable<CreatedBy> CreatedBy, IEnumerable<int> Runtime, DateTime FirstAirDate, IEnumerable<Genre> Genres, bool InProduction, IEnumerable<string> Languages, DateTime LastAirDate, LastEpisodeToAir LastEpisodeToAir, object NextEpisodeToAir, IEnumerable<Network> Networks, int NumberOfEpisodes, int NumberOfSeasons, IEnumerable<string> OriginCountry, string OriginalLanguage, string OriginalName, string Overview, double Popularity, string PosterPath, IEnumerable<ProductionCompany> ProductionCompanies, IEnumerable<Seasons> Seasons, string Status, double VoteAverage, IEnumerable<Cast> Casts, IEnumerable<Video> Videos)
    {
        public SeriesDetail() : this(0, string.Empty, Enumerable.Empty<CreatedBy>(), Enumerable.Empty<int>(), DateTime.MinValue, Enumerable.Empty<Genre>(), false, Enumerable.Empty<string>(), DateTime.MinValue, new(), new(), Enumerable.Empty<Network>(), 0, 0, Enumerable.Empty<string>(), string.Empty, string.Empty, string.Empty, 0.0, string.Empty, Enumerable.Empty<ProductionCompany>(), Enumerable.Empty<Seasons>(), string.Empty, 0.0, Enumerable.Empty<Cast>(), Enumerable.Empty<Video>()) { }
    }

    public record SeriesResult(long Id, string Name, DateTime FirstAirDate, IEnumerable<string> OriginCountry, string PosterPath, string Overview, IEnumerable<long> GenreIds, double VoteAverage, int Idx, int SearchType)
    {
        public SeriesResult() : this(0, string.Empty, DateTime.MinValue, Enumerable.Empty<string>(), string.Empty, string.Empty, Enumerable.Empty<long>(), 0.0, 0, 1) { }
    }

    public record CreatedBy(long Id, string Name, string CreditId, int Gender, string ProfilePath)
    {
        public CreatedBy() : this(0, string.Empty, string.Empty, 0, string.Empty) { }
    }

    public record LastEpisodeToAir(long Id, string Name, DateTime AirDate, int EpisodeNumber, string Overiew, string ProductionCode, int SeasonNumber, long ShowId, string StillPath, double VoteAverage, int VoteCount)
    {
        public LastEpisodeToAir() : this(0, string.Empty, DateTime.MinValue, 0, string.Empty, string.Empty, 0, 0, string.Empty, 0.0, 0) { }
    }

    public record Seasons(long Id, string Name, DateTime AirDate, int EpisodeCount, string Overview, string PosterPath, int SeasonNumber)
    {
        public Seasons() : this(0, string.Empty, DateTime.MinValue, 0, string.Empty, string.Empty, 0) { }
    }

    public record Network(long Id, string Name, string LogoPath, string OriginCountry)
    {
        public Network() : this(0, string.Empty, string.Empty, string.Empty) { }
    }

    public record GetSeriesRequest(string SearchText, int PageNum = 1)
    {
        public GetSeriesRequest() : this(string.Empty, 1) { }
    }

    public record GetSeriesByIdRequest(long Id)
    {
        public GetSeriesByIdRequest() : this(0) { }
    }

    public record GetSeriesSimilarRequest(long SeriesId, int PageNum = 1)
    {
        public GetSeriesSimilarRequest() : this(0, 1) { }
    }
}
