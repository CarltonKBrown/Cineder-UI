using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Components
{
    public partial class StarRating
    {
        [Parameter]
        public double Rating { get; set; }

        [Parameter]
        public string Colour { get; set; } = string.Empty;

        private int Rate { get; set; }

        private int EmptyRate {  get; set; }

        private bool IsFrac {  get; set; }

        private ICollection<string> WholeRatingClasses { get; set; } = ["bi", "bi-star-fill"] ;
        private ICollection<string> FracRatingClasses { get; set; } = ["bi", "bi-star-half"];
        private ICollection<string> EmptyRatingClasses { get; set; } = ["bi", "bi-star"];

        protected override void OnInitialized()
        {
            Rate = (int)Math.Floor(Rating);
            IsFrac = Math.Ceiling(Rating % 1) > 0;
            EmptyRate = IsFrac ? 5 - (Rate + 1) : 5 - Rate;
            WholeRatingClasses.Add(TextColourClass);
            FracRatingClasses.Add(TextColourClass);
            EmptyRatingClasses.Add(TextColourClass);
            base.OnInitialized();
        }

		protected override void OnParametersSet()
		{
			Rate = (int)Math.Floor(Rating);
			IsFrac = Math.Ceiling(Rating % 1) > 0;
            EmptyRate = IsFrac ? 5 - (Rate + 1) : 5 - Rate;
            WholeRatingClasses.Add(TextColourClass);
			FracRatingClasses.Add(TextColourClass);
            EmptyRatingClasses.Add(TextColourClass);
			base.OnParametersSet();
		}

		private string TextColourClass => Colour.ToLower() switch
        {
            "blue" => "text-primary",
            "grey" or "gray" => "text-secondary",
            "yellow" or "gold" => "text-warning",
            _ => "text-light"
        };
    }
}
