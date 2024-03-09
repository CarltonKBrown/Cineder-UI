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
        IStateContainer? Store { get; set; }

        [Inject]
        NavigationManager? NavigationManager { get; set; }

        protected override async Task  OnInitializedAsync()
        {
            Store!.OnStateChanged += StateHasChanged;

            await Store!.InitializeStore();

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            if (string.IsNullOrWhiteSpace(Model?.SearchText ?? "") || (Model?.SearchType ?? 0) < 1)
            {
                Model = SetModelFromStore();
            }

            base.OnParametersSet();
        }

        private LandingPageModel SetModelFromStore()
        {
            try
            {
                var siteMode = Store!.State.SiteMode;

                var searchText = siteMode == SiteMode.Series ? Store!.State.SeriesState.SearchText : Store!.State.MovieState.SearchText;

                return new(searchText, (int)siteMode);
            }
            catch (Exception)
            {
                return new();
            }
        }

        private static IEnumerable<SiteMode> SiteModesOptions => Enum.GetValues<SiteMode>().Where(x => x != SiteMode.None);

        private static string RadioButtonId(SiteMode btnId) => $"search-type-{(int)btnId}";

        public async Task Submit()
        {
            try
            {
                var siteMode = (SiteMode)Model!.SearchType;

                var searchText = Model!.SearchText;

                await Store!.SetHomePageSearch(searchText, siteMode);

                NavigationManager!.Refresh();
            }
            catch (Exception)
            {
                NavigationManager!.Refresh();
            }
        }

        public void Dispose()
        {
            Store!.OnStateChanged -= StateHasChanged;
        }
    }
}
