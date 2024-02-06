using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext; 
        }

        public ICollection<User> GetUsers()
            => dataContext.Users.OrderBy(u => u.Id).Where(u => u.DeletedAt == null).ToList();
        

        public User GetUser(int id)
            => dataContext.Users.FirstOrDefault(u => u.Id == id && u.DeletedAt == null);

        public User GetUser(string username)
            => dataContext.Users.FirstOrDefault(u => u.Username == username && u.DeletedAt == null);

        public bool UserExists(int id)
            => dataContext.Users.Any(u => u.Id == id && u.DeletedAt == null);

        public bool CreateUser(int countryId, User user)
        {
            var country = dataContext.Countries.FirstOrDefault(c => c.Id == countryId);

            user.CountryOfResidence = country;
            user.CountryOfResidenceId = countryId;
            dataContext.Add(user);

            return Save();
        }

        public bool UpdateUser(int countryId, User user)
        {
            var country = dataContext.Countries.FirstOrDefault(c => c.Id == countryId);
            user.CountryOfResidence = country;
            user.CountryOfResidenceId = countryId;

            dataContext.Update(user);

            return Save();
        }

        public bool DeleteUser(User user)
        {
            user.DeletedAt = DateTime.Now;
            // "Soft delete" of a user
            dataContext.Update(user);

            return Save();
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();

            return saved > 0;
        }
    }
}
