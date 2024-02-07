using Base.Library.DTOs;
using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client.Library.Helpers
{
    public sealed class GetHttpClient
    {
        #region FIELDS
        private const string HeaderKey = "Authorization";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly LocalStorageService localStorageService;
        #endregion

        #region CTOR
        public GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
        {
            this.httpClientFactory = httpClientFactory;
            this.localStorageService = localStorageService;
        }
        #endregion


        public async Task<HttpClient> GetPrivateHttpClientAsync()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await localStorageService.GetToken();

            if(String.IsNullOrWhiteSpace(stringToken))
                return client;

            var deSerializeToken = Serializations.DeserializeJSONString<UserSessionDTO>(stringToken);

            if(deSerializeToken == null) 
                return client;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", deSerializeToken.Token);
            return client;
        }

        public HttpClient GetPublicHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }

    }
}
