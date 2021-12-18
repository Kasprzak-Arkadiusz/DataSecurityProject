using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = "Hello from webfrontend";

            var client = _httpClientFactory.CreateClient("Default");
            var response = await client.GetAsync("https://api/WeatherForecast");

            ViewData["Message"] += " and " + await response.Content.ReadAsStringAsync();
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var client = _httpClientFactory.CreateClient("Default");
            var response = await client.GetAsync("https://api/Users");
            return View(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}