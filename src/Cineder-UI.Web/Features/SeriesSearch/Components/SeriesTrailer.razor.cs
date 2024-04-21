using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.SeriesSearch.Components
{
    public partial class SeriesTrailer
    {
        [Parameter]
        public string SeriesName { get; set; } = string.Empty;

        [Parameter]
        public IEnumerable<Video> SeriesVideos { get; set; } = [];

        [Parameter]
        public EventCallback OnTrailerLoaded { get; set; }


        private Video Trailer => SeriesVideos?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Key)) ?? new();

        private string TrailerName => Trailer?.Name ?? $"{SeriesName} Trailer";

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
