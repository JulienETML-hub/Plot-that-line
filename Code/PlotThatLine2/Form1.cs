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

        static string CustomFormatter(double temperature)
        {

                return $"{temperature}°";

        }
        ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new()
        {
            LabelFormatter = CustomFormatter
        };
        private async void LoadData()
        {
            string apiUrlAlaska = $"https://archive-api.open-meteo.com/v1/archive?latitude=61.16&longitude=-153.632&start_date=2024-04-01&end_date=2024-06-30&daily=temperature_2m_max";
            string apiUrl = $"https://archive-api.open-meteo.com/v1/archive?latitude={Lausanne.Latitude}&longitude={Lausanne.Longitude}&start_date=2004-03-01&end_date=2004-03-27&daily=temperature_2m_max";

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                //response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

               JObject json = JObject.Parse(Convert.ToString(responseBody));

                // Extraction des données
                DateTime[] times = json["daily"]["time"].ToObject<DateTime[]>();
                double[] temperatures = json["daily"]["temperature_2m_max"].ToObject<double[]>();

                // Stocker les données dans la propriété de City
                Lausanne.Time = times;
                Lausanne.Temperature = temperatures;


                if (Lausanne.Time.Length > 0 && Lausanne.Temperature.Length > 0)
                {
                    ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
                    {
                        LabelText = "Température en degrée",
                    };
                    customAxisY.FrameLineStyle.IsVisible = false;
                    Graph1.Width = 500;
                    Graph1.Height = 400;
                    Graph1.Plot.Axes.Left.TickGenerator = myTickGenerator;
                    Graph1.Plot.Add.ScatterLine(Lausanne.Time,Lausanne.Temperature);
                    Graph1.Plot.Axes.DateTimeTicksBottom();
                    Graph1.Plot.Axes.SetLimitsY(Lausanne.Temperature.Min(), Lausanne.Temperature.Max());
                    Graph1.Plot.Axes.AddLeftAxis(customAxisY);

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
