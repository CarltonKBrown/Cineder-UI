using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;

namespace Cineder_UI.Web.Features.SeriesSearch.Models
{
    public class SeriesSearchPageModel
    {
        public SeriesSearchPageModel(string search, int selected, SearchResult<SeriesResult>? seriesResults)
        {
            Search = search;
            Selected = selected;
            SeriesResults = seriesResults;
        }

        public SeriesSearchPageModel():this(string.Empty, 0, new()) { }

        public string Search { get; set; } = string.Empty;
        public int Selected { get; set; } = (int)SortOptions.None;
        public SearchResult<SeriesResult>? SeriesResults { get; set; } = new();
    }

}
