using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using ScottPlot;
using System.Linq;
using ScottPlot.WinForms;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Text.Json.Nodes;
using System.Text.Json;
namespace PlotThatLine2
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private City Lausanne;

        public Form1()
        {
            InitializeComponent();
            Lausanne = new City("Lausanne", "Swiss", 46.519, 6.632);
            LoadData();
        }

        private async void LoadData()
        {
            string apiUrl = $"https://archive-api.open-meteo.com/v1/archive?latitude={Lausanne.Latitude}&longitude={Lausanne.Longitude}&start_date=2020-01-01&end_date=2024-01-01&daily=temperature_2m_max";

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
               // string[] test = responseBody.Split(',');
              //  var test2 = test.Select(x => x.Contains("T12:00")).ToList();
               JObject json = JObject.Parse(Convert.ToString(responseBody));

                // Extraction des données
                DateTime[] times = json["daily"]["time"].ToObject<DateTime[]>();
                double[] temperatures = json["daily"]["temperature_2m_max"].ToObject<double[]>();

                // Stocker les données dans la propriété de City
                Lausanne.Time = times;
                Lausanne.Temperature = temperatures;


                // Exemple de graphique avec ScottPlot
                if (Lausanne.Time.Length > 0 && Lausanne.Temperature.Length > 0)
                {
                    double[] x = { 1.5, 2.8, 4.1, 5.6, 7.2, 8.9, 10.3, 12.4, 14.6, 16.1 };
                    double[] y = { 1.9, 3.2, 5.7, 6.8, 8.4, 10.2, 12.6, 14.3, 16.7, 19.0 };
                    Graph1.Width = 500;
                    Graph1.Height = 400;
                    Graph1.Plot.Add.ScatterLine(Lausanne.Time,Lausanne.Temperature);
                    Graph1.Plot.Axes.DateTimeTicksBottom();
                    Graph1.Plot.Axes.SetLimitsY(Lausanne.Temperature.Min(), Lausanne.Temperature.Max());

                    Graph1.Plot.ShowGrid();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erreur de requête HTTP: {e.Message}");
            }
        }
    




    public void cityBox_TextChanged(object sender, EventArgs e)
        {
            cityBox.Text = "Temporaire";
        }

        private void Graph1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
