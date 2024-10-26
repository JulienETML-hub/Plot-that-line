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
        private List<City> _cities; 
        public List<City> Cities
        {
            get => _cities;  // Retourne le champ priv�
            set => _cities = value;  // Affecte le champ priv�
        }

        public Form1()
        {
            InitializeComponent();
            Cities = new List<City> {};             
            LoadCitiesFromJsonWithoutNullablePropertiesAsync("../../../datasets/");
            Graph1.Refresh();
        }
        /// <summary>
        /// M�thode qui va cr�er des objets City pour chaque fichier json (contenant les donn�es d'une city) dans un r�p�rtoire donn�e
        /// et qui les ajoute � la liste Cities
        /// </summary>
        /// <param Chemin o� se trouve les fichiers="folderPath"></param>
        /// <returns></returns>
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
        /// <summary>
        /// M�thode pr rajouter � �  la fin d'une temp�rature donn�es
        /// </summary>
        /// <param name="temperature"></param>
        /// <returns>string</returns>
        static string CustomFormatter(double temperature)
        {
            return $"{temperature}�";
        }

        ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new()
        {
            LabelFormatter = CustomFormatter
        };
        /// <summary>
        /// Affichage du graphique
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        private void GraphRefresh(DateTime timeStart, DateTime timeEnd)
        {
            Graph1.Reset();
            Graph1.Width = 500;
            Graph1.Height = 400;
            Graph1.Plot.Axes.Left.TickGenerator = myTickGenerator;
            Graph1.Plot.Axes.DateTimeTicksBottom();
            Graph1.Plot.Title("Graphique des temp�ratures");
        }
        /// <summary>
        /// M�thode qui va charger les donn�es de chaque villes pr�sentes dans Cities (via donn�es local ou API)
        /// et qui va ensuite afficher les donn�es sous forme de courbe, selon la requ�te (date de d�but et fin)
        /// </summary>
        /// <param name="citiesSelected"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        private async void LoadData(List<City> citiesSelected, DateTime timeStart, DateTime timeEnd)
        {
            GraphRefresh(timeStart, timeEnd);
            string timeStartString = timeStart.ToString("yyyy-MM-dd");
            string timeEndString = timeEnd.ToString("yyyy-MM-dd");
            foreach (City city in citiesSelected)
            {
                DateTime firstAvailableDate;
                DateTime? lastAvailableDate = null;
                bool dataIncomplete = true;
                if (File.Exists($"../../../datasets/{city.Name}_weather_data.json"))
                {
                    try
                    {
                        string jsonContent = await File.ReadAllTextAsync($"../../../datasets/{city.Name}_weather_data.json");
                        var storedCity = JsonSerializer.Deserialize<City>(jsonContent);

                        // V�rifier si les donn�es couvrent la plage de dates demand�e
                        if (storedCity.Time != null && storedCity.Time.Length >= 0)
                        {
                            firstAvailableDate = storedCity.Time.Min();
                            lastAvailableDate = storedCity.Time.Max();

                            // Si les donn�es couvrent la plage de dates demand�e
                            if (firstAvailableDate <= timeStart && lastAvailableDate >= timeEnd )
                            {
                                // Utiliser les donn�es locales sans appel � l'API
                                city.Time = storedCity.Time;
                                city.Temperature = storedCity.Temperature;

                                dataIncomplete = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dataIncomplete = true;
                        MessageBox.Show($"Erreur lors de la lecture des donn�es locales pour {city.Name}: {ex.Message}");
                    }
                }
                if (dataIncomplete == true)
                {
                    string apiUrl;
                    DateTime today = DateTime.Now.AddDays(-3);
                    string todayString = today.ToString("yyyy-MM-dd");
                    MessageBox.Show(todayString);
                    apiUrl = $"https://archive-api.open-meteo.com/v1/archive?latitude={city.Latitude}&longitude={city.Longitude}&start_date={timeStartString}&end_date={todayString}&daily=temperature_2m_max";
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
                        // Mise � jour du fichier json local (correspondant � la city)
                        city.StoreDataAsync();
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Erreur lors de l'appel API : {e.Message}");
                    }
                }
                DateTime[] timesToDisplay;
                double[] temperaturesToDisplay;

                // TODO #1 Ici cela ne prend pas les premi�res donn�es du laps de temps dont on a besoin (il manque juste le 1jour)
                timesToDisplay = city.Time.Where(t => t >= timeStart && t <= timeEnd).ToArray();
                temperaturesToDisplay = city.Temperature.SkipLast(Array.IndexOf(city.Time, timesToDisplay.First()))
                                                               .Take(timesToDisplay.Length)
                                                               .ToArray();
                // Affichage de la courbe 
                var line = Graph1.Plot.Add.ScatterLine(timesToDisplay, temperaturesToDisplay);
                line.LegendText = city.Name;
            }
            UpdateCheckedListBox();
            // Rafra�chir le graphique apr�s ajout des donn�es
            Graph1.Refresh();

            // Mettre � jour la CheckedListBox

        }
        /// <summary>
        /// V�rifie pour chaque ville s'il est pr�sente dans la checkBoxList, s'il ne l'est pas elle l'ajoute
        /// </summary>
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
        /// <summary>
        /// En cas d'interaction avec la checkBoxList, appel la m�thode "Search_Click"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search_Click(sender, e);

        }
        /// <summary>
        /// Ouvre un nouveau formulaire "addCity" permettant d'ajouter une ville � la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCityB_Click(object sender, EventArgs e)
        {
            addCity newFormAddCity = new addCity();
            newFormAddCity.CityAdded += AddCityToList;
            newFormAddCity.Show();
        }
        /// <summary>
        /// Ajoute la city passer en param�tre � la liste Cities
        /// </summary>
        /// <param name="newCity"></param>
        private void AddCityToList(City newCity)
        {
            Cities.Add(newCity);  // Ajoute la nouvelle ville � la liste
            
            MessageBox.Show($"La ville {newCity.Name} a �t� ajout�e !");
            refresh();
        }
        /// <summary>
        /// Rafraichie la liste des villes, et cr�e un json file simple pour chacune (name, country, latitude, longitude)
        /// </summary>
        private void refresh()
        {
            UpdateCheckedListBox();  // Rafra�chit la liste des villes affich�es
            foreach (City city in Cities)
            {
                city.CreateJsonFileAsync();  // Appelle la m�thode d'exportation JSON
            }
        }
        /// En cas d'interaction avec le dateTimePicker, appel la m�thode "Search_Click"
        private void dateTimePickerDebut_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerDebut.Value <= dateTimePickerFin.Value)
            {
                Search_Click(sender, e);
            }
            else
            {
                MessageBox.Show("valeur supp");
            }
        }
        /// En cas d'interaction avec le dateTimePicker, appel la m�thode "Search_Click"
        private void dateTimePickerFin_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerDebut.Value <= dateTimePickerFin.Value)
            {
                Search_Click(sender, e);
            }
            else
            {
                MessageBox.Show("valeur supp");
            }
        }
        /// <summary>
        /// Cr�e une liste avec les villes s�lectionner dans la checkBoxList et appel la m�thode loadDate avec cette 
        /// liste ainsi qu'avec les dates choisis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
