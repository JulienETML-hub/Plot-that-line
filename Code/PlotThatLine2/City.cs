using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlotThatLine2
{
    public class City
    {

        string name;
        string country;
        double latitude;
        double longitude;
        DateTime[]? time;
        double[]? temperature;

        public string Name { get => name; set => name = value; }
        public string Country { get => country; set => country = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public DateTime[]? Time { get => time; set => time = value; }
        public double[]? Temperature { get => temperature; set => temperature = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public City(string name, string country, double latitude, double longitude, DateTime[]? time, double[]? temperature)
        {
            this.Name = name;
            this.Country = country;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Time = time;
            this.Temperature = temperature;
        }
        public City(string name, string country, double latitude, double longitude)
        {
            Name = name;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
            Time = null;
            Temperature = null;
        }
        public City() { }

        public async Task CreateJsonFileAsync()
        {
            var cityData = new
            {
                Name = this.Name,
                Country = this.Country,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Temperature = this.Temperature,
                Time = this.Time,
                Data = new List<object>()
            };

            // Ajout des données journalières de température dans l'objet cityData
            if (this.Time != null)
            {
                for (int i = 0; i <= this.Time.Length; i++)
                {
                    cityData.Data.Add(new { Date = this.Time[i], Temperature = this.Temperature[i] });
                }
            }

            // Conversion de l'objet en JSON
            string jsonString = JsonSerializer.Serialize(cityData, new JsonSerializerOptions { WriteIndented = true });

            // Définition du nom du fichier JSON
            string fileName = $"{this.Name}_weather_data.json";

            // Écriture du fichier JSON
            await File.WriteAllTextAsync("../../../datasets/" + fileName, jsonString);

            Console.WriteLine($"Le fichier JSON '{fileName}' a été créé avec succès !");
        }
    }
}