﻿namespace WeatherApplication.Models
{
    /// <summary>
    /// Model do przechowywania ubran w bazie danych
    /// </summary>
    public class Ubranie
    {
        public int id { get; set; }
        public string rodzaj_ubrania { get; set; }
        public string kolor { get; set; }
        public string? opis { get; set; }

    }
}   
