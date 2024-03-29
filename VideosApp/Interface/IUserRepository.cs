﻿using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string username);
        bool UserExists(int id);
        bool UserExists(string username);
        bool UserExists(string username, string password);

        bool CreateUser(int countryId, User user);
        bool UpdateUser(int countryId, User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
