using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UI.Models;
using UI.Utils;

namespace UI.Controllers
{
    public class LoginController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var client = HttpClientFactory.CreateClient("api");
            var actionPath = GetControllerName();

            var json = JsonConvert.SerializeObject(loginViewModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.PostAsync(actionPath, content);

            var result = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Token", result?.Token);
                return RedirectToAction("Index", "Home");
            }

            loginViewModel.Error = result?.Result.Errors[0];

            return View(loginViewModel);
        }

        public IActionResult PasswordReset()
        {
            return RedirectToAction("Index", "PasswordReset");
        }
    }
}