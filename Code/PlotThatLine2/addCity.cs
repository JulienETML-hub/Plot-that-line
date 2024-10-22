using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PlotThatLine2
{
    public partial class addCity : Form
    {
        public event Action<City> CityAdded;  // Déclare l'événement
        public string name;
        public string country;
        public double latitude;
        public double longitude;



        public addCity()
        {
            InitializeComponent();
        }

        private void nameOfCity_TextChanged(object sender, EventArgs e)
        {
            this.name = nameOfCity.Text;
        }



        private void CountryOfCity_TextChanged(object sender, EventArgs e)
        {
            this.country = CountryOfCity.Text;
        }
        private void latitudeOfCity_TextChanged(object sender, EventArgs e)
        {
            this.latitude = Convert.ToDouble(latitudeOfCity.Text);
        }

        private void longitudeOfCity_TextChanged(object sender, EventArgs e)
        {
            this.longitude = Convert.ToDouble(longitudeOfCity.Text);
        }
        private void validate_Click(object sender, EventArgs e)
        {
            City addedCity = new City(this.name, this.country, this.latitude, this.longitude);
            closeAddCity(addedCity);
        }
        private void closeAddCity(City city)
        {
            CityAdded?.Invoke(city);
            this.Close();
        }

        private void addCity_Load(object sender, EventArgs e)
        {

        }
    }
}
