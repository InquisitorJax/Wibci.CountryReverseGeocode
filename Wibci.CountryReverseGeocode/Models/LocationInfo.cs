namespace Wibci.CountryReverseGeocode.Models
{
    public class LocationInfo
    {
        public LocationInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string CurrencySymbol { get; set; }
        public string Id { get; private set; }
        public string Name { get; private set; }
    }
}