using Cineder_UI.Web.Features.MoviesSearch.Models;
using Cineder_UI.Web.Features.SeriesSearch.Models;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cineder_UI.Web.Features.SeriesSearch
{
    public partial class Index
    {
        [Inject]
        NavigationManager NavMngr { get; set; } = default!;

        [Inject]
        IStateContainer? Store { get; set; }

        [SupplyParameterFromForm]
        public SeriesSearchPageModel PageModel { get; set; } = new();

        [Parameter]
        [SupplyParameterFromQuery]
        public string SearchText { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public int Page { get; set; } = 1;

        public int TotalPages => PageModel?.SeriesResults?.TotalPages ?? 1;

        private bool IsBusy { get; set; } = false;
        public string PageTitle { get; } = "Series";

        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            Store!.OnStateChanged += StateHasChanged;

            await Store!.InitializeStore();

            await SearchSeries();

            var series = Store?.State.SeriesState.SearchResult ?? new();

            PageModel ??= new(SearchText, 0, series);

            await base.OnInitializedAsync();

            IsBusy = false;
        }

        protected override void OnParametersSet()
        {
            IsBusy = true;

            if (SearchText != PageModel.Search || Page != (PageModel?.SeriesResults?.Page ?? 1))
            {
                PageModel = new(SearchText, 0, Store!.State.SeriesState.SearchResult);
            }

            base.OnParametersSet();

            IsBusy = false;
        }

        private async Task SearchSeries()
        {
            IsBusy = true;

            await Store!.SetSeriesSearch(SearchText, Page);

            IsBusy = false;

            NavMngr.NavigateTo($"/series?searchText={SearchText}&page={Page}");
        }

        private static IEnumerable<BreadCrumbItem> NavItems
        {
            get
            {
                return
                [
                    new BreadCrumbItem("Home", "/", false),
                    new BreadCrumbItem("Series", "/series", true),
                ];
            }
        }

        private async Task ToDetailsPage(long seriesId)
        {
            await Store!.SetSeriesDetail(seriesId);

            NavMngr.NavigateTo($"/series/{seriesId}");
        }

        private async Task ChangePage(int pageNum)
        {
            if (pageNum < 1 || pageNum.Equals(Store!.State.SeriesState.SearchResult.Page))
            {
                return;
            }
            await Store!.SetPage(pageNum);

            Page = Store!.State.SeriesState.SearchResult.Page;

            await SearchSeries();
        }

        private async Task ChangeSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText) || searchText.Equals(Store!.State.SeriesState.SearchText))
            {
                return;
            }

            await Store!.SetSearchText(searchText);

            await Store!.SetPage(1);

            SearchText = Store!.State.SeriesState.SearchText;

            Page = Store!.State.SeriesState.SearchResult.Page;

            await SearchSeries();
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

            SortSeries(sortOption);

            IsBusy = false;
        }

        private void SortSeries(SortOptions sortOption)
        {
            PageModel.SeriesResults = sortOption switch
            {
                SortOptions.AlphaAsc => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderBy(x => x.Name)
                },
                SortOptions.AlphaDesc => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderByDescending(x => x.Name)
                },
                SortOptions.YearAsc => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderBy(x => x.FirstAirDate.Year)
                },
                SortOptions.YearDesc => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderByDescending(x => x.FirstAirDate.Year)
                },
                SortOptions.RatingsAsc => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderBy(x => x.VoteAverage)
                },
                SortOptions.RatingsDesc => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderByDescending(x => x.VoteAverage)
                },
                _ or SortOptions.None => PageModel.SeriesResults! with
                {
                    Results = PageModel.SeriesResults.Results.OrderBy(x => x.Idx)
                }
            };
        }
    }
}
