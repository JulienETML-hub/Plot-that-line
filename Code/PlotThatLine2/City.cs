using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotThatLine2
{
    public class City
    {
        public string name;
        public string country;
        public double latitude;
        public double longitude;

        public City(string name, string country, double latitude, double longitutude)
        {
            this.name = name;
            this.country = country;
            this.latitude = latitude;
            this.longitude = longitutude;

            
        }
    }
}
