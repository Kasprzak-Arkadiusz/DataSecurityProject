using Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        // All services will be added here
        private IAuthenticationService _authenticationService;

        protected IAuthenticationService AuthenticationService =>
            _authenticationService ??= HttpContext.RequestServices.GetService<IAuthenticationService>();
    }
}
