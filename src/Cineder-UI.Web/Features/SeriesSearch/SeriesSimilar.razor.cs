using Cineder_UI.Web.Features.SeriesSearch.Models;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.SeriesSearch
{
	public partial class SeriesSimilar
	{
		[Inject]
		NavigationManager NavMngr { get; set; } = default!;

		[Inject]
		IStateContainer? Store { get; set; }

		[SupplyParameterFromForm]
		public SeriesSearchPageModel PageModel { get; set; } = new();

		[Parameter]
		public int Id { get; set; } = 0;

		[Parameter]
		[SupplyParameterFromQuery]
		public int Page { get; set; } = 1;

		public int TotalPages => PageModel?.SeriesResults?.TotalPages ?? 1;

		private bool IsBusy { get; set; } = false;
		public string PageTitle => $"Series like";

		private string SeriesPageLink => $"/series?searchText={Store.State.SeriesState.SearchText}&page={Store.State.SeriesState.SearchResult.Page}";

		private string SeriesName => Store.State.SeriesState.SeriesDetail.Name;
		private long SeriesId => Store.State.SeriesState.SeriesDetail.Id;

		private IEnumerable<BreadCrumbItem> NavItems =>
			[
				new BreadCrumbItem("Home", "/", false),
				new BreadCrumbItem("Series", SeriesPageLink, false),
				new BreadCrumbItem($"{SeriesName}", $"/series/{SeriesId}", false),
				new BreadCrumbItem($"Similar", $"/series/{SeriesId}/similar", true)
			];


		protected override async Task OnInitializedAsync()
		{
			IsBusy = true;

			Store!.OnStateChanged += StateHasChanged;

			await Store!.InitializeStore();

			await SearchSeries();

			var series = Store?.State.SeriesState.Similar ?? new();

            if (!(PageModel?.SeriesResults?.Results.Any() ?? false))
            {
				PageModel = new(Store!.State.SeriesState.SearchText, 0, series); 
			}

			await base.OnInitializedAsync();

			IsBusy = false;
		}

		protected override void OnParametersSet()
		{
			IsBusy = true;

			if (Id != Store.State.SeriesState.SeriesDetail.Id || Page != (PageModel?.SeriesResults?.Page ?? 1))
			{
				PageModel = new(Store!.State.SeriesState.SearchText, 0, Store!.State.SeriesState.Similar);
			}

			base.OnParametersSet();

			IsBusy = false;
		}

		private async Task SearchSeries()
		{
			IsBusy = true;

			if (Page < 1) Page = 1;

			await Store!.SetSimilarSeries(Id, Page);

			IsBusy = false;

			NavMngr.NavigateTo($"/series/{Id}/similar?page={Page}");
		}

		private async Task ToDetailsPage(long seriesId)
		{
			await Store!.SetSeriesDetail(seriesId);

			NavMngr.NavigateTo($"/series/{seriesId}");
		}

		private async Task ChangePage(int pageNum)
		{
			if (pageNum < 1 || pageNum.Equals(Store!.State.SeriesState.Similar.Page))
			{
				return;
			}

			await Store!.SetSimilarPage(pageNum);

			Page = Store!.State.SeriesState.Similar.Page;

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
