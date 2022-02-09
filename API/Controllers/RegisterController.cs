using ApiLibrary.Validators.DetailedValidators;
using CommonLibrary.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class RegisterController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            if (!RegisterValidator.Validate(userDto))
            {
                
                return BadRequest("Invalid attempt. Try again.");
            }

            var result = await AuthenticationService.RegisterAsync(userDto);

            if (result.Succeeded)
                return Ok();

            return BadRequest("Invalid attempt. Try again.");
        }
    }
}