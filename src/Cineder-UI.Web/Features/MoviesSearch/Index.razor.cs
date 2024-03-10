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

		public MovieSearch? Model { get; set; }
		public MovieFilter? Filter { get; set; }

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

		protected override void OnInitialized()
		{
			Model ??= new();

			Filter ??= new();

			base.OnInitialized();
		}

		//protected override async Task OnParametersSetAsync()
		//{
		//	var search = SearchText;

		//	var page = Page;

		//	await JSRuntime!.InvokeVoidAsync("alert", $"Search: {search} | Page: {page}");

		//	base.OnParametersSet();
		//}

		private static IEnumerable<FilterOption>? FilterOptions
		{
			get
			{
				var opt = Enum.GetValues<SortOptions>();

				var finalList = opt.Select(FilterOption.FromSortOptions);

				return finalList;
			}
		}
	}
}
