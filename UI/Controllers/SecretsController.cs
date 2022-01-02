using CommonLibrary.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UI.Models;
using UI.Utils;

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

            var uriBuilder = new UriBuilder(client.BaseAddress + actionPath);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            var userName = ((ClaimsIdentity)User.Identity).GetCurrentUserName();
            query["userName"] = userName;

            uriBuilder.Query = query.ToString() ?? string.Empty;
            var url = uriBuilder.ToString();

            var httpResponseMessage = await client.GetAsync(url);

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await httpResponseMessage.Content.ReadFromJsonAsync<List<string>>();

            var viewModel = new List<SecretViewModel>();
            var id = 0;
            result?.ForEach(s => viewModel.Add(new SecretViewModel(s, id++)));

            ViewData["Token"] = HttpContext.Session.GetString("Token");
            ViewData["UserName"] = userName;

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSecretViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var client = HttpClientFactory.CreateClient("api");
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var actionPath = GetControllerName();

            var secretDto = new SecretDto
            {
                UserName =  ((ClaimsIdentity)User.Identity).GetCurrentUserName(),
                Password = viewModel.Password,
                ServiceName = viewModel.ServiceName
            };

            var json = JsonConvert.SerializeObject(secretDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.PostAsync(actionPath, content);

            if (httpResponseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewData["Error"] = "Unexpected error";
            return View(viewModel);
        }
    }
}