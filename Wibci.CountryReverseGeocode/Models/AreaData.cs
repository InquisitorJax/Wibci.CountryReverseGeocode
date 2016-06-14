using System.Collections.Generic;

namespace Wibci.CountryReverseGeocode.Models
{
    //classes used to deserialize json data in Data folder:

    internal abstract class AreaData
    {
        public string id { get; set; }
        public Properties properties { get; set; }
        public string type { get; set; }
    }

    internal abstract class Geometry
    {
        public string type { get; set; }
    }

    internal class MultiPolygon : Geometry
    {
        public List<List<List<List<double>>>> coordinates { get; set; }
    }

    internal class MultiPolygonData : AreaData
    {
        public MultiPolygon geometry { get; set; }
    }

    internal class Polygon : Geometry
    {
        public List<List<List<double>>> coordinates { get; set; }
    }

    internal class PolygonData : AreaData
    {
        public Polygon geometry { get; set; }
    }

    internal class Properties
    {
        public string fips { get; set; }
        public string name { get; set; }
    }
}