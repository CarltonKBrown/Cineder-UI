using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;

namespace Cineder_UI.Web.Store
{
    public record AppState(SiteMode SiteMode, MovieState MovieState, SeriesState SeriesState)
    {
        public AppState():this(SiteMode.None, new(), new()){}
    }

    public record MovieState(string SearchText, SearchResult<MoviesResult> SearchResult, MovieDetail MovieDetail, SearchResult<MoviesResult> Similar)
    {
        public MovieState() : this(string.Empty, new(), new(), new()){}
    }

    public record SeriesState(string SearchText, SearchResult<SeriesResult> SearchResult, SeriesDetail SeriesDetail, SearchResult<SeriesResult> Similar)
    {
        public SeriesState(): this(string.Empty, new(), new(), new()){}
    }
}
