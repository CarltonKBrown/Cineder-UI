using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Components
{
    public partial class PageControlBar
    {
        [Parameter]
        public string CurrentPage { get; set; } = "1";

        [Parameter]
        public string TotalPages { get; set; } = "1";

        [Parameter]
        public EventCallback<string> OnPageChange { get; set; }

        private string LastPageBeforeChange {  get; set; } = "1";

        private async Task ChangePage()
        {
            if (CurrentPage.Equals(LastPageBeforeChange))
            {
                return;
            }

            if (!int.TryParse(CurrentPage, out _))
            {
                return;
            }

            LastPageBeforeChange = CurrentPage;

            await OnPageChange.InvokeAsync(CurrentPage);
        }

        private async Task BtnNext()
        {
            if(!int.TryParse(CurrentPage, out var intData))
            {
                return;
            }

            var newPage = ++intData;

            await OnPageChange.InvokeAsync(newPage.ToString());
        }

        private async Task BtnPrev()
        {
            if (!int.TryParse(CurrentPage, out var intData))
            {
                return;
            }

            var newPage = intData < 2 ? 1 : --intData;

            await OnPageChange.InvokeAsync(newPage.ToString());
        }

        private async Task BtnFirstPage()
        {
            await OnPageChange.InvokeAsync("1");
        }

        private async Task BtnLastPage()
        {
            if (!int.TryParse(TotalPages, out var lastPage))
            {
                return;
            }

            await OnPageChange.InvokeAsync(lastPage.ToString());
        }
    }
}
