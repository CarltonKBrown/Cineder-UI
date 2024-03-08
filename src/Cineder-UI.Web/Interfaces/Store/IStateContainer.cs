using Cineder_UI.Web.Store;

namespace Cineder_UI.Web.Interfaces.Store
{
    public interface IStateContainer
    {
        AppState State { get; }

        event Action OnStateChanged;

        void NotifyStateChanged();

        Task RefreshStateAsync();

        Task RestartSessionAsync();
    }
}
