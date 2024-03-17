using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
