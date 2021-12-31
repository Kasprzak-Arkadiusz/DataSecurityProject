using CommonLibrary.Dto;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UI.Models;

namespace UI.Controllers
{
    public class PasswordResetController : BaseController
    {
        private readonly IDataProtector _protector;

        public PasswordResetController(IConfiguration configuration, IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(configuration["PurposeForPasswordResetControllerProtector"]);
        }

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
                return RedirectToAction("SendEmail");
            }

            var token = await httpResponseMessage.Content.ReadAsStringAsync();
            return RedirectToAction("SendEmail", new { token = _protector.Protect(token) });
        }

        public IActionResult SendEmail(string token)
        {
            var callbackUrl = Url.Action("ChangePassword",
                values: new { token },
                protocol: Request.Scheme,
                controller: "PasswordReset");

            ViewData["Url"] = callbackUrl;
            return View();
        }

        public IActionResult ChangePassword(string token)
        {
            var viewModel = new PasswordChangeViewModel
            {
                Code = _protector.Unprotect(token)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var client = HttpClientFactory.CreateClient("api");
            var actionPath = GetControllerName();

            var uriBuilder = new UriBuilder(client.BaseAddress + actionPath + "/Validate");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["emailAddress"] = viewModel.Email;
            query["token"] = viewModel.Code;
            uriBuilder.Query = query.ToString() ?? string.Empty;
            var url = uriBuilder.ToString();

            var httpResponseMessage = await client.GetAsync(url);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                ViewData["Error"] = "Invalid reset token";
                return View();
            }

            var updatePasswordDto = new UpdatePasswordDto
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
            };

            var json = JsonConvert.SerializeObject(updatePasswordDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var postResponseMessage = await client.PostAsync(actionPath, content);

            return postResponseMessage.IsSuccessStatusCode ? View("Success") : View();
        }
    }
}