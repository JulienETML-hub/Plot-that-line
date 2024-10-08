using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using ScottPlot;
using System.Linq;
using ScottPlot.WinForms;
using System.Collections.Generic;

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
            City Lausanne = new City("Lausanne", "Swiss", 46.519, 6.632);
            City Geneve = new City("Gen�ve", "Swiss", 46.2205, 6.132);
            Cities = new List<City> { Lausanne, Geneve };  // Initialise la liste ici
            Graph1.Refresh();
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

                    Graph1.Plot.Add.ScatterLine(city.Time, city.Temperature);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Erreur lors de l'appel API : {e.Message}");
                }
            }

            // Rafra�chir le graphique apr�s ajout des donn�es
            Graph1.Refresh();

            // Mettre � jour la CheckedListBox

        }

        private void UpdateCheckedListBox()
        {
            checkedListBox1.Items.Clear();
            foreach (City city in Cities)
            {
                checkedListBox1.Items.Add(city.Name);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            UpdateCheckedListBox();  // Rafra�chit la liste des villes affich�es
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            checkedListBox1.Enabled = true;
            UpdateCheckedListBox();  // Rafra�chit la liste des villes affich�es
            foreach (City city in Cities)
            {
                city.CreateJsonFileAsync();  // Appelle la m�thode d'exportation JSON
            }
        }

        private void dateTimePickerDebut_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePickerFin_ValueChanged(object sender, EventArgs e)
        {
        }

        private void Search_Click(object sender, EventArgs e)
        {
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
