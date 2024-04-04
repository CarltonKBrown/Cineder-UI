using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.SeriesSearch.Components
{
    public partial class SeriesList
    {
        private readonly int _cols = 4;

        [Parameter]
        public SearchResult<SeriesResult>? Series { get; set; } = new();

        [Parameter]
        public EventCallback<long> OnDetailsClicked { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        private int Rows
        {
            get
            {
                var resultsCount = Series?.Results?.Count() ?? 0;

                if (resultsCount < 1)
                {
                    return 0;
                }

                return Convert.ToInt32(Math.Ceiling((double)resultsCount / _cols));
            }
        }

        private IEnumerable<SeriesResult>[] RowItems
        {
            get
            {
                if ((Series?.Results?.Count() ?? 0) < 1 || Rows < 1)
                {
                    return [];
                }

                var itemRows = new IEnumerable<SeriesResult>[Rows];

                for (int i = 0; i < Rows; i++)
                {
                    itemRows[i] = Series!.Results.Skip(i * _cols).Take(_cols);
                }

                return itemRows;
            }
        }
        private static string ImagePath(string posterPath)
        {
            if (string.IsNullOrWhiteSpace(posterPath))
            {
                return "https://via.placeholder.com/400";
            }

            return $"https://image.tmdb.org/t/p/w400{posterPath}";
        }

        private void DetailsClicked(long movieId)
        {
            OnDetailsClicked.InvokeAsync(movieId);
        }
    }
}
