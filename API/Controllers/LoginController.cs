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
            try
            {
                var loginResponse = await AuthenticationService.LoginAsync(loginDto, Configuration["JWT:Key"]);

                if (loginResponse.Result.Succeeded)
                    return Ok(loginResponse);

                await Task.Delay(3000); // Slow down hacking attempt
                return BadRequest(loginResponse);
            }
            catch (Exception)
            {
                return BadRequest("Unexpected error");
            }
        }
    }
}