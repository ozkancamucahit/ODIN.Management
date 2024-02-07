using Base.Library.DTOs;
using Base.Library.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Library.Repositories.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(RegisterDTO user);
        Task<LoginResponse> SignInAsync(LoginDTO user);
    }
}
