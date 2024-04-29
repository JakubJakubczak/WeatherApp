using Newtonsoft.Json;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    /// <summary>
    /// Obsluga polaczenia z api openweathermap
    /// </summary>
    public static class ApiService
    {
        /// <summary>
        /// Pobranie danych pogodowych dla danej lokalizcji
        /// </summary>
        /// <param name="latitude">szerokosc geograficzna</param>
        /// <param name="longitude">dlugosc geograficzna</param>
        /// <returns>obeikt typu root zdeserializowany z pliku json otrzymanego z api</returns>
        public static async Task<Root> GetWeather(double latitude, double longitude)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid=4b00570f91b09ef35a70dff6fd11fb4e",latitude,longitude));
            return JsonConvert.DeserializeObject<Root>(response);
        }

        /// <summary>
        /// Pobranie danych pogodowych dla danego miasta
        /// </summary>
        /// <param name="city">Miasto dla ktorego chcemy pobrac pogode</param>
        /// <returns>obeikt typu root zdeserializowany z pliku json otrzymanego z api</returns>
        public static async Task<Root> GetWeatherByCity(string city)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&appid=4b00570f91b09ef35a70dff6fd11fb4e", city));
            return JsonConvert.DeserializeObject<Root>(response);
        }


    }
}
