using Cineder_UI.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cineder_UI.Web.Features.Home
{
	public partial class Index
	{
        [SupplyParameterFromForm]
        public LandingPageModel? Model { get; set; } = new();

		[Inject]
		IJSRuntime? Js { get; set; }
		

		private SearchType[] SearchTypes =
		[
			new("Movies", 0),
			new("Series", 1)
		];

		protected override void OnInitialized()
		{
			Model = new(string.Empty, 0);
		}

		private string RadioButtonId(int btnId) => $"search-type-{btnId}";

        public async Task Submit()
		{
			await Js!.InvokeVoidAsync("alert", $"Seach Text: {Model?.SearchText ?? ""} | Search Type: {Model?.SearchType ?? 0}");
		}
	}
}
