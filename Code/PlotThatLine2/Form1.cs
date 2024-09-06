using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using ScottPlot;
using System.Linq;

namespace PlotThatLine2
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task Main(string[] args)
        {
            City Lausanne = new City("Lausanne", "Swiss", 46.519, 6.632);



            string apiUrl = $"https://archive-api.open-meteo.com/v1/archive?latitude={Lausanne.latitude}&longitude={Lausanne.longitude}&start_date=2024-08-26&end_date=2024-09-05&hourly=temperature_2m";

            try
            {
                // Envoyer une requête GET
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Lève une exception si le code de statut HTTP n'est pas 200-299

                // Lire la réponse en tant que chaîne

                string responseBody = await response.Content.ReadAsStringAsync();

                //Console.WriteLine(responseBody);
                Console.WriteLine("HEADER : " + responseBody);
                Console.ReadLine();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erreur de requête HTTP: {e.Message}");
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void cityBox_TextChanged(object sender, EventArgs e)
        {
            cityBox.Text = "lausnane";
        }

        private void Graph1_Load(object sender, EventArgs e)
        {

        }
    }
}
