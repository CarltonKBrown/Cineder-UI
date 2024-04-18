using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.MoviesSearch.Components
{
    public partial class MovieBody
    {
        [Parameter]
        public string MovieOverview { get; set; }= string.Empty;

        [Parameter]
        public IEnumerable<Cast> MovieCasts { get; set; } = Enumerable.Empty<Cast>();

        [Parameter]
        public IEnumerable<ProductionCompany> MovieProductionCompanies { get; set; } = Enumerable.Empty<ProductionCompany>();


        private string FormatCast
        {
            get
            {
                var casts = MovieCasts ?? [];

                if (!casts.Any())
                {
                    return "N/A";
                }

                var castNames = casts?.Skip(0)?.Take(10)?.Select(x => x.Name) ?? [];

                return string.Join(", ", castNames);
            }
        }

        private string FormatProduction
        {
            get
            {
                var productionCompanies = MovieProductionCompanies ?? [];

                if (!productionCompanies.Any())
                {
                    return "N/A";
                }

                var productionCompaniesNames = productionCompanies?.Select(x => x.Name) ?? [];

                return string.Join(", ", productionCompaniesNames);
            }
        }
    }
}
