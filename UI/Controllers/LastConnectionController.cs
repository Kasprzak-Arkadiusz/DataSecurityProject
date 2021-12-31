using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using UI.Models;
using UI.Utils;

namespace UI.Controllers
{
    public class LastConnectionController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            // Call to Api to retrieve last connections
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
                return RedirectToAction("Index", "Login");

            if (!httpResponseMessage.IsSuccessStatusCode)
                return View(new List<LastConnectionViewModel>());

            var result = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<LastConnectionViewModel>>();

            return View(result);
        }
    }
}