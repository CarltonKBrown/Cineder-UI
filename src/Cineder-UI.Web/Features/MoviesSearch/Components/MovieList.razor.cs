using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch.Components
{
	public partial class MovieList
	{
        [Parameter]
        public SearchResult<MoviesResult>? Movies { get; set; }
        private int Cols = 4;
        private int Count { get; set; }

		protected override void OnParametersSet()
		{
            Count = Movies?.Results.Count() ?? 0;

			base.OnParametersSet();
		}

        private int Rows => Convert.ToInt32((double) Movies!.Results.Count() / Cols);

        private IEnumerable<MoviesResult>[] RowItems
        {
            get
            {
                if ((Movies?.Results?.Count() ?? 0) < 1 || Rows < 1)
                {
                    return [];
                }

                var itemRows = new IEnumerable<MoviesResult>[Rows];

				for (int i = 0; i < Rows; i++)
                {
                    itemRows[i] = Movies!.Results.Skip(i * Cols).Take(Cols);
				}

                return itemRows;
            }
        }
		private static string ImagePath(string posterPath)
        {
            if (string.IsNullOrWhiteSpace(posterPath))
            {
                return "https://via.placeholder.com/300";
			}

            return $"https://image.tmdb.org/t/p/w300{posterPath}";
        }
    }
}
