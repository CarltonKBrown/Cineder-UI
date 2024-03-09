using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Common;
using Features.Home.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cineder_UI.Web.Features.Home
{
    public partial class Index: IDisposable
    {
        [SupplyParameterFromForm]
        public LandingPageModel? Model { get; set; } = new();

        [Inject]
        IJSRuntime? Js { get; set; }

        [Inject]
        IStateContainer? Store { get; set; }

        protected override void OnInitialized()
        {
            Store!.OnStateChanged += StateHasChanged;

            Store!.InitializeStore();
        }

        protected override void OnParametersSet()
        {
            if (string.IsNullOrWhiteSpace(Model?.SearchText ?? "") || (Model?.SearchType ?? 0) < 1)
            {
                return;
            }

            Model = SetModelFromStore();
        }

        private LandingPageModel SetModelFromStore()
        {
            var siteMode = Store!.State.SiteMode;

            var searchText = siteMode == SiteMode.Series ? Store!.State.SeriesState.SearchText : Store!.State.MovieState.SearchText;

            return new(searchText, (int)siteMode);
        }
        private static IEnumerable<SiteMode> SiteModesOptions => Enum.GetValues<SiteMode>().Where(x => x != SiteMode.None);

        private static string RadioButtonId(SiteMode btnId) => $"search-type-{(int)btnId}";

        public async Task Submit()
        {
            var siteMode = (SiteMode)Model!.SearchType;

            var searchText = Model!.SearchText;

            await Store!.SetHomePageSearch(searchText, siteMode);

            await Js!.InvokeVoidAsync("alert", $"Store: {Store!.State}");
        }

        public void Dispose()
        {
            Store!.OnStateChanged -= StateHasChanged;
        }
    }
}
