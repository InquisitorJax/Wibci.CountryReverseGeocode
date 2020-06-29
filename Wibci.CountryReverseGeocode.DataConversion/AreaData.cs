using System.Collections.Generic;

namespace Wibci.CountryReverseGeocode.DataConversion.Models
{
    //classes used to deserialize json data in Data folder:

    internal abstract class InputAreaData
    {
        public string id { get; set; }
        public InputProperties properties { get; set; }
        public string type { get; set; }

    }

    internal abstract class InputGeometry {
        public string type { get; set; }
    }

    internal class InputMultiPolygon : InputGeometry {
        public List<List<List<List<double>>>> coordinates { get; set; }
    }

    internal class InputMultiPolygonData : InputAreaData {
        public InputMultiPolygon geometry { get; set; }
    }

    internal class InputPolygon : InputGeometry {
        public List<List<List<double>>> coordinates { get; set; }
    }

    internal class InputPolygonData : InputAreaData {
        public InputPolygon geometry { get; set; }
    }

    internal class InputProperties {
        public string fips { get; set; }
        public string name { get; set; }
    }
}