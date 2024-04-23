using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.SeriesSearch.Components
{
    public partial class SeriesHeader
    {
        [Parameter]
        public string SeriesName { get; set; } = string.Empty;

        [Parameter]
        public double SeriesRating { get; set; } = double.MinValue;

        [Parameter]
        public DateTime SeriesReleasDate { get; set; } = DateTime.MinValue;

        [Parameter]
        public IEnumerable<Genre> SeriesGenre { get; set; } = [];

        [Parameter]
        public int SeriesSeasons { get; set; } = 0;

        [Parameter]
        public int SeriesEpisodes { get; set; } = 0;

        [Parameter]
        public string SeriesStatus { get; set; } = string.Empty;


        private string FormatGenres
        {
            get
            {
                var genres = SeriesGenre ?? [];

                if (!genres.Any())
                {
                    return "N/A";
                }

                var genreNames = genres?.Select(x => x.Name) ?? [];

                return string.Join(", ", genreNames);
            }
        }
    }
}
