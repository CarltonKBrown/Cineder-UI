using Blazor.SubtleCrypto;
using Blazored.SessionStorage;
using Cineder_UI.Web.Interfaces.Services;
using PreventR;
using System.Text.Json;

namespace Cineder_UI.Web.Services
{
    public class BrowserStorageService : IBrowserStorageService
    {
        private readonly ISessionStorageService _storageService;
        private ICryptoService _cryptoService;

        public BrowserStorageService(ISessionStorageService storageSesion, ICryptoService cryptoService)
        {
            _storageService = storageSesion.Prevent(nameof(storageSesion)).Null().Value;

            _cryptoService = cryptoService.Prevent(nameof(cryptoService)).Null().Value;
        }

        public async Task<T> GetSessionStorageItemAsync<T>(string uniqueKey)
        {
            var encryptedString = await _storageService.GetItemAsync<string>(uniqueKey);

            var jsonString = await _cryptoService.DecryptAsync(encryptedString);

            var data = JsonSerializer.Deserialize<T>(jsonString);

            return data ?? default!;
        }

        public async Task SetSessionStorageItemAsync<T>(string uniqueKey, T item)
        {
            var jsonString = JsonSerializer.Serialize(item);

            var encryptedData = await _cryptoService.EncryptAsync(jsonString);

            await _storageService.SetItemAsync(uniqueKey, encryptedData.Value);
        }
    }
}
