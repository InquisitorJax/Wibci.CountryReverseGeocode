namespace Wibci.CountryReverseGeocode.Models 
{

    public struct AreaData 
    {
        public string id;
        public string name;
        public string fips;
        public MultiPolygon geometry;

        public AreaData(string id, string name, string fips, MultiPolygon geometry) 
        {
            this.id = id;
            this.name = name;
            this.fips = fips;
            this.geometry = geometry;
        }
    }

    public struct MultiPolygon {
        public (double, double)[][] coordinates;

        public MultiPolygon((double, double)[][] coordinates) 
        {
            this.coordinates = coordinates;
        }
    }
}