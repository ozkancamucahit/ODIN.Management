﻿using Base.Library.DTOs;
using Base.Library.Entities;
using Base.Library.Entities.Identity;
using Base.Library.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Library.Data;
using Server.Library.Helpers;
using Server.Library.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Library.Repositories.Implementations
{
    public sealed class UserAccountRepository : IUserAccount
    {
        private readonly IOptions<JwtSection> config;
        private readonly AppDbContext appDbContext;

        #region CTOR
        public UserAccountRepository(IOptions<JwtSection> config, AppDbContext appDbContext)
        {
            this.config = config;
            this.appDbContext = appDbContext;
        }
        #endregion

        public async Task<GeneralResponse> CreateAsync(RegisterDTO user)
        {
            if (user is null)
            {
                return new GeneralResponse(false, "Model is empty");
            }

            var chkUser = await FindUserByEmail(user.Email.Trim());

            if (chkUser is not null)
                return new GeneralResponse(false, "User registered already");

            var applicationUser = await AddToDatabase(new ApplicationUser
            {
                Name = user.FullName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });

            //chack create and add role
            var chkAdminRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.Admin));

            if (chkAdminRole is null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole { Name = Constants.Admin });
                await AddToDatabase(new UserRole { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Account Created");
            }

            var chkUserRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(u => u.Name!.Equals(Constants.User));
            SystemRole response = new();
            if (chkUserRole is null)
            {
                response = await AddToDatabase(new SystemRole { Name = Constants.User });
                await AddToDatabase(new UserRole { RoleId = response.Id, UserId = applicationUser.Id });
            }
            else
            {
                await AddToDatabase(new UserRole { RoleId = chkUserRole.Id, UserId = applicationUser.Id });
            }

            return new GeneralResponse(true, "Account Created");

        }

        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = appDbContext.Add(model!);
            await appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        private async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await appDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Email!.ToLower()!.Equals(email!.ToLower()));
        }

        public async Task<LoginResponse> SignInAsync(LoginDTO user)
        {
            if (user is null) return new LoginResponse(false, "User not found");

            var applicationUser = await FindUserByEmail(user.Email!);

            if (applicationUser is null) return new LoginResponse(false, "User not found");

            //verify password
            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
                return new LoginResponse(false, "email/password invalid");

            var getUserRole = await appDbContext.UserRoles.FirstOrDefaultAsync(r => r.UserId == applicationUser.Id);

            if (getUserRole is null) return new LoginResponse(false, "user role not found");

            var getRoleName = await appDbContext.SystemRoles.FirstOrDefaultAsync(r => r.Id == getUserRole.RoleId);

            if (getUserRole is null) return new LoginResponse(false, "user role not found");

            string jwtToken = GenerateToken(applicationUser, getRoleName!.Name!);
            string refreshToken = GenerateRefreshToken();
            return new LoginResponse(true, "login success", jwtToken, refreshToken);


        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private string GenerateToken(ApplicationUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}