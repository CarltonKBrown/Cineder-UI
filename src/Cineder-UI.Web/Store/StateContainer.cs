using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Interfaces.Store;
using Cineder_UI.Web.Models.Common;
using Microsoft.Extensions.Options;

namespace Cineder_UI.Web.Store
{
    public class StateContainer : IStateContainer
    {
        private readonly IBrowserStorageService _browserStorage;
        private readonly SessionOptions _sessionOptions;
        public StateContainer(IBrowserStorageService browserStorage, IOptionsSnapshot<SessionOptions> sessionOptions)
        {
            _browserStorage = browserStorage;
            _sessionOptions = sessionOptions.Value;
        }

        public AppState State { get; private set; } = new();

        public event Action OnStateChanged = () => { };

        public void NotifyStateChanged() => OnStateChanged?.Invoke();

        private async Task<AppState> GetAppStateAsync()
        {
            var storeName = _sessionOptions.StoreName;

            return await _browserStorage.GetSessionStorageItemAsync<AppState>(storeName);
        }

        private async Task CommitAsync(AppState state)
        {
            var storeName = _sessionOptions.StoreName;

            State = state;

            await _browserStorage.SetSessionStorageItemAsync(storeName, State);

            NotifyStateChanged();
        }

        public async Task RefreshStateAsync()
        {
            var currentState = await GetAppStateAsync() ?? new();

            await CommitAsync(currentState);
        }

        public async Task RestartSessionAsync()
        {
            await CommitAsync(new());
        }
    }
}
