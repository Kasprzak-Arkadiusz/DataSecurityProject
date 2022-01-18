using ApiLibrary.Validators.DetailedValidators;
using CommonLibrary.Common;
using CommonLibrary.Dto;
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
            if (!LoginValidator.Validate(loginDto))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return BadRequest("Invalid login attempt.");
            }

            try
            {
                var jwtKey = Environment.GetEnvironmentVariable("JWTKEY");
                var loginResponse = await AuthenticationService.LoginAsync(loginDto, jwtKey);

                if (loginResponse.Result.Succeeded)
                    return Ok(loginResponse);

                await Task.Delay(TimeSpan.FromSeconds(3)); // Slow down hacking attempt
                return BadRequest(loginResponse);
            }
            catch (Exception)
            {
                var loginResponse = new LoginResponse { Result = Result.Failure(new[] { "Invalid login attempt." }) };
                return BadRequest(loginResponse);
            }
        }
    }
}