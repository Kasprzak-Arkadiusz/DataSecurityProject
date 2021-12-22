using Application.Common;
using Application.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class LoginController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = new LoginResponse(Result.Failure(new[] { "Unexpected error" }));

            try
            {
                loginResult = await AuthenticationService.LoginAsync(loginDto, Configuration["JWT:Key"]);
            }
            catch (Exception)
            {
                return BadRequest(loginResult);
            }

            if (loginResult.Result.Succeeded)
                return Ok(loginResult);

            await Task.Delay(3000); // Slow down hacking attempt
            return BadRequest(loginResult);
        }
    }
}