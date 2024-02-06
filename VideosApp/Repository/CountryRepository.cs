using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext dataContext;

        public CountryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool CountryExists(int id)
            => dataContext.Countries.Any(c => c.Id == id);

        public bool CreateCountry(Country country)
        {
            dataContext.Add(country);

            return Save();
        }

        public bool UpdateCountry(Country country)
        {
            dataContext.Update(country);

            return Save();
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();

            return saved > 0;
        }

        public ICollection<Country> GetCountries()
            => dataContext.Countries.OrderBy(c => c.Id).ToList();

        public Country GetCountry(int id)
            => dataContext.Countries.FirstOrDefault(c => c.Id == id);

        public Country GetUserCountry(int userId)
            => dataContext.Users.Where(u => u.Id == userId).Select(u => u.CountryOfResidence).FirstOrDefault();

        public Country GetCountry(string name)
            => dataContext.Countries.FirstOrDefault(c => c.Name == name);
    }
}
