
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApplication.Models;
using WeatherApplication.Services;

namespace WeatherApplication.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private double ConvertKelvinToCelsius(double Kelvin)
        {
            return (Kelvin - 273.15);
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectedDatesAndCity(string city, string startDate, string endDate)
        {
            Debug.WriteLine("City: " + city);
            Debug.WriteLine("Start Date: " + startDate);
            Debug.WriteLine("End Date: " + endDate);

            
            var weatherData = await ApiService.GetWeatherByCity(city);

            
            //List<int> selectedIndexes = new List<int>();
            List<double> selectedTemperatures = new List<double>();
            List<string> selectedDescriptions = new List<string>();

            string formattedStartDate = $"{startDate} 12:00:00"; 
            string formattedEndDate = $"{endDate} 12:00:00"; 

            
            for (int i = 0; i < weatherData.list.Count; i++)
            {
                
                string dateWithoutTime = weatherData.list[i].dt_txt.Split(' ')[0];

                
                if (string.Compare(dateWithoutTime, startDate) >= 0 && string.Compare(dateWithoutTime, endDate) <= 0)
                {
                    selectedTemperatures.Add(Math.Round(ConvertKelvinToCelsius(weatherData.list[i].main.temp),1));
                    selectedDescriptions.Add(weatherData.list[i].weather[0].description);
                }
            }

            Debug.WriteLine("Selected temp:");
            foreach (var temp in selectedTemperatures)
            {
                Debug.WriteLine(temp);
            }

            Debug.WriteLine("Selected desc:");
            foreach (var desc in selectedDescriptions)
            {
                Debug.WriteLine(desc);
            }

            return Ok(new { Temperatures = selectedTemperatures, Descriptions = selectedDescriptions });
        }


    }


}

