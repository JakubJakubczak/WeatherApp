
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApplication.Data;
using WeatherApplication.Models;
using WeatherApplication.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApplication.Controllers
{
        

    /// <summary>
    /// Obsługa podstrony do planowania wycieczek
    /// </summary>
    public class CalendarController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }
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
        /// Ustlenie listy pogody i listy ubran
        /// </summary>
        /// <param name="city">Miasto potrzebna do ustalenia pogody przez api</param>
        /// <param name="startDate">Data poczatku wycieczki</param>
        /// <param name="endDate">Data konca wycieczki</param>
        /// <returns>4 Listy: temperatur, opisow pogody, ubran które sa potrzebne, i ubran ktore posiada uzytkownik </returns>
        [HttpPost]
        public async Task<IActionResult> GetSelectedDatesAndCity(string city, string startDate, string endDate)
        {
            Debug.WriteLine("City: " + city);
            Debug.WriteLine("Start Date: " + startDate);
            Debug.WriteLine("End Date: " + endDate);

            var weatherData = await ApiService.GetWeatherByCity(city);

            //Stowrzenie slownika zawierajacego listy temperatur i opsiow pogody dla poszczegolnych dat
            Dictionary<string, List<double>> temperaturesByDate = new Dictionary<string, List<double>>();
            Dictionary<string, List<string>> descriptionsByDate = new Dictionary<string, List<string>>();


            foreach (var forecast in weatherData.list)
            {
                string dateWithoutTime = forecast.dt_txt.Split(' ')[0];

                //Sprawdzenie, czy data znajduje sie w zakresie [startDate:endDate] 
                if (string.Compare(dateWithoutTime, startDate) >= 0 && string.Compare(dateWithoutTime, endDate) <= 0)
                {
                    //Sprawdzenie, czy slownik zawiera klucz odpowidajacy aktualnej dacie
                    if (!temperaturesByDate.ContainsKey(dateWithoutTime))
                    {
                        // Jeśli nie, tworzenie nowej listy dla tej daty
                        temperaturesByDate[dateWithoutTime] = new List<double>();
                        descriptionsByDate[dateWithoutTime] = new List<string>();
                    }

                    // Dodanie temperatury i opisu pogody do listy dla aktualnej daty
                    temperaturesByDate[dateWithoutTime].Add(Math.Round(ConvertKelvinToCelsius(forecast.main.temp), 1));
                    descriptionsByDate[dateWithoutTime].Add(forecast.weather[0].description);
                }
            }

            // Wywolanie funkcji ChooseClothes z danymi pogodowymi
            List<string> clothes = ChooseClothes(temperaturesByDate);
            Tuple<List<string>, List<string>> ClotheToTakeAndToBuy = ClothesToTake(clothes);

            List<string> clothesToTake = ClotheToTakeAndToBuy.Item1;
            List<string> clothesToBuy = ClotheToTakeAndToBuy.Item2;

            // Wyświetlenie danych w konoli
            //Jesli aplikacja nie dziala prawidlowo odkomentuj
           /* foreach (var date in temperaturesByDate.Keys)
            {
                Debug.WriteLine("Date: " + date);
                Debug.WriteLine("Temperatures: " + string.Join(", ", temperaturesByDate[date]));
                Debug.WriteLine("Descriptions: " + string.Join(", ", descriptionsByDate[date]));
                Debug.WriteLine("Clothes: " + string.Join(", ", clothes)); 
            }
            */
            return Ok(new { TemperaturesByDate = temperaturesByDate, DescriptionsByDate = descriptionsByDate, ClothesByDate = clothesToTake, ClothesBuy = clothesToBuy });
           
        }
        /// <summary>
        /// Tutaj trzeba dodac jeszcze
        /// </summary>
        /// <param name="clothes_needed"></param>
        /// <returns></returns>
        private Tuple<List<string>, List<string>> ClothesToTake(List<string> clothes_needed)
        {   
            List<string> clothesToTake = new List<string>();
            List<string> clothesToBuy = new List<string>();
            List<string> clothesWeHave = new List<string>();
            List<string> clothesWeNeed = clothes_needed;

            // query na jakie ubrania są w bazie
            var clothes = _context.Ubranie.Select(w => w.rodzaj_ubrania).ToList();


            // tutaj w sumie bez sensu ale niech zostanie na razie
            foreach (var clothe in clothes)
            {
                clothesWeHave.Add(clothe);
            }

            foreach (var clothe in clothesWeNeed)
            {
                if(clothesWeHave.Contains(clothe))
                {
                    clothesToTake.Add(clothe);
                    clothesWeHave.Remove(clothe);
                }
                else
                {
                    clothesToBuy.Add(clothe);
                }
            }

            Tuple<List<string>, List<string>> ClotheToTakeAndToBuy = new Tuple<List<string>, List<string>>(clothesToTake, clothesToBuy);

            return ClotheToTakeAndToBuy;
        }
        /// <summary>
        /// Logika dobierania ubran na podstawie temperatury
        /// </summary>
        /// <param name="temperaturesByDate">Lista temperatur dla danej kolekcji dat(dla danej wycieczki)</param>
        /// <returns>Liste ubran jakie nalezy zabrac</returns>
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

                clothes.Add("Koszulka");
                if (isShortsNeeded)
                {
                    clothes.Add("Krotkie spodednki");
                }
                else
                {
                    clothes.Add("Spodnie");
                }
            }

            if (isJacketNeeded)
            {
                clothes.Add("Kurtka");
            }

            if (isSweatshirtNeeded)
            {
                clothes.Add("Bluza");
            }

            return clothes;
    }



    }


}

