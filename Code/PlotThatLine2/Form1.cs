using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using ScottPlot;
using System.Linq;
using ScottPlot.WinForms;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
namespace PlotThatLine2
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        private List<City> _cities;  // Utilise un champ priv�
        
        
        public List<City> Cities
        {
            get => _cities;  // Retourne le champ priv�
            set => _cities = value;  // Affecte le champ priv�
        }

        public Form1()
        {
            InitializeComponent();
            Cities = new List<City> {};  // Initialise la liste ici
            LoadCitiesFromJsonWithoutNullablePropertiesAsync("../../../datasets/");
            Graph1.Refresh();
        }
        public async Task LoadCitiesFromJsonWithoutNullablePropertiesAsync(string folderPath)
        {

            // R�cup�rer tous les fichiers JSON dans le dossier
            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

            foreach (string file in jsonFiles)
            {
                try
                {
                    // Lire le contenu du fichier JSON
                    string jsonContent = await File.ReadAllTextAsync(file);

                    // D�s�rialiser en utilisant un mod�le r�duit sans les propri�t�s non souhait�es
                    var city = JsonSerializer.Deserialize<City>(jsonContent);

                    if (city.Name != this.Name)
                    {
                        // Convertir l'objet en City tout en ignorant time et temperature
                        City cityComplete = new City
                        (
                            city.Name,
                            city.Country,
                            city.Latitude,
                            city.Longitude,
                            city.Time,
                            city.Temperature
                        );

                        Cities.Add(cityComplete);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la lecture du fichier '{file}': {ex.Message}");
                }
            }

        }

        static string CustomFormatter(double temperature)
        {
            return $"{temperature}�";
        }

        ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new()
        {
            LabelFormatter = CustomFormatter
        };

        private async void LoadData(List<City> citiesSelected, DateTime timeStart, DateTime timeEnd)
        {
            Graph1.Reset();
            string timeStartString = timeStart.ToString("yyyy-MM-dd");
            string timeEndString = timeEnd.ToString("yyyy-MM-dd");

            ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
            {
                LabelText = "Temp�rature en degr�",
            };
            customAxisY.FrameLineStyle.IsVisible = false;
            Graph1.Width = 500;
            Graph1.Height = 400;
            Graph1.Plot.Axes.Left.TickGenerator = myTickGenerator;
            Graph1.Plot.Axes.DateTimeTicksBottom();
            Graph1.Plot.Axes.AddLeftAxis(customAxisY);

            foreach (City city in citiesSelected)
            {
                string apiUrl = $"https://archive-api.open-meteo.com/v1/archive?latitude={city.Latitude}&longitude={city.Longitude}&start_date={timeStartString}&end_date={timeEndString}&daily=temperature_2m_max";

                try
                {

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);

                    // Extraction des donn�es
                    DateTime[] times = json["daily"]["time"].ToObject<DateTime[]>();
                    double[] temperatures = json["daily"]["temperature_2m_max"].ToObject<double[]>();

                    // Stocker les donn�es dans la propri�t� de City
                    city.Time = times;
                    city.Temperature = temperatures;

                    var line = Graph1.Plot.Add.ScatterLine( city.Time, city.Temperature);
                    line.LegendText = city.Name;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Erreur lors de l'appel API : {e.Message}");
                }
            }
            refresh();
            // Rafra�chir le graphique apr�s ajout des donn�es
            Graph1.Refresh();

            // Mettre � jour la CheckedListBox

        }

        private void UpdateCheckedListBox()
        {
            foreach (City city in Cities)
            {
                if (!checkedListBox1.Items.Contains(city.Name)) // V�rifie si le nom n'est pas d�j� pr�sent
                {
                    checkedListBox1.Items.Add(city.Name); // Ajoute le nom s'il n'est pas dans la liste
                }
            }
           
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search_Click(sender, e);

        }

        private void addCityB_Click(object sender, EventArgs e)
        {
            addCity newFormAddCity = new addCity();
            newFormAddCity.CityAdded += AddCityToList;
            newFormAddCity.Show();
        }

        private void AddCityToList(City newCity)
        {
            Cities.Add(newCity);  // Ajoute la nouvelle ville � la liste
            MessageBox.Show($"La ville {newCity.Name} a �t� ajout�e !");
            refresh();
        }
        private void refresh()
        {
            UpdateCheckedListBox();  // Rafra�chit la liste des villes affich�es
            foreach (City city in Cities)
            {
                city.CreateJsonFileAsync();  // Appelle la m�thode d'exportation JSON
            }
        }
        private void refresh_Click(object sender, EventArgs e)
        {
        }

        private void dateTimePickerDebut_ValueChanged(object sender, EventArgs e)
        {
            Search_Click(sender, e);
        }

        private void dateTimePickerFin_ValueChanged(object sender, EventArgs e)
        {
            Search_Click(sender, e);

        }

        private void Search_Click(object sender, EventArgs e)
        {
            refresh();
            DateTime timeStart = dateTimePickerDebut.Value;
            DateTime timeEnd = dateTimePickerFin.Value;

            List<string> citiesSelectedString = checkedListBox1.CheckedItems.Cast<string>().ToList();

            List<City> citiesSelected = Cities
                .Where(city => citiesSelectedString.Contains(city.Name))
                .ToList();

            LoadData(citiesSelected, timeStart, timeEnd);
        }
    }
}
