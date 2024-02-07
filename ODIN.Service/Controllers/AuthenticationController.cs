using Base.Library.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Library.Repositories.Contracts;

namespace ODIN.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthenticationController : ControllerBase
    {
        private readonly IUserAccount userAccount;

        #region CTOR
        public AuthenticationController(IUserAccount userAccount)
        {
            this.userAccount = userAccount;
        }
        #endregion

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(RegisterDTO user)
        {
            if(ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest("user is null");
                }

                var result = await userAccount.CreateAsync(user);
                return Ok(result);  
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> SignInAsync(LoginDTO user)
        {

            if (user is null) 
                return BadRequest("model is empty");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var result = await userAccount.SignInAsync(user);
            return Ok(result);


        }



    }
}
