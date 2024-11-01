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
        /// <summary>
        /// Méthode qui va créer un fichier json avec les informations de base (name, country, latitude, longitude) 
        /// </summary>
        /// <returns></returns>
        public async Task CreateJsonFileAsync()
        {
            string filePath = $"../../../datasets/{this.Name}_weather_data.json";

            // Définition du nom du fichier JSON
            if (!File.Exists(filePath))
            {            
                
                var cityData = new
                {
                    Name = this.Name,
                    Country = this.Country,
                    Latitude = this.Latitude,
                    Longitude = this.Longitude

                };
                // Conversion de l'objet en JSON
                string jsonString = JsonSerializer.Serialize(cityData, new JsonSerializerOptions { WriteIndented = true });
                string fileName = $"{this.Name}_weather_data.json";

                // Écriture du fichier JSON
                await File.WriteAllTextAsync("../../../datasets/" + fileName, jsonString);

                Console.WriteLine($"Le fichier JSON '{fileName}' a été créé avec succès !");
            }
        }
        /// <summary>
        /// Méthode qui va mettre à jour les données du fichier json grâce aux données de l'objet
        /// </summary>
        /// <returns></returns>
        public async Task StoreDataAsync()
        {
            try
            {
                // Création du nom de fichier basé sur la propriété Name de la ville
                string filePath = $"../../../datasets/{this.Name}_weather_data.json";
                City existingCityData = null;
                // Vérifier si le fichier existe
                if (File.Exists(filePath))
                {
                    try
                    {
                        // Lire les données existantes du fichier
                        string existingJson = await File.ReadAllTextAsync(filePath);
                        existingCityData = JsonSerializer.Deserialize<City>(existingJson);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la lecture du fichier existant pour {this.Name} : {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y a pas de fichier à filePath");
                }
                // Fusionner les données
                if (existingCityData != null)
                {
                    // Fusionner les dates et températures
                    if (existingCityData.Time != null && existingCityData.Temperature != null && this.Time != null && this.Temperature !=null )
                    {
                        var mergedData = existingCityData.Time
                            .Zip(existingCityData.Temperature, (time, temp) => new { time, temp })
                            .Concat(this.Time.Zip(this.Temperature, (time, temp) => new { time, temp }))
                            .GroupBy(data => data.time) // Grouper par date (time)
                            .Select(g => new
                            {
                                Time = g.Key,
                                Temperature = g.Last().temp // Prioriser les nouvelles valeurs
                            })
                            .OrderBy(data => data.Time)
                            .ToList();

                        this.Time = mergedData.Select(d => d.Time).ToArray();
                        this.Temperature = mergedData.Select(d => d.Temperature).ToArray();
                    }
                }
                else
                {
                    MessageBox.Show("existingCityData est null");
                }
                // Sérialiser l'objet City en JSON
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(this, options);

                // Enregistrer le JSON fusionné dans un fichier
                await File.WriteAllTextAsync(filePath, jsonString);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement des données de la ville {this.Name} : {ex.Message} {this.Temperature.Last()}");
            }
        }
    }
}