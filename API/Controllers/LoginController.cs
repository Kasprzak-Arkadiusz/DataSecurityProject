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
            var result = Result.Failure(new[] { "Unexpected error" });

            try
            {
                result = await AuthenticationService.LoginAsync(loginDto);
            }
            catch (Exception)
            {
                return BadRequest(result.Errors[0]);
            }

            if (!result.Succeeded)
            {
                // await Task.Delay(3000); // Slow down hacking attempt
                return BadRequest(result.Errors[0]);
            }

            return Ok();
        }
    }
}