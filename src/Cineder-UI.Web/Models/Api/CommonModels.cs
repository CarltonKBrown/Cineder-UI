namespace Cineder_UI.Web.Models.Api
{
    public record Cast(long Id, string Name, string Character, string CreditId, int Gender, int Order, string ProfilePath, bool Adult, string KnownForDepartment, double Popularity)
    {
        public Cast() : this(0, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty, false, string.Empty, 0.0) { }
    }

    public record Genre(long Id, string Name)
    {
        public Genre() : this(0, string.Empty) { }
    }

    public record ProductionCompany(long Id, string Name, string LogoPath, string OriginCountry)
    {
        public ProductionCompany() : this(0, string.Empty, string.Empty, string.Empty) { }
    }

    public record Video(string Id, string Name, string IsoLang, string IsoRegion, string Key, string Site, int Size, string Type, bool Official, DateTime PublishedAt)
    {
        public Video() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty, false, DateTime.Today) { }
    }

    public record SearchResult<T>(int Page, IEnumerable<T> Results, int TotalResults, int TotalPages)
    {
        public SearchResult() : this(1, Enumerable.Empty<T>(), 0, 0) { }
    }
}
