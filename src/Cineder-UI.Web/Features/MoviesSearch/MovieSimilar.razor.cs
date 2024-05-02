using Cineder_UI.Web.Features.MoviesSearch.Models;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch
{
    public partial class MovieSimilar
    {
        [Inject]
        NavigationManager NavMngr { get; set; } = default!;

        [Parameter]
        public int Id { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public int Page { get; set; } = 1;

        [Inject]
        IStateContainer Store { get; set; } = default!;

        [SupplyParameterFromForm]
        public MovieSearchPageModel PageModel { get; set; } = new();

        public int TotalPages => PageModel?.MoviesResults?.TotalPages ?? 1;

        private bool IsBusy { get; set; } = false;

        public string PageTitle => $"Movies like";

        private string MoviesPageLink => $"/movies?searchText={Store.State.MovieState.SearchText}&page={Store.State.MovieState.SearchResult.Page}";

        private string MovieName => Store.State.MovieState.MovieDetail.Name;
        private long MovieId => Store.State.MovieState.MovieDetail.Id;

        private IEnumerable<BreadCrumbItem> NavItems =>
            [
                new BreadCrumbItem("Home", "/", false),
                new BreadCrumbItem("Movies", MoviesPageLink, false),
                new BreadCrumbItem($"{MovieName}", $"/movies/{MovieId}", false),
                new BreadCrumbItem($"Similar", $"/movies/{MovieId}/similar", true)
            ];

        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            Store!.OnStateChanged += StateHasChanged;

            await Store!.InitializeStore();

            await SearchMovies();

            var movies = Store?.State.MovieState.Similar ?? new();

            if (!(PageModel?.MoviesResults?.Results.Any() ?? false))
            {
                PageModel = new(Store!.State.MovieState.SearchText, 0, movies);
            }

            await base.OnInitializedAsync();

            IsBusy = false;
        }

        protected override void OnParametersSet()
        {
            IsBusy = true;

            if (Id != Store.State.MovieState.MovieDetail.Id || Page != (PageModel?.MoviesResults?.Page ?? 1))
            {
                PageModel = new(Store!.State.MovieState.SearchText, 0, Store!.State.MovieState.Similar);
            }

            base.OnParametersSet();

            IsBusy = false;
        }

        private async Task SearchMovies()
        {
            IsBusy = true;

            if (Page < 1) Page = 1;

            await Store!.SetSimilarMovie(Id, Page);

            IsBusy = false;

            NavMngr.NavigateTo($"/movies/{Id}/similar?page={Page}");
        }

        private void SortMovies(SortOptions sortOption)
        {
            PageModel.MoviesResults = sortOption switch
            {
                SortOptions.AlphaAsc => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.Name)
                },
                SortOptions.AlphaDesc => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.Name)
                },
                SortOptions.YearAsc => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.ReleaseDate.Year)
                },
                SortOptions.YearDesc => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.ReleaseDate.Year)
                },
                SortOptions.RatingsAsc => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.VoteAverage)
                },
                SortOptions.RatingsDesc => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.VoteAverage)
                },
                _ or SortOptions.None => PageModel.MoviesResults! with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.Idx)
                }
            };
        }

            private void ChangeSort(int sortVal)
        {
            IsBusy = true;

            if (!Enum.IsDefined(typeof(SortOptions), sortVal))
            {
                return;
            }

            PageModel.Selected = sortVal;

            var sortOption = (SortOptions)sortVal;

            SortMovies(sortOption);

            IsBusy = false;
        }

        private async Task ChangePage(int pageNum)
        {
            if (pageNum < 1 || pageNum.Equals(Store!.State.MovieState.Similar.Page))
            {
                return;
            }
            await Store!.SetSimilarPage(pageNum);

            Page = Store!.State.MovieState.Similar.Page;

            await SearchMovies();
        }

        private async Task ToDetailsPage(long movieId)
        {
            await Store!.SetMovieDetail(movieId);

            NavMngr.NavigateTo($"/movies/{movieId}");
        }
    }
}
