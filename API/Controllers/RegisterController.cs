using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CommonLibrary.Dto;

namespace API.Controllers
{
    public class RegisterController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
        {
            var result = await AuthenticationService.RegisterAsync(userDto);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid attempt. Try again.");
            }

            return Ok();
        }
    }
}