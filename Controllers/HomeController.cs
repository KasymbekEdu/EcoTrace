using Microsoft.AspNetCore.Mvc;

namespace EcoSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Step1", "Survey");
        }
    }
}
