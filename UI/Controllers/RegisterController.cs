using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    public class RegisterController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var client = HttpClientFactory.CreateClient("api");
            var actionPath = GetControllerName();

            var json = JsonConvert.SerializeObject(registerViewModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.PostAsync(actionPath, content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewData["Error"] = await httpResponseMessage.Content.ReadAsStringAsync();

            return View(registerViewModel);
        }
    }
}