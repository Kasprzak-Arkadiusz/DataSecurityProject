using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class PasswordResetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
