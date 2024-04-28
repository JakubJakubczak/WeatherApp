
using Microsoft.AspNetCore.Mvc;
using WeatherApplication.Services;
using WeatherApplication.Views.Shared;

namespace WeatherApplication.Controllers
{
    public class CheckWeatherController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        /*public async Task<IActionResult> Index()
        {
            double latitude = 51.11908339691092;
            double longitude = 17.126694053556225;
            var weather = await ApiService.GetWeather(latitude, longitude);

            // Konwersja temperatury z Fahrenheitów na stopnie Celsiusza
            foreach (var forecast in weather.list)
            {
                forecast.main.temp = Math.Round(ConvertKelvinToCelsius(forecast.main.temp), 1);
                forecast.main.temp_min = Math.Round(ConvertKelvinToCelsius(forecast.main.temp_min), 1);
                forecast.main.temp_max = Math.Round(ConvertKelvinToCelsius(forecast.main.temp_max), 1);
            }

            return View(weather);
        }

        private double ConvertKelvinToCelsius(double Kelvin)
        {
            return (Kelvin - 273.15);
        }
        */

        private double ConvertKelvinToCelsius(double Kelvin)
        {
            return (Kelvin - 273.15);
        }

        [HttpPost]
        public async Task<IActionResult> ShowWeather(string city)
        {
            var weather = await ApiService.GetWeatherByCity(city);

            foreach (var forecast in weather.list)
            {
                forecast.main.temp = Math.Round(ConvertKelvinToCelsius(forecast.main.temp), 1);
                forecast.main.temp_min = Math.Round(ConvertKelvinToCelsius(forecast.main.temp_min), 1);
                forecast.main.temp_max = Math.Round(ConvertKelvinToCelsius(forecast.main.temp_max), 1);
            }

            return PartialView("_WeatherDetailsPartial", weather);
        }

    }
}