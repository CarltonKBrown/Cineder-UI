using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Components
{
    public partial class Loading
    {
        [Parameter]
        public bool IsLoading { get; set; } = false;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
