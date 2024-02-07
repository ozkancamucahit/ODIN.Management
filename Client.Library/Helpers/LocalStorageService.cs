using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Library.Helpers
{
    public sealed class LocalStorageService
    {
        #region FIELDS
        private readonly ILocalStorageService localStorageService;
        private const string StorageKey = "authentication-token";
        #endregion

        #region CTOR
        public LocalStorageService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }
        #endregion

        public async Task<string> GetToken()
        {
            return await localStorageService.GetItemAsStringAsync(StorageKey);
        }

        public async Task SetToken(string item)
        {
            await localStorageService.SetItemAsStringAsync(StorageKey, item);
        }

        public async Task RemoveToken()
        {
            await localStorageService.RemoveItemAsync(StorageKey);
        }

    }
}
