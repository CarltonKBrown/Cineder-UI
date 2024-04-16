using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cineder_UI.Web.Features.MoviesSearch
{
	public partial class MovieDetails
	{
        [Parameter]
        public int Id { get; set; } = 0;

        [Inject]
        protected IStateContainer Store { get; set; } = default!;

        [Inject]
        protected IJSRuntime Js { get; set; } = default!;

        protected MovieDetail Movie { get; set; } = new();

        private string PageName => $"{Movie.Name}";
        private string MoviesPageLink => $"/movies?searchText={Store.State.MovieState.SearchText}&page={Store.State.MovieState.SearchResult.Page}";

        private IEnumerable<BreadCrumbItem> NavItems => 
            [
                new BreadCrumbItem("Home", "/", false),
                new BreadCrumbItem("Movies", MoviesPageLink, false),
                new BreadCrumbItem($"{Movie.Name}", $"/movies/{Id}", true)
            ];

        private bool IsLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            await Store.SetMovieDetail(Id);

            Movie = Store.State.MovieState.MovieDetail;

            await base.OnInitializedAsync();

            IsLoading = false;
        }

        protected override void OnParametersSet()
        {
            IsLoading = true;

            Movie = Store.State.MovieState.MovieDetail;

           base.OnParametersSetAsync();

            IsLoading = false;
        }

        private async Task SimilarClicked()
        {
            await Js.InvokeVoidAsync("alert", $"{Id} - {Movie.PosterPath}");
        }

        private Video MovieTrailer => Movie?.Videos?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Key)) ?? new();

        private string TrailerPath
        {
            get
            {
                var embedKey = MovieTrailer?.Key ?? string.Empty;

                var basePath = "https://www.youtube.com/embed/";

                return $"{basePath}{embedKey}";
            }
        }

        private string TrailerName => MovieTrailer?.Name ?? $"{Movie.Name} Trailer";

        private string FormatDuration
        {
            get
            {
                var duration = Movie?.Runtime ?? 0;

                if (duration < 1)
                {
                    return "N/A";
                }

                var hours = Math.Floor(duration / 60);

                var minutes = duration % 60;

                if (hours < 1)
                {
                    return $"{minutes} mins";
                }

                var hourDisplayUnit = hours > 1 ? "hrs" : "hr";

                return $"{hours} {hourDisplayUnit} {minutes} mins";
                
            }
        }

        private string FormatCast
        {
            get
            {
                var casts = Movie?.Casts ?? [];

                if (!casts.Any())
                {
                    return "N/A";
                }

                var castNames = casts?.Skip(0)?.Take(10)?.Select(x => x.Name) ?? [];

                return string.Join(", ", castNames) ;
            }
        }

        private string FormatGenres
        {
            get
            {
                var genres = Movie?.Genres ?? [];

                if (!genres.Any())
                {
                    return "N/A";
                }

                var genreNames = genres?.Select(x => x.Name) ?? [];

                return string.Join(", ", genreNames);
            }
        }

        private string FormatProduction
        {
            get
            {
                var productionCompanies = Movie?.ProductionCompanies ?? [];

                if (!productionCompanies.Any())
                {
                    return "N/A";
                }

                var productionCompaniesNames = productionCompanies?.Select(x => x.Name) ?? [];

                return string.Join(", ", productionCompaniesNames);
            }
        }
    }
}
