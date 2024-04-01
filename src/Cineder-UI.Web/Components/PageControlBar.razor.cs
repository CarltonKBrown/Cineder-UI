using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Components
{
    public partial class PageControlBar
    {
        [Parameter]
        public int CurrentPage { get; set; } = 1;

        [Parameter]
        public int TotalPages { get; set; } = 1;

        [Parameter]
        public EventCallback<int> OnPageChange { get; set; }

        private int LastPageBeforeChange {  get; set; } = 1;

        private async Task ChangePage(int newPage)
        {
            if (newPage.Equals(CurrentPage))
            {
                return;
            }

            await OnPageChange.InvokeAsync(newPage);
        }

        private async Task BtnNext()
        {
            if(CurrentPage.Equals(TotalPages))
            {
                return;
            }

            var nextPage = CurrentPage + 1;

			await ChangePage(nextPage);
        }

        private async Task BtnPrev()
        {
            if (CurrentPage.Equals(1))
            {
                return;
            }

            var previousPage = CurrentPage < 2 ? 1 : CurrentPage - 1;

            await ChangePage(previousPage);
        }

        private async Task BtnFirstPage()
        {
			if (CurrentPage.Equals(1))
			{
				return;
			}

			await ChangePage(1);
        }

        private async Task BtnLastPage()
        {
            if (CurrentPage.Equals(TotalPages))
            {
                return;
            }

            await ChangePage(TotalPages);
        }
    }
}
