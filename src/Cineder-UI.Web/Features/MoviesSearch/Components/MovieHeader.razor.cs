using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch.Components
{
    public partial class MovieHeader
    {
        [Parameter]
        public string MovieName { get; set; } = string.Empty;

        [Parameter]
        public double MovieRating { get; set; } = double.MinValue;

        [Parameter]
        public DateTime MovieReleaseDate { get; set; }= DateTime.MinValue;

        [Parameter]
        public double MovieDuration { get; set; } = double.MinValue;

        [Parameter]
        public IEnumerable<Genre> MovieGenres { get; set; } = Enumerable.Empty<Genre>();


        private string FormatDuration
        {
            get
            {
                if (MovieDuration < 1)
                {
                    return "N/A";
                }

                var hours = Math.Floor(MovieDuration / 60);

                var minutes = MovieDuration % 60;

                if (hours < 1)
                {
                    return $"{minutes} mins";
                }

                var hourDisplayUnit = hours > 1 ? "hrs" : "hr";

                return $"{hours} {hourDisplayUnit} {minutes} mins";

            }
        }

        private string FormatGenres
        {
            get
            {
                var genres = MovieGenres ?? [];

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
