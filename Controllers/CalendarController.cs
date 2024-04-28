
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApplication.Models;
using WeatherApplication.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

       // [HttpPost]
        //public async Task<IActionResult> ShowPlan(string city)
        //{
        //
        //    return PartialView("_PlanerUbranPartial");
        //}

        [HttpPost]
        public async Task<IActionResult> GetSelectedDatesAndCity(string city, string startDate, string endDate)
        {
            Debug.WriteLine("City: " + city);
            Debug.WriteLine("Start Date: " + startDate);
            Debug.WriteLine("End Date: " + endDate);

            var weatherData = await ApiService.GetWeatherByCity(city);

            // Tworzymy słownik, który będzie trzymał listy temperatur i opisów pogody dla poszczególnych dat
            Dictionary<string, List<double>> temperaturesByDate = new Dictionary<string, List<double>>();
            Dictionary<string, List<string>> descriptionsByDate = new Dictionary<string, List<string>>();

            // Wypełniamy słowniki danymi z pobranej pogody
            foreach (var forecast in weatherData.list)
            {
                string dateWithoutTime = forecast.dt_txt.Split(' ')[0];

                // Sprawdzamy, czy data znajduje się w zakresie między startDate i endDate
                if (string.Compare(dateWithoutTime, startDate) >= 0 && string.Compare(dateWithoutTime, endDate) <= 0)
                {
                    // Sprawdzamy, czy słownik zawiera już klucz odpowiadający aktualnej dacie
                    if (!temperaturesByDate.ContainsKey(dateWithoutTime))
                    {
                        // Jeśli nie, tworzymy nowe listy dla tej daty
                        temperaturesByDate[dateWithoutTime] = new List<double>();
                        descriptionsByDate[dateWithoutTime] = new List<string>();
                    }

                    // Dodajemy temperaturę i opis pogody do listy dla aktualnej daty
                    temperaturesByDate[dateWithoutTime].Add(Math.Round(ConvertKelvinToCelsius(forecast.main.temp), 1));
                    descriptionsByDate[dateWithoutTime].Add(forecast.weather[0].description);
                }
            }

            // Wywołujemy funkcję ChooseClothes z danymi pogodowymi
            List<string> clothes = ChooseClothes(temperaturesByDate);

            // Wyświetlamy dane dla każdej daty
            foreach (var date in temperaturesByDate.Keys)
            {
                Debug.WriteLine("Date: " + date);
                Debug.WriteLine("Temperatures: " + string.Join(", ", temperaturesByDate[date]));
                Debug.WriteLine("Descriptions: " + string.Join(", ", descriptionsByDate[date]));
                Debug.WriteLine("Clothes: " + string.Join(", ", clothes)); 
            }

            return Ok(new { TemperaturesByDate = temperaturesByDate, DescriptionsByDate = descriptionsByDate, ClothesByDate = clothes });

        }

            private List<string> ChooseClothes(Dictionary<string, List<double>> temperaturesByDate)
             {
            List<string> clothes = new List<string>();
            bool isJacketNeeded = false;
            bool isSweatshirtNeeded = false;

            foreach (var date in temperaturesByDate.Keys)
            {
                bool isShortsNeeded = true;

                foreach (var temperature in temperaturesByDate[date])
                {
                    if (temperature < 20)
                    {
                        isShortsNeeded = false;
                    }

                    if (temperature < 10)
                    {
                        isJacketNeeded = true;
                    }

                    if (temperature < 15)
                    {
                        isSweatshirtNeeded = true;
                    }
                }

                clothes.Add("T-shirt");
                clothes.Add("Bielizna");

                if (isShortsNeeded)
                {
                    clothes.Add("Krotkie spodnie");
                }
            }

            if (isJacketNeeded)
            {
                clothes.Add("Kortka");
            }

            if (isSweatshirtNeeded)
            {
                clothes.Add("Bluza");
            }

            return clothes;
        }



    }


}

