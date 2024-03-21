using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Cineder_UI.Web.Components
{
    public partial class SortBar
    {
        [Parameter]
        public int Selected { get; set; } = 0;

        [Parameter]
        public EventCallback<int> OnSortChanged { get; set; }

        public int LastSelected { get; set; } = 0;

        private static IEnumerable<FilterOption>? FilterOptions
        {
            get
            {
                var opt = Enum.GetValues<SortOptions>();

                var finalList = opt.Select(FilterOption.FromSortOptions);

                return finalList;
            }
        }

        private async Task SortChanged()
        {
            if(Selected < 0 || Selected == LastSelected)
            {
                return;
            }
            
            if (!Enum.IsDefined(typeof(SortOptions), Selected))
            {
                return;
            }

            LastSelected = Selected;

            await OnSortChanged.InvokeAsync(Selected);
        }
    }
}
