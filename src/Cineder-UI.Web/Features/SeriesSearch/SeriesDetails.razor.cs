using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cineder_UI.Web.Features.SeriesSearch
{
	public partial class SeriesDetails
	{
		[Parameter]
		public int Id { get; set; } = 0;

		[Inject]
		protected IStateContainer Store { get; set; } = default!;

		[Inject]
		protected IJSRuntime Js { get; set; } = default!;

        protected SeriesDetail Series { get; set; } = new();

		private string PageName => $"{Series.Name}";

        private string SeriesPageLink => $"/series?searchText={Store.State.SeriesState.SearchText}&page={Store.State.SeriesState.SearchResult.Page}";

        private IEnumerable<BreadCrumbItem> NavItems =>
            [
                new BreadCrumbItem("Home", "/", false),
                new BreadCrumbItem("Series", SeriesPageLink, false),
                new BreadCrumbItem($"{Series.Name}", $"/series/{Id}", true)
            ];

        private bool IsLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            await Store!.InitializeStore();

            await Store.SetSeriesDetail(Id);

            Series = Store.State.SeriesState.SeriesDetail;

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            IsLoading = true;

            Series = Store.State.SeriesState.SeriesDetail;

            base.OnParametersSetAsync();
        }

        private async Task SimilarClicked()
        {
            await Js.InvokeVoidAsync("alert", $"{Id} - {Series.PosterPath}");
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
