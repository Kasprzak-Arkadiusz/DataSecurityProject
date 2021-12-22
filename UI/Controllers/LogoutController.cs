using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            

            return RedirectToAction("Index", "Home");
        }
    }
}
