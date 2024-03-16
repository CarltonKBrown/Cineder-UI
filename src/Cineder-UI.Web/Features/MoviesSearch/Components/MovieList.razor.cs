using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch.Components
{
	public partial class MovieList
	{
        private readonly int _cols = 6;

        [Parameter]
        public SearchResult<MoviesResult>? Movies { get; set; } = new();

		protected override void OnParametersSet()
		{
			base.OnParametersSet();
		}

        private int Rows
        {
            get
            {
                var resultsCount = Movies?.Results?.Count() ?? 0;

                if(resultsCount < 1)
                {
                    return 0;
                }

                return Convert.ToInt32(Math.Ceiling((double)resultsCount / _cols));
            }
        }

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
                    itemRows[i] = Movies!.Results.Skip(i * _cols).Take(_cols);
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
