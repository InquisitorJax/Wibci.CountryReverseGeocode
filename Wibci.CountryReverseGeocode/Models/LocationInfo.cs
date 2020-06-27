namespace Wibci.CountryReverseGeocode.Models
{
    public class LocationInfo
    {
        public static LocationInfo FromAreaData(AreaData ad) {
            return new LocationInfo(ad.id, ad.name);
        }
        public LocationInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; private set; }
        public string Name { get; private set; }
    }
}