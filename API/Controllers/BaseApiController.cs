using Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        // All services will be added here
        private IAuthenticationService _authenticationService;
        private IConfiguration _configuration;

        protected IAuthenticationService AuthenticationService =>
            _authenticationService ??= HttpContext.RequestServices.GetService<IAuthenticationService>();

        protected IConfiguration Configuration =>
            _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();
    }
}