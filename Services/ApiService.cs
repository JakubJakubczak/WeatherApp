using Newtonsoft.Json;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    public static class ApiService
    {
        public static async Task<Root> GetWeather(double latitude, double longitude)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid=4b00570f91b09ef35a70dff6fd11fb4e",latitude,longitude));
            return JsonConvert.DeserializeObject<Root>(response);
        }

        public static async Task<Root> GetWeatherByCity(string city)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&appid=4b00570f91b09ef35a70dff6fd11fb4e", city));
            return JsonConvert.DeserializeObject<Root>(response);
        }


    }
}
