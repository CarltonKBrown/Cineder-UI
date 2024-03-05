namespace Cineder_UI.Web.Models.Common
{
    public record CinederApiOptions(string BaseUrl)
    {
        public CinederApiOptions() : this(string.Empty)
        {

        }
    }
}
