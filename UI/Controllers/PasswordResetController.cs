using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommonLibrary.Dto;
using UI.Models;

namespace UI.Controllers
{
    public class PasswordResetController : BaseController
    {
        private readonly IDataProtector _protector;

        public PasswordResetController(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("PasswordReset");
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
            return RedirectToAction("SendEmail", new
            {
                token = _protector.Protect(token), 
                email = _protector.Protect(viewModel.EmailAddress)
            });
        }

        public IActionResult SendEmail(string token, string email)
        {
            var callbackUrl = Url.Action("ChangePassword",
                values: new { token, email },
                protocol: Request.Scheme,
                controller: "PasswordReset");

            ViewData["Url"] = callbackUrl;
            return View();
        }

        public IActionResult ChangePassword(string token, string email)
        {
            var unprotectedToken = _protector.Unprotect(token);
            var unprotectedEmail = _protector.Unprotect(email);

            var viewModel = new PasswordChangeViewModel
            {
                Code = unprotectedToken,
                Email = unprotectedEmail
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel viewModel)
        {
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