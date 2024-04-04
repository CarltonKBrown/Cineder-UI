using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch
{
	public partial class MovieDetails
	{
        [Parameter]
        public int Id { get; set; } = 0;

        [Inject]
        protected IStateContainer Store { get; set; } = default!;

        protected MovieDetail Movie { get; set; } = new();

        private string PageName => $"{Movie.Name}";
        private string MoviesPageLink => $"/movies?searchText={Store.State.MovieState.SearchText}&page={Store.State.MovieState.SearchResult.Page}";

        private IEnumerable<BreadCrumbItem> NavItems => 
            [
                new BreadCrumbItem("Home", "/", false),
                new BreadCrumbItem("Movies", MoviesPageLink, false),
                new BreadCrumbItem($"{Movie.Name}", $"/movies/{Id}", true)
            ];


        protected override async Task OnInitializedAsync()
        {
            await Store.SetMovieDetail(Id);

            Movie = Store.State.MovieState.MovieDetail;

            await base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            Movie = Store.State.MovieState.MovieDetail;

            return base.OnParametersSetAsync();
        }

        private static string ImagePath(string posterPath)
        {
            if (string.IsNullOrWhiteSpace(posterPath))
            {
                return "https://via.placeholder.com/400";
            }

            return $"https://image.tmdb.org/t/p/w400{posterPath}";
        }
    }
}
