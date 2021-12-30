using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;
using UI.Models;

namespace UI.Controllers
{
    public class PasswordResetController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PasswordResetViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var client = HttpClientFactory.CreateClient("api");
            var actionPath = GetControllerName();

            var uriBuilder = new UriBuilder(client.BaseAddress + actionPath);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["emailAddress"] = viewModel.EmailAddress;
            uriBuilder.Query = query.ToString() ?? string.Empty;
            var url = uriBuilder.ToString();

            var httpResponseMessage = await client.GetAsync(url);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("EmailSend");
            }

            var token = await httpResponseMessage.Content.ReadAsStringAsync();
            return RedirectToAction("EmailSend", new { token });
        }

        public IActionResult EmailSend(string token)
        {
            // Generate link to page with resetting password
            ViewData["Token"] = token;
            return View();
        }
    }
}