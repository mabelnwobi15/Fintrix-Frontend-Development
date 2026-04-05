using Microsoft.AspNetCore.Mvc;

namespace SmartInvoice.MVC.Controllers
{
    public class Settings : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
