using Cineder_UI.Web.Features.MoviesSearch.Models;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch
{
    public partial class Index
    {
        [Inject]
        NavigationManager NavMngr { get; set; } = default!;

        [Inject]
        IStateContainer? Store { get; set; }

        [SupplyParameterFromForm]
        public MovieSearchPageModel PageModel { get; set; } = new();

        [Parameter]
        [SupplyParameterFromQuery]
        public string SearchText { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public int Page { get; set; } = 1;

        public string PageString => Page.ToString();

        public string TotalPagesString => PageModel!.MoviesResults!.TotalPages.ToString();

        protected override async Task OnInitializedAsync()
        {
            Store!.OnStateChanged += StateHasChanged;

            await Store!.InitializeStore();

            await SearchMovies();

            var movies = Store?.State.MovieState.SearchResult ?? new();

            PageModel ??= new(SearchText, 0, movies);

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            if (SearchText != PageModel.Search || Page != (PageModel?.MoviesResults?.Page ?? 1))
            {
                PageModel = new(SearchText, 0, Store!.State.MovieState.SearchResult);
            }

            base.OnParametersSet();
        }

        private async Task SearchMovies(string searchText = "", int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                SearchText = searchText;
            }

            if (page > 0)
            {
                Page = page;
            }

            await Store!.SetMoviesSearch(SearchText, Page);
        }

        private async Task ChangePage(string pageNum)
        {
            if (!int.TryParse(pageNum, out var pageNumInt))
            {
                return;
            }

            if ((Store?.State?.MovieState?.SearchResult?.Page ?? 1) == pageNumInt)
            {
                return;
            }

            await SearchMovies(string.Empty, pageNumInt);

            NavMngr.NavigateTo($"/movies?searchText={SearchText}&page={Page}");
        }

        private async Task ChangeSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            if ((Store?.State?.MovieState?.SearchText ?? "") == searchText)
            {
                return;
            }

            await SearchMovies(searchText);

            NavMngr.NavigateTo($"/movies?searchText={SearchText}&page={Page}");
        }

        private void ChangeSort(int sortVal)
        {
            if (!Enum.IsDefined(typeof(SortOptions), sortVal))
            {
                return;
            }

            PageModel.Selected = sortVal;

            var sortOption = (SortOptions)sortVal;

            SortMovies(sortOption);
        }

        private void SortMovies(SortOptions sortOption)
        {
            PageModel.MoviesResults = sortOption switch
            {
                SortOptions.AlphaAsc => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.Name)
                },
                SortOptions.AlphaDesc => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.Name)
                },
                SortOptions.YearAsc => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.ReleaseDate.Year)
                },
                SortOptions.YearDesc => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.ReleaseDate.Year)
                },
                SortOptions.RatingsAsc => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderBy(x => x.VoteAverage)
                },
                SortOptions.RatingsDesc => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.VoteAverage)
                },
                _ or SortOptions.None => PageModel.MoviesResults with
                {
                    Results = PageModel.MoviesResults.Results.OrderByDescending(x => x.Idx)
                }
            };
        }
    }
}
