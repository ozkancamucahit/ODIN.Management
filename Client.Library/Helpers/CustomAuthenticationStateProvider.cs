using Base.Library.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Client.Library.Helpers
{
    public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        #region FIELDS
        private readonly LocalStorageService localStorageService;
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        #endregion


        #region CTOR
        public CustomAuthenticationStateProvider(LocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }
        #endregion


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (String.IsNullOrWhiteSpace(stringToken))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var deserializeToken = Serializations.DeserializeJSONString<UserSessionDTO>(stringToken);

            if (deserializeToken == null)
                return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(deserializeToken.Token!);

            if (getUserClaims == null)
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));


        }

        public async Task UpdateAuthenticationState(UserSessionDTO userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            if(userSession.Token != null ||userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.SerializeObj(userSession);
                await localStorageService.SetToken(serializeSession);

                var getUserClaims = DecryptToken(userSession.Token);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }

            else
            {
                await localStorageService.RemoveToken();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));


        }

        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email is null)
                return new ClaimsPrincipal();

            var claimsAdd = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, claims.Id),
                    new(ClaimTypes.Name, claims.Name),
                    new(ClaimTypes.Email, claims.Email),
                    new(ClaimTypes.Role, claims.Role)
                };

            return new ClaimsPrincipal(
                new ClaimsIdentity(claimsAdd, "JwtAuth"));


        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (String.IsNullOrWhiteSpace(jwtToken))
                return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userId = token.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault( c => c.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault( c => c.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault( c => c.Type == ClaimTypes.Role);

            return new CustomUserClaims(userId!.Value, name!.Value, email!.Value, role!.Value);

            throw new NotImplementedException();
        }
    }
}
