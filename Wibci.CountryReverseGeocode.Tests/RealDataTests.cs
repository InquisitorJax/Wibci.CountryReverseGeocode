using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Wibci.CountryReverseGeocode.Models;

namespace Wibci.CountryReverseGeocode.Tests
{
    [TestFixture]
    public class RealDataTests
    {
        private CountryReverseGeocodeService _service;

        [Test]
        public void GetState_TestMajorUSCities()
        {
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../USTestData.csv");
            List<string> csvLines = File.ReadLines(filePath).ToList();
            int total = 0;
            int success = 0;
            StringBuilder result = new StringBuilder();
            csvLines.ForEach(csvLine =>
            {
                total++;
                var tokens = csvLine.Split(',');
                
                var gl = new GeoLocation() { Latitude = double.Parse(tokens[0]), Longitude = double.Parse(tokens[1]) };
                var info = _service.FindUSAState(gl);

                //Assert
                if (info == null)
                {
                    result.AppendLine($"null for ({gl.Latitude}, {gl.Longitude}) from line {csvLine}");
                } else {
                    var returnedState = info.Name;
                    var expectedState = MapStateCodeToName(tokens[3]);
                    if (returnedState == expectedState)
                    {
                        success++;
                    } else
                    {
                        result.AppendLine($"returned state {info.Name} != {csvLine[3]}:{expectedState}");
                    }
                }
            });

            int fails = total - success;
            Assert.IsTrue(fails == 0, $"{fails}/{total} lookups failed (see below)\n" + result.ToString());

            /*if (fails > 80)
            {
                Assert.Fail($"{fails}/{total} lookups failed (see below)\n" + result.ToString());
            }*/
        }

        private string MapStateCodeToName(string c)
        {
            Dictionary<string, string> map = new Dictionary<string, string>()
            {
                ["AK"] = "Alaska",
                ["AL"] = "Alabama",
                ["AR"] = "Arkansas",
                ["AZ"] = "Arizona",
                ["CA"] = "California",
                ["CO"] = "Colorado",
                ["CT"] = "Connecticut",
                ["DC"] = "District of Columbia",
                ["DE"] = "Delaware",
                ["FL"] = "Florida",
                ["GA"] = "Georgia",
                ["GU"] = "Guam",
                ["HI"] = "Hawaii",
                ["IA"] = "Iowa",
                ["ID"] = "Idaho",
                ["IL"] = "Illinois",
                ["IN"] = "Indiana",
                ["KS"] = "Kansas",
                ["KY"] = "Kentucky",
                ["LA"] = "Louisiana",
                ["MA"] = "Massachusetts",
                ["MD"] = "Maryland",
                ["ME"] = "Maine",
                ["MI"] = "Michigan",
                ["MN"] = "Minnesota",
                ["MO"] = "Missouri",
                ["MS"] = "Mississippi",
                ["MT"] = "Montana",
                ["NC"] = "North Carolina",
                ["ND"] = "North Dakota",
                ["NE"] = "Nebraska",
                ["NH"] = "New Hampshire",
                ["NJ"] = "New Jersey",
                ["NM"] = "New Mexico",
                ["NV"] = "Nevada",
                ["NY"] = "New York",
                ["OH"] = "Ohio",
                ["OK"] = "Oklahoma",
                ["OR"] = "Oregon",
                ["PA"] = "Pennsylvania",
                ["PR"] = "Puerto Rico",
                ["RI"] = "Rhode Island",
                ["SC"] = "South Carolina",
                ["SD"] = "South Dakota",
                ["TN"] = "Tennessee",
                ["TX"] = "Texas",
                ["UT"] = "Utah",
                ["VA"] = "Virginia",
                ["VI"] = "Virgin Islands",
                ["VT"] = "Vermont",
                ["WA"] = "Washington",
                ["WI"] = "Wisconsin",
                ["WV"] = "West Virginia",
                ["WY"] = "Wyoming",
            };
            return map[c];
        }

        [Test]
        public void GetState_TestCountryCapitals()
        {
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../CountryTestData.csv");
            List<string> csvLines = File.ReadLines(filePath).ToList();
            int total = 0;
            int success = 0;
            StringBuilder result = new StringBuilder();
            csvLines.ForEach(csvLine =>
            {
                total++;
                var tokens = csvLine.Split(',');

                var gl = new GeoLocation() { Latitude = double.Parse(tokens[2]), Longitude = double.Parse(tokens[3]) };
                var info = _service.FindCountry(gl);

                //Assert
                if (info == null)
                {
                    result.AppendLine($"null for ({gl.Latitude}, {gl.Longitude}) from line {csvLine}");
                } else
                {
                    var returnedCountry = info.Name;
                    var expectedCountry = tokens[0];
                    if (returnedCountry == expectedCountry)
                    {
                        success++;
                    } else
                    {
                        result.AppendLine($"returned country {returnedCountry} != {expectedCountry}");
                    }
                }
            });

            int fails = total - success;
            
            Assert.IsTrue(fails == 0, $"{fails}/{total} lookups failed (see below)\n" + result.ToString());
            //Console.WriteLine($"{fails}/{total} lookups failed (see below)\n" + result.ToString());
            /*if (fails > 0)
            {
                Assert.Fail($"{fails}/{total} lookups failed (see below)\n" + result.ToString());
            }*/
        }

        [SetUp]
        public void Setup()
        {
            _service = new CountryReverseGeocodeService();
        }
    }
}