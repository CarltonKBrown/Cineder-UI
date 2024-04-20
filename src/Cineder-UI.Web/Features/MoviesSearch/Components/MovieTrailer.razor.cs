using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch.Components
{
    public partial class MovieTrailer
    {
        [Parameter]
        public string MovieName { get; set; } = string.Empty;

        [Parameter]
        public IEnumerable<Video> MovieVideos { get; set; } = Enumerable.Empty<Video>();

        [Parameter]
        public EventCallback OnTrailerLoaded { get; set; }


        private Video Trailer => MovieVideos?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Key)) ?? new();

        private string TrailerName => Trailer?.Name ?? $"{MovieName} Trailer";

        private string TrailerPath
        {
            get
            {
                var embedKey = Trailer?.Key ?? string.Empty;

                var basePath = "https://www.youtube.com/embed/";

                return $"{basePath}{embedKey}";
            }
        }

        private async Task TrailerLoaded()
        {
            await OnTrailerLoaded.InvokeAsync();
        }
    }
}
