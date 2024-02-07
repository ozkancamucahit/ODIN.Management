using Base.Library.DTOs;
using Base.Library.Responses;
using Client.Library.Helpers;
using Client.Library.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Library.Services.Implementations
{
    internal sealed class UserAccountService : IUserAccount
    {
        #region FIELDS
        private readonly GetHttpClient getHttpClient;
        private const string AuthUrl = "api/Authentication";
        #endregion

        #region CTOR
        public UserAccountService(GetHttpClient getHttpClient)
        {
            this.getHttpClient = getHttpClient;
        }
        #endregion


        public async Task<GeneralResponse> CreateAsync(RegisterDTO user)
        {
            using var httpClient = getHttpClient.GetPublicHttpClient();

            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/Register", user);

            if(!result.IsSuccessStatusCode)
                return new GeneralResponse(false, "=> Error Occured :" + result.ReasonPhrase);

            return await result.Content.ReadFromJsonAsync<GeneralResponse>();


        }
        public async Task<LoginResponse> SignInAsync(LoginDTO user)
        {
            using var httpclient = getHttpClient.GetPublicHttpClient();
            var result = await httpclient.PostAsJsonAsync(AuthUrl + "/Login", user);

            if (!result.IsSuccessStatusCode)
                return new LoginResponse(false, "=> Error Occured :" + result.ReasonPhrase);

            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }
        public Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<WeatherForecast[]> GetWeatherForecasts()
        {
            using var httpClient = await getHttpClient.GetPrivateHttpClientAsync();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast");
            return result;
        }


    }
}
