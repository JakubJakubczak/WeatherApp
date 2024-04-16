using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApplication.Models;
using WeatherApplication.Services;

namespace WeatherApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            double latitude = 51.11908339691092;
            double longitude = 17.126694053556225;
            var weather = await ApiService.GetWeather(latitude, longitude);

            // Konwersja temperatury z Fahrenheitów na stopnie Celsiusza
            foreach (var forecast in weather.list)
            {
                forecast.main.temp = ConvertKelvinToCelsius(forecast.main.temp);
                forecast.main.temp_min = ConvertKelvinToCelsius(forecast.main.temp_min);
                forecast.main.temp_max = ConvertKelvinToCelsius(forecast.main.temp_max);
            }

            return View(weather);
        }

        private double ConvertKelvinToCelsius(double Kelvin)
        {
            return (Kelvin - 273.15);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
