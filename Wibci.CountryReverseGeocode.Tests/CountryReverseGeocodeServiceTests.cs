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
    public class CountryReverseGeocodeServiceTests
    {
        private CountryReverseGeocodeService _service;

        [Test]
        public void GetCountry_ForInsideMultiPolygon_FindsCountry()
        {
            //Arrange
            //Buco Zau -4.895250, 12.437329
            GeoLocation angola = new GeoLocation { Longitude = 12.437329, Latitude = -4.895250 };

            //Act
            var info = _service.FindCountry(angola);

            //Assert
            Assert.NotNull(info, "Expected to find area information");
            Assert.AreEqual("Angola", info.Name, "Expected to return 'Angola' as a country");
        }

        [Test]
        public void GetCountry_ForInsidePolygon_FindsCountry()
        {
            //Arrange
            //35.227575, 65.167173
            GeoLocation afghanistan = new GeoLocation { Longitude = 65.167173, Latitude = 35.227575 };

            //Act
            var info = _service.FindCountry(afghanistan);

            //Assert
            Assert.NotNull(info, "Expected to find area information");
            Assert.AreEqual("Afghanistan", info.Name, "Expected to return 'Afghanistan' as country");
        }

        [Test]
        public void GetCountry_ForOutsideMultiPolygon_DoesNotFindCountry()
        {
            //Arrange
            //Bering Sea
            GeoLocation alaskaOcean = new GeoLocation { Longitude = -172.060482, Latitude = 62.510037 };

            //Act
            var info = _service.FindCountry(alaskaOcean);

            //Assert
            Assert.Null(info, "Expected not to find area information for coordinate outside the states");
        }

        [Test]
        public void GetCountry_ForRussiaBug_FindsCountry()
        {
            //Arrange
            GeoLocation location = new GeoLocation { Latitude = 50.064546, Longitude = 40.587502 };

            //Act
            var info = _service.FindCountry(location);

            //Assert
            Assert.NotNull(info, "Expected to find area information");
            Assert.AreEqual("Russia", info.Name, "Expected to return 'Russia' as a country");
        }

        [Test]
        public void GetState_ForInsideMultiPolygon_FindsState()
        {
            //Arrange
            //Savoonga Island
            GeoLocation alaska = new GeoLocation { Longitude = -170.333949, Latitude = 63.431027 };

            //Act
            var info = _service.FindUSAState(alaska);

            //Assert
            Assert.NotNull(info, "Expected to find area information");
            Assert.AreEqual("Alaska", info.Name, "Expected to return 'Alaska' as state");
        }

        [Test]
        public void GetState_ForInsidePolygon_FindsState()
        {
            //Arrange
            GeoLocation alabama = new GeoLocation { Longitude = -87.762676, Latitude = 34.305190 };

            //Act
            var info = _service.FindUSAState(alabama);

            //Assert
            Assert.NotNull(info, "Expected to find area information");
            Assert.AreEqual("Alabama", info.Name, "Expected to return 'Alabama' as state");
        }

        [Test]
        public void GetState_ForInsidePolygon_FindsState_California()
        {
            //California 

            //Arrange
            GeoLocation california = new GeoLocation { Longitude = -122.4404, Latitude = 37.78595 };

            //Act
            var info = _service.FindUSAState(california);

            //Assert
            Assert.NotNull(info, "Expected to find area information");
            Assert.AreEqual("California", info.Name, "Expected to return 'California' as state");
        }

        [Test]
        public void GetState_ForOutsidePolygon_DoesNotFindState()
        {
            //Arrange

            //capetown
            GeoLocation capetown = new GeoLocation { Longitude = 18.751451, Latitude = -33.855952 };

            //Act
            var info = _service.FindUSAState(capetown);

            //Assert
            Assert.Null(info, "Expected not to find area information for coordinate outside the states");
        }

        [SetUp]
        public void Setup()
        {
            _service = new CountryReverseGeocodeService();
        }

        [Test]
        [Ignore("Figure a way to get currency from country")]
        public void TestCultures()
        {
            //Arrange
            //Buco Zau -4.895250, 12.437329
            GeoLocation angola = new GeoLocation { Longitude = 12.437329, Latitude = -4.895250 };

            //Act
            var info = _service.FindCountry(angola);

            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID)).OrderBy(y => y.ThreeLetterISORegionName);

            foreach (var region in regions)
            {
                Debug.WriteLine(region.ThreeLetterISORegionName);
            }

            var currencySymbol = regions.FirstOrDefault(x => x.ThreeLetterISORegionName == info.Id);
        }
    }
}