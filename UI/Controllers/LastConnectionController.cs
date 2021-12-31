using Microsoft.AspNetCore.Mvc;
using Wangkanai.Detection.Services;

namespace UI.Controllers
{
    public class LastConnectionController : BaseController
    {
        private readonly IDetectionService _detectionService;
        public LastConnectionController(IDetectionService detectionService)
        {
            _detectionService = detectionService;
        }

        public IActionResult Index()
        {
            return View(_detectionService);
        }
    }
}
