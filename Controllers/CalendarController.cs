
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApplication.Models;

namespace WeatherApplication.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetSelectedDatesAndCity(string city, string[] selectedDates)
        {

            Debug.WriteLine("City: " + city);
            foreach (var date in selectedDates)
            {
                Debug.WriteLine("Selected Date: " + date);
            }


            return Ok(); 
        }

    }


}

