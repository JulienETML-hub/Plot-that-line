using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlotThatLine2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotThatLine2.Tests
{
    [TestClass()]
    public class CityTests
    {
        [TestMethod()]
        public void changeCountryTest()
        {
            // Arrange
            City city = new City("Lausanne", "Suise", 45, 5);

            // Act
            city.changeCountry("Suisse");

            // Assert
            Assert.IsTrue(city.Country is string);
            Assert.AreEqual("Suisse", city.Country);
        }

        [TestMethod()]
        public void changeNameTest()
        {
            // Arrange
            City city = new City("Pully2", "Suise", 45, 5);

            // Act
            city.changeName("Pully");

            // Assert
            Assert.IsTrue(city.Name is string);
            Assert.AreEqual("Pully", city.Name);
        }
        [TestMethod]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            City city = new City("Lausanne", "Suisse", 45, 5);

            // Assert
            Assert.AreEqual("Lausanne", city.Name);
            Assert.AreEqual("Suisse", city.Country);
            Assert.AreEqual(45, city.Latitude);
            Assert.AreEqual(5, city.Longitude);
        }
        

    }
}