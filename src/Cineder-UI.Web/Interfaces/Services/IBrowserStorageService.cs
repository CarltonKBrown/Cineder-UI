namespace Cineder_UI.Web.Interfaces.Services
{
    public interface IBrowserStorageService
    {
        Task<T> GetSessionStorageItemAsync<T>(string key);
        Task SetSessionStorageItemAsync<T>(string key, T item);
    }
}
