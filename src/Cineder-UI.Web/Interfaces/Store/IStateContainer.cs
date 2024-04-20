using Cineder_UI.Web.Models.Common;
using Cineder_UI.Web.Store;

namespace Cineder_UI.Web.Interfaces.Store
{
    public interface IStateContainer
    {
        AppState State { get; }

        event Action OnStateChanged;

        void NotifyStateChanged();

        Task InitializeStore();

        Task RefreshStateAsync();

        Task RestartSessionAsync();

        Task SetSiteMode(SiteMode siteMode);
        Task SetSearchText(string searchText);
        Task SetPage(int page);

        Task SetHomePageSearch(string searchText, SiteMode siteMode);

        Task SetMoviesSearch(string searchText, int page);
        Task SetSeriesSearch(string searchText, int page);
        Task SetMovieDetail(long movieId);
    }
}
