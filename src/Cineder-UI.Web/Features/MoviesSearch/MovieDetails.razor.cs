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
        NavigationManager NavMngr { get; set; } = default!;

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

            await Store!.InitializeStore();

            await Store.SetMovieDetail(Id);

            Movie = Store.State.MovieState.MovieDetail;

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            IsLoading = true;

            Movie = Store.State.MovieState.MovieDetail;

           base.OnParametersSetAsync();
        }

        private void SimilarClicked()
        {
            NavMngr.NavigateTo($"/movies/{Id}/similar?page=1");
        }

        private void OnPageLoad()
        {
            if (IsLoading)
            {
                IsLoading = false;
            }
        }
    }
}
