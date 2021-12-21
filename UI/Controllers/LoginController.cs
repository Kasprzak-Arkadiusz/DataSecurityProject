using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UI.Models;

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

            // Temporary solution
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return View();
            }

            loginViewModel.Error = await httpResponseMessage.Content.ReadAsStringAsync();

            return View(loginViewModel);
        }

        public IActionResult PasswordReset()
        {
            return RedirectToAction("Index", "PasswordReset");
        }
    }
}