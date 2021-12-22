using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            // Calling API to logout (terminate JWT Token etc.)

            return RedirectToAction("Index", "Home");
        }
    }
}
