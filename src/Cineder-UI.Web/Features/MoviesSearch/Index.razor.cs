using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cineder_UI.Web.Features.MoviesSearch
{
    public partial class Index
    {

        [Parameter]
        [SupplyParameterFromQuery]
        public string SearchText { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public int Page { get; set; } = 1;

		[Inject]
		IJSRuntime? JSRuntime { get; set; }

		[Inject]
		IStateContainer? Store { get; set; }

		public MovieSearch? Model { get; set; } = new();
		public MovieFilter? Filter { get; set; } = new();
		public SearchResult<MoviesResult>? MoviesResults { get; set; } = new();

		public class MovieSearch
		{
            public string? Search { get; set; }
        }

		public class MovieFilter
		{
            public MovieFilter()
            {
				Selected = (int)SortOptions.None;
            }

            public int? Selected { get; set; } = (int)SortOptions.None; 
		}

		protected override async Task OnInitializedAsync()
		{
			Store!.OnStateChanged += StateHasChanged;

			await Store!.InitializeStore();

			await SearchMovies();

			Model ??= new();

			Filter ??= new();

			MoviesResults  = Store?.State.MovieState.SearchResult ?? new();

			await base.OnInitializedAsync();
		}

		protected override void OnParametersSet()
		{
			Model!.Search = Store!.State.MovieState.SearchText;

			MoviesResults = Store!.State.MovieState.SearchResult;

			base.OnParametersSet();
		}

		private static IEnumerable<FilterOption>? FilterOptions
		{
			get
			{
				var opt = Enum.GetValues<SortOptions>();

				var finalList = opt.Select(FilterOption.FromSortOptions);

				return finalList;
			}
		}

		private async Task SearchMovies()
		{
			await Store!.SetMoviesSearch(SearchText, Page);
		}
	}
}
