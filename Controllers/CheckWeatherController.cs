
using Microsoft.AspNetCore.Mvc;
using WeatherApplication.Services;
using WeatherApplication.Views.Shared;

namespace WeatherApplication.Controllers
{
    /// <summary>
    /// Obsługa podstrony do sprawdzania pogody
    /// </summary>
    public class CheckWeatherController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Zamiana Stopni w skali Kelvina na stopnie w skali Celciusza
        /// </summary>
        /// <param name="Kelvin">Stopnie w skali Kelvina</param>
        /// <returns>Stopnie w skaali celcjusza</returns>
        private double ConvertKelvinToCelsius(double Kelvin)
        {
            return (Kelvin - 273.15);
        }


        /// <summary>
        /// Wyswietlenie pogody na nowej stronie
        /// </summary>
        /// <param name="city">Nazwa miasta dla ktorego chcemy uzyskac pogode</param>
        /// <returns>PartialView z danymi pogodowymi</returns>
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