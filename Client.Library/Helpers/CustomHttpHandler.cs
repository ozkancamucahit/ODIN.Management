using Base.Library.DTOs;
using Client.Library.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Library.Helpers
{
    /// <summary>
    /// custom handler to add token and call refresh token
    /// </summary>
    public sealed class CustomHttpHandler : DelegatingHandler
    {
        private readonly GetHttpClient getHttpClient;
        private readonly LocalStorageService localStorageService;
        private readonly IUserAccount userAccount;

        #region CTOR
        public CustomHttpHandler(GetHttpClient getHttpClient,
                                 LocalStorageService localStorageService,
                                 IUserAccount userAccount)
        {
            this.getHttpClient = getHttpClient;
            this.localStorageService = localStorageService;
            this.userAccount = userAccount;
        }
        #endregion


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginURL = request.RequestUri!.AbsoluteUri.ToLower().Contains("login");
            bool registerURL = request.RequestUri!.AbsoluteUri.ToLower().Contains("register");
            bool refreshTokenURL = request.RequestUri!.AbsoluteUri.ToLower().Contains("refresh");

            if(loginURL || registerURL || refreshTokenURL)
            {
                //dont append to header
                return await base.SendAsync(request, cancellationToken);
            }

            var result = await base.SendAsync(request, cancellationToken);

            if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var stringToken = await localStorageService.GetToken();

                if (String.IsNullOrWhiteSpace(stringToken))
                {
                    return result;
                }

                // token expired
                string token = String.Empty;

                try
                {
                    token = request.Headers.Authorization!.Parameter!;

                }
                catch (Exception ex)
                {
                }

                var deserializedToken = Serializations.DeserializeJSONString<UserSessionDTO>(stringToken);

                if (deserializedToken is null)
                    return result;

                if (String.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                var newJwtToken = await GetRefreshToken(deserializedToken!.RefreshToken!);

                if (String.IsNullOrWhiteSpace(newJwtToken))
                {
                    return result;
                }

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }
            else
                return result;

        }

        private async Task<string> GetRefreshToken(string refreshToken)
        {

            var result = await userAccount.RefreshTokenAsync(new RefreshToken { Token = refreshToken});

            string serializedToken = 
                Serializations.SerializeObj(new UserSessionDTO { Token= result.Token, RefreshToken= result.RefreshToken});

            await localStorageService.SetToken(serializedToken);

            return result.Token;
        }
    }
}
