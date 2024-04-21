using Cineder_UI.Web.Models.Api;
using Microsoft.AspNetCore.Components;

namespace Cineder_UI.Web.Features.SeriesSearch.Components
{
    public partial class SeriesBody
    {
        [Parameter]
        public string SeriesOverview { get; set; } = string.Empty;

        [Parameter]
        public IEnumerable<Cast> SeriesCasts { get; set; } = [];

        [Parameter]
        public IEnumerable<ProductionCompany> SeriesProductionCompanies { get; set; } = [];

        [Parameter]
        public IEnumerable<Network> SeriesNetworks { get; set; } = [];

        private string FormatCast
        {
            get
            {
                var casts = SeriesCasts ?? [];

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
                var productionCompanies = SeriesProductionCompanies ?? [];

                if (!productionCompanies.Any())
                {
                    return "N/A";
                }

                var productionCompaniesNames = productionCompanies?.Select(x => x.Name) ?? [];

                return string.Join(", ", productionCompaniesNames);
            }
        }

        private string FormatNetworks
        {
            get
            {
                var networks = SeriesNetworks ?? [];

                if (!networks.Any())
                {
                    return "N/A";
                }

                var networksNames = networks?.Select(x => x.Name) ?? [];

                return string.Join(", ", networksNames);
            }
        }
    }
}
