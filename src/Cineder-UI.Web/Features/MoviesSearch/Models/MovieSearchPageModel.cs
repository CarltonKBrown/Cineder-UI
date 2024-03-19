using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;

namespace Cineder_UI.Web.Features.MoviesSearch.Models
{
    public class MovieSearchPageModel
    {
        public MovieSearchPageModel(string search, int selected, SearchResult<MoviesResult>? moviesResults)
        {
            Search = search;
            Selected = selected;
            MoviesResults = moviesResults;
        }

        public MovieSearchPageModel():this(string.Empty, 0, new()) { }

        public string Search { get; set; } = string.Empty;
        public int Selected { get; set; } = (int)SortOptions.None;
        public SearchResult<MoviesResult>? MoviesResults { get; set; } = new();
    }

}
