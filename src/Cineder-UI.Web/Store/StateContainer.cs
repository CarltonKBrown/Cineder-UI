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
        private readonly ISeriesService _seriesService;
        public StateContainer(IBrowserStorageService browserStorage, IOptionsSnapshot<SessionOptions> sessionOptions, IMovieService movieService, ISeriesService seriesService)
        {
            _browserStorage = browserStorage;
            _sessionOptions = sessionOptions.Value;
            _movieService = movieService;
            _seriesService = seriesService;
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
                        await SetHomePageSeriesSearch(searchText);
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
            var currentState = State with
            {
                SiteMode = SiteMode.Movie
            };

            if (!State.MovieState.SearchText.Equals(searchText, StringComparison.OrdinalIgnoreCase))
            {
                var movieResults = await SearchMovies(searchText);

                currentState = currentState with
                {
                    MovieState = State.MovieState with
                    {
                        SearchText = searchText,
                        SearchResult = movieResults
                    }
                };
            }

            await CommitAppStateAsync(currentState);
        }

        private async Task SetHomePageSeriesSearch(string searchText)
        {
            var currentState = State with
            {
                SiteMode = SiteMode.Series
            };

            if (!State.SeriesState.SearchText.Equals(searchText, StringComparison.OrdinalIgnoreCase))
            {
                var seriesResults = await SearchSeries(searchText);

                currentState = currentState with
                {
                    SeriesState = State.SeriesState with
                    {
                        SearchText = searchText,
                        SearchResult = seriesResults
                    }
                };
            }

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

        private async Task<SearchResult<SeriesResult>> SearchSeries(string searchText, int page = 1)
        {
            var request = new GetSeriesRequest(searchText, page);

            var response = await _seriesService.GetSeries(request);

            if ((response.Results?.Count() ?? 0) < 1)
            {
                var totalResults = State.SeriesState.SearchResult?.TotalResults ?? 0;

                var totalPage = State.SeriesState.SearchResult?.TotalPages ?? 0;

                response = new(page, [], totalResults, totalPage);
            }

            return response;
        }

        public async Task SetSeriesSearch(string searchText, int page)
        {
            try
            {
                var seriesResults = await SearchSeries(searchText, page);

                var currentState = State with
                {
                    SeriesState = State.SeriesState with
                    {
                        SearchText = searchText,
                        SearchResult = seriesResults
                    }
                };

                await CommitAppStateAsync(currentState);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        public async Task SetMovieDetail(long movieId)
        {
            try
            {
                if (movieId < 1 || State.MovieState.MovieDetail.Id.Equals(movieId))
                {
                    return;
                }

                var movieDetail = await FetchMovieDetails(movieId);

                if (movieDetail.Id < 1)
                {
                    return;
                }

                State = State with
                {
                    MovieState = State.MovieState with
                    {
                        MovieDetail = movieDetail
                    }
                };

                await CommitAppStateAsync(State);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        public async Task SetSeriesDetail(long seriesId)
        {
            try
            {
                if (seriesId < 1 || State.SeriesState.SeriesDetail.Id.Equals(seriesId))
                {
                    return;
                }

                var seriesDetail = await FetchSeriesDetails(seriesId);

                if (seriesDetail.Id < 1)
                {
                    return;
                }

                State = State with
                {
                    SeriesState = State.SeriesState with
                    {
                        SeriesDetail = seriesDetail
                    }
                };

                await CommitAppStateAsync(State);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        private async Task<MovieDetail> FetchMovieDetails(long movieId)
        {
            var request = new GetMovieByIdRequest(movieId);

            var response = await _movieService.GetMovieByIdAsync(request);

            if ((response?.Id ?? 0) < 1)
            {
                response = new();
            }

            return response ?? new();
        }

        private async Task<SeriesDetail> FetchSeriesDetails(long seriesId)
        {
            var request = new GetSeriesByIdRequest(seriesId);
            
            var response = await _seriesService.GetSeriesByIdAsync(request);

            if ((response?.Id ?? 0) < 1)
            {
                response = new();
            }

            return response ?? new();
        }

        public async Task SetSimilarMovie(long movieId, int page = 1)
        {
            try
            {
                var movieResults = await FetchSimilarMovies(movieId, page);

                var currentState = State with
                {
                    MovieState = State.MovieState with
                    {
                        Similar = movieResults
                    }
                };

                await CommitAppStateAsync(currentState);
            }
            catch (Exception)
            {
                await InitializeStore();
            }
        }

        public async Task<SearchResult<MoviesResult>> FetchSimilarMovies(long movieId, int page = 1)
        {
            var request = new GetMoviesSimilarRequest(movieId, page);

            var response = await _movieService.GetMoviesSimilar(request);

            if ((response.Results?.Count() ?? 0) < 1)
            {
                var totalResults = State.MovieState.Similar?.TotalResults ?? 0;

                var totalPage = State.MovieState.Similar?.TotalPages ?? 0;

                response = new(page, [], totalResults, totalPage);
            }

            return response;
        }

        public async Task SetSimilarPage(int page)
        {
            try
            {
                switch (State.SiteMode)
                {
                    case SiteMode.Movie:
                        await SetMovieSimilarPage(page);
                        break;
                    case SiteMode.Series:
                        await SetSeriesSimilarPage(page);
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
        private async Task SetMovieSimilarPage(int page)
        {
            try
            {
                if (State.MovieState.Similar.Page.Equals(page))
                {
                    return;
                }

                var currentState = State with
                {
                    MovieState = State.MovieState with
                    {
                        Similar = State.MovieState.Similar with
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

        private async Task SetSeriesSimilarPage(int page)
        {
            try
            {
                if (State.SeriesState.Similar.Page.Equals(page))
                {
                    return;
                }

                var currentState = State with
                {
                    SeriesState = State.SeriesState with
                    {
                        Similar = State.SeriesState.Similar with
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

    }
}
