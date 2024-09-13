using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotThatLine2
{
    public class City
    {
        DateTime[] time;
        double[] temperature;
        string name;
        string country;
        double latitude;
        double longitude;

        public City(string name, string country, double latitude, double longitutude)
        {
            this.name = name;
            this.country = country;
            this.latitude = latitude;
            this.longitude = longitutude;

            
        }
        public DateTime[] Time { get; set; }
        public double[] Temperature { get; set; }
        public string Name { get; set; }
        public string Country { get; set; } 
        public double Latitude { get { return latitude;} }
        public double Longitude { get { return longitude;} }

        public DateTime[] Time1 { get => time; set => time = value; }
    }
}
