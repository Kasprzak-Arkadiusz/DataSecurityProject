using CommonLibrary.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
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

            var result = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Token", result?.Token);

                var claims = result?.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();

                var user = new GenericPrincipal(new ClaimsIdentity(claims?.First(c => c.Type == "Name").Value), new[] { "User" });
                HttpContext.User = user;

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = false,
                    IssuedUtc = DateTimeOffset.Now
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }

            loginViewModel.Error = result?.Result.Errors[0];

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString("Token", string.Empty);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult PasswordReset()
        {
            return RedirectToAction("Index", "PasswordReset");
        }
    }
}