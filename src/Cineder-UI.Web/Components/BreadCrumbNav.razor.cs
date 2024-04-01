using Cineder_UI.Web.Models.Common;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Components
{
	public partial class BreadCrumbNav
	{
        [Parameter]
        public IEnumerable<BreadCrumbItem> BreadCrumbs { get; set; } = [];

		protected override void OnInitialized()
		{
            if (!BreadCrumbs?.Any() ?? false)
            {
                BreadCrumbs = [];
            }
            base.OnInitialized();
		}

		protected override void OnParametersSet()
		{
			if (!BreadCrumbs?.Any() ?? false)
			{
				BreadCrumbs = [];
			}

			base.OnParametersSet();
		}
	}
}
