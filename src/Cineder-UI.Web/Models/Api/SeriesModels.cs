namespace Cineder_UI.Web.Models.Api
{
    public record CreatedBy(long Id, string Name, string CreditId, int Gender, string ProfilePath)
    {
        public CreatedBy() : this(0, string.Empty, string.Empty, 0, string.Empty) { }
    }

    public record LastEpisodeToAir(long Id, string Name, DateTime AirDate, int EpisodeNumber, string Overiew, string ProductionCode, int SeasonNumber, long ShowId, string StillPath, double VoteAverage, int VoteCount)
    {
        public LastEpisodeToAir() : this(0, string.Empty, DateTime.MinValue, 0, string.Empty, string.Empty, 0, 0, string.Empty, 0.0, 0) { }
    }
}
