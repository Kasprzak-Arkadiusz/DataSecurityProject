using Application.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class RegisterController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await AuthenticationService.RegisterAsync(userDto);

            if (!result.Succeeded)
            {
                // Here would be normally logging with message received from result
                return BadRequest("Invalid attempt. Try again.");
            }

            return Ok();
        }
    }
}