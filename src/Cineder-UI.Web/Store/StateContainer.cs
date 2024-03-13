using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.Extensions.Options;

namespace Cineder_UI.Web.Store
{
    public class StateContainer : IStateContainer
    {
        private readonly IBrowserStorageService _browserStorage;
        private readonly SessionOptions _sessionOptions;
        private readonly IMovieService _movieService;   
        public StateContainer(IBrowserStorageService browserStorage, IOptionsSnapshot<SessionOptions> sessionOptions, IMovieService movieService)
        {
            _browserStorage = browserStorage;
            _sessionOptions = sessionOptions.Value;
            _movieService = movieService;
        }

        public AppState State { get; private set; } = new();

        public event Action OnStateChanged = () => { };

        public void NotifyStateChanged() => OnStateChanged?.Invoke();

        public async Task InitializeStore()
        {
            await RefreshStateAsync();

            NotifyStateChanged();
        }

        private async Task<AppState> GetAppStateAsync()
        {
            try
            {
                var storeName = _sessionOptions.StoreName;

                return await _browserStorage.GetSessionStorageItemAsync<AppState>(storeName);
            }
            catch (Exception)
            {
                await RestartSessionAsync();

                return State;
            }
        }

        private async Task CommitAppStateAsync(AppState state)
        {
            try
            {
                var storeName = _sessionOptions.StoreName;

                State = state;

                await _browserStorage.SetSessionStorageItemAsync(storeName, State);

                NotifyStateChanged();
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        public async Task RefreshStateAsync()
        {
            var currentState = await GetAppStateAsync() ?? new();

            await CommitAppStateAsync(currentState);
        }

        public async Task RestartSessionAsync()
        {
            await CommitAppStateAsync(new());
        }

        public async Task SetSiteMode(SiteMode siteMode)
        {
            var currentState = await GetAppStateAsync();

            currentState = currentState with { SiteMode = siteMode };

            await CommitAppStateAsync(currentState);
        }

        public async Task SetSearchText(string searchText)
        {
            try
            {
                var currentState = await GetAppStateAsync();

                switch (currentState.SiteMode)
                {

                    case SiteMode.Movie:
                        currentState = currentState with { MovieState = currentState.MovieState with { SearchText = searchText } };
                        break;
                    case SiteMode.Series:
                        currentState = currentState with { SeriesState = currentState.SeriesState with { SearchText = searchText } };
                        break;
                    case SiteMode.None:
                    default:
                        break;
                }

                await CommitAppStateAsync(currentState);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        public async Task SetHomePageSearch(string searchText, SiteMode siteMode)
        {
            try
            {
                var currentState = await GetAppStateAsync();

                switch (siteMode)
                {

                    case SiteMode.Movie:
                        currentState = currentState with { SiteMode = siteMode, MovieState = currentState.MovieState with { SearchText = searchText } };
                        break;
                    case SiteMode.Series:
                        currentState = currentState with { SiteMode = siteMode, SeriesState = currentState.SeriesState with { SearchText = searchText } };
                        break;
                    case SiteMode.None:
                    default:
                        break;
                }

                await CommitAppStateAsync(currentState);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

		public async Task SetMoviesSearch(string searchText, int page)
		{
            try
            {
                var currentState = await GetAppStateAsync();

                //if (currentState.MovieState.SearchText.Equals(searchText) && currentState.MovieState.SearchResult.Page == page)
                //{
                //    return;
                //}

                var request = new GetMoviesRequest(searchText, page);

                var movieResults = await _movieService.GetMovies(request);

				currentState = currentState with
				{
					MovieState = currentState.MovieState with
					{
						SearchText = searchText,
						SearchResult = movieResults
					}
				};

				await CommitAppStateAsync(currentState);
			}
            catch (Exception)
            {
                await InitializeStore();
            }
		}
	}
}
