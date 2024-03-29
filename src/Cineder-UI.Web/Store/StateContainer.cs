using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Api;
using Cineder_UI.Web.Models.Common;
using Microsoft.Extensions.Options;
using System.IO;

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

        public async Task SetPage(int page)
        {
            try
            {
                switch (State.SiteMode)
                {
                    case SiteMode.Movie:
                        await SetMoviePage(page);
                        break;
                    case SiteMode.Series:
                        await SetSeriesPage(page);
                        break;
                    case SiteMode.None:
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        private async Task SetMoviePage(int page)
        {
            try
            {
                if (State.MovieState.SearchResult.Page.Equals(page))
                {
                    return;
                }

                var currentState = State with
                {
                    MovieState = State.MovieState with
                    {
                        SearchResult = State.MovieState.SearchResult with
                        {
                            Page = page
                        }
                    }
                };

                await CommitAppStateAsync(currentState);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        private async Task SetSeriesPage(int page)
        {
            try
            {
                if (State.SeriesState.SearchResult.Page.Equals(page))
                {
                    return;
                }

                var currentState = State with
                {
                    SeriesState = State.SeriesState with
                    {
                        SearchResult = State.SeriesState.SearchResult with
                        {
                            Page = page
                        }
                    }
                };

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
                switch (siteMode)
                {
                    case SiteMode.Movie:
                        await SetHomePageMovieSearch(searchText);
                        return;
                    case SiteMode.Series:
                        var currentState = State with { SiteMode = siteMode, SeriesState = State.SeriesState with { SearchText = searchText } };
                        await CommitAppStateAsync(currentState);
                        return;
                    case SiteMode.None:
                    default:
                        return;
                }
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        private async Task SetHomePageMovieSearch(string searchText) 
        {
            if (State.MovieState.SearchText.Equals(searchText, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            
            var movieResults = await SearchMovies(searchText);

            var currentState = State with 
            { 
                SiteMode = SiteMode.Movie, 
                MovieState = State.MovieState with 
                { 
                    SearchText = searchText, 
                    SearchResult = movieResults 
                } 
            };

            await CommitAppStateAsync(currentState);
        }


		public async Task SetMoviesSearch(string searchText, int page)
		{
            try
            {
                var movieResults = await SearchMovies(searchText, page);

                var currentState = State with
				{
					MovieState = State.MovieState with
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

        private async Task<SearchResult<MoviesResult>> SearchMovies(string searchText, int page = 1)
        {
            var request = new GetMoviesRequest(searchText, page);

            var response = await _movieService.GetMovies(request);

            if ((response.Results?.Count() ?? 0) < 1)
            {
                var totalResults = State.MovieState.SearchResult?.TotalResults ?? 0;

                var totalPage = State.MovieState.SearchResult?.TotalPages ?? 0;

                response = new(page, [], totalResults, totalPage);
            }

            return response;
        }
    }
}
