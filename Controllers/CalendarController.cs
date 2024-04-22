
using Microsoft.AspNetCore.Mvc;

namespace WeatherApplication.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}