using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountry(string name);
        Country GetUserCountry(int id);
        bool CountryExists(int id);

        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
    }
}
