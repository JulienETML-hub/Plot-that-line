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
    public class Form1Tests
    {
        [TestMethod()]
    public void UpdateCheckedListBox_AddsUniqueCities() { 
    {
        // Arrange
        var form = new Form1();
        var checkedListBox1 = form.GetCheckedListBox1();
        form.Cities = new List<City>
    {
        new City("Paris", "France", 48.8566, 2.3522),
        new City("Lyon", "France", 45.7640, 4.8357),
        new City("Paris", "France", 48.8566, 2.3522) // Duplicate city name
    };
        checkedListBox1.Items.Add("Lyon"); // Already contains Lyon

        // Act
        form.UpdateCheckedListBox();

        // Assert
        Assert.AreEqual(2, checkedListBox1.Items.Count); // Only "Paris" and "Lyon" should be present
        Assert.IsTrue(checkedListBox1.Items.Contains("Paris"));
        Assert.IsTrue(checkedListBox1.Items.Contains("Lyon"));
    }

    }
    }
}