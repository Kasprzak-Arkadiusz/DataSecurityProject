using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string GetControllerName()
        {
            return ControllerContext.RouteData.Values["controller"]?.ToString();
        }

        private IHttpClientFactory _httpClientFactory;

        protected IHttpClientFactory HttpClientFactory =>
            _httpClientFactory ??= HttpContext.RequestServices.GetService<IHttpClientFactory>();
    }
}