using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class SecretsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}