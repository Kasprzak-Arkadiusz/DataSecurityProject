using CommonLibrary.Common;
using CommonLibrary.Dto;
using IpInfo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UI.Models;
using Wangkanai.Detection.Services;

namespace UI.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IDetectionService _detectionService;

        public LoginController(IDetectionService detectionService)
        {
            _detectionService = detectionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                await LoginUserAsync(result);

                using var ipInfoClient = new HttpClient();
                var ipInfoToken = Environment.GetEnvironmentVariable("IpInfoToken");
                var api = new IpInfoApi(ipInfoToken, ipInfoClient);
                var response = await api.GetCurrentInformationAsync();

                var lastConnectionViewModel = new LastConnectionDto
                {
                    DeviceType = _detectionService.Device.Type.ToString(),
                    BrowserName = _detectionService.Browser.Name.ToString(),
                    PlatformName = _detectionService.Platform.Name.ToString(),
                    City = response.City,
                    Region = response.Region,
                    Country = response.Country,
                    ConnectionTime = DateTime.Now,
                    UserName = loginViewModel.UserName
                };
                const string createActionPath = "LastConnection";
                var token = HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var createJson = JsonConvert.SerializeObject(lastConnectionViewModel);
                var createContent = new StringContent(createJson, Encoding.UTF8, "Application/json");
                await client.PostAsync(createActionPath, createContent);

                return RedirectToAction("Index", "Home");
            }

            if (result != null)
                ViewData["Error"] = string.Join("\n", result.Result.Errors);

            return View(loginViewModel);
        }

        private async Task LoginUserAsync(LoginResponse result)
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
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                IsPersistent = false,
                IssuedUtc = DateTimeOffset.Now
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
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