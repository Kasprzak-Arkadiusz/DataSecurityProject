using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class SecretsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
