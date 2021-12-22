using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class SecretsController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var client = HttpClientFactory.CreateClient("api");
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var actionPath = GetControllerName();

            var httpResponseMessage = await client.GetAsync(actionPath);

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}
