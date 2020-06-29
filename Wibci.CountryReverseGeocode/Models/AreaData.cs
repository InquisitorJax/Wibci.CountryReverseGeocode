using System.Collections.Generic;

namespace Wibci.CountryReverseGeocode.Models
{
    //classes used to deserialize json data in Data folder:

    public class AreaData {
        public AreaData(string id, string name, List<List<List<double>>> coordinates)
        {
            this.id = id;
            this.name = name;
            this.coordinates = coordinates;
        }

        public string id { get; set; }
        public string name { get; set; }

        public List<List<List<double>>> coordinates { get; set; }

    }
}