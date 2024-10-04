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
        DateTime[]? time;
        double[] temperature;
        string name;
        string country;
        double? latitude;
        double? longitude;
        public double? Latitude { get { return latitude; } }
        public double? Longitude { get { return longitude; } }
        public DateTime[]? Time { get => time; set => time = value; }
        public string Name { get => name; set => name = value; }
        public string Country { get => country; set => country = value; }
        public double[] Temperature { get => temperature; set => temperature = value; }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public City(string name, string country, double? latitude, double? longitude)
        {
            this.name = name;
            this.country = country;
            this.latitude = latitude;
            this.longitude = longitude;

            
        }

        public async Task CreateJsonFileAsync()
        {
            var cityData = new
            {
                Name = this.name,
                Country = this.country,
                Latitude = this.latitude,
                Longitude = this.longitude,
                Data = new List<object>()
            };

            // Ajout des données journalières de température dans l'objet cityData
            if (this.time != null)
            {
                for (int i = 0; i <= this.time.Length; i++)
                {
                    cityData.Data.Add(new { Date = this.time[i], Temperature = this.temperature[i] });
                }
            }

            // Conversion de l'objet en JSON
            string jsonString = JsonSerializer.Serialize(cityData, new JsonSerializerOptions { WriteIndented = true });

            // Définition du nom du fichier JSON
            string fileName = $"{this.name}_weather_data.json";

            // Écriture du fichier JSON
            await File.WriteAllTextAsync($"../../../datasets/{fileName}", jsonString);

            Console.WriteLine($"Le fichier JSON '{fileName}' a été créé avec succès !");
        }
    }
}
