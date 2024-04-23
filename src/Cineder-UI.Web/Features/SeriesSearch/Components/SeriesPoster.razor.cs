using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.SeriesSearch.Components
{
    public partial class SeriesPoster
    {
        [Parameter]
        public string ImagePath { get; set; } = string.Empty;

        [Parameter]
        public EventCallback OnSimilarClicked { get; set; }

        [Parameter]
        public string BtnText { get; set; } = string.Empty;


        private async Task SimliarClick()
        {
            await OnSimilarClicked.InvokeAsync();
        }
        private string RenderImagePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ImagePath))
                {
                    return "https://via.placeholder.com/300";
                }

                return $"https://image.tmdb.org/t/p/w300{ImagePath}";
            }
        }
    }
}
