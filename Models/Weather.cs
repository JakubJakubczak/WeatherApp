using Newtonsoft.Json;

namespace WeatherApplication.Models
{
    public class oWeather
    {
        public int id { get; set; }
        public string name { get; set; }
        public double temp {  get; set; }
        public double windSpeed { get; set; }
        public double hum { get; set; }
        public int? month {  get; set; }
        public int? day { get; set; }
        public int? hour { get; set; }

        public oWeather() { }

    }


}

