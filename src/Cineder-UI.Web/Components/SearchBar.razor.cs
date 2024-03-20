using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Components
{
    public partial class SearchBar
    {
        [Parameter]
        public string SearchText { get; set; } = string.Empty;

        [Parameter]
        public string PlaceHolder { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<string> OnSubmit { get; set; }


        private async Task SubmitSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return;
            }

            await OnSubmit.InvokeAsync(SearchText);
        }
    }
}
