namespace Cineder_UI.Web.Models.Common
{
    public record CinederApiOptions(string BaseUrl)
    {
        public CinederApiOptions() : this(string.Empty) {}
    }

    public record SessionOptions(string StoreName, string StoreKey)
    {
        public SessionOptions(): this(string.Empty, string.Empty) { }
    }
}
