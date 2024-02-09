using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoApp_Public.Utils;
using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideoApp_Public.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserRepository userRepository;

        public RegisterController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            this.userRepository = userRepository;
        }

        // GET: Register
        public IActionResult Index()
        {
            //var dataContext = _context.Users.Include(u => u.CountryOfResidence);
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
	        [Bind("Id,CreatedAt,DeletedAt,Username,FirstName,LastName,Email,PwdHash,PwdSalt,Phone,IsConfirmed,SecurityToken,CountryOfResidenceId")] User user,
	        IFormCollection form)
        {
            // PwdSalt is manually assigned
	        ModelState.Remove("PwdSalt");
            // And we're storing the ID of the country rather than the country object
	        ModelState.Remove("CountryOfResidence");

            var existingUser = userRepository
                .GetUsers()
                .FirstOrDefault(
                    u => u.Username.Trim().ToUpper() == user.Username.Trim().ToUpper()
                         || u.Email.Trim().ToUpper() == user.Email.Trim().ToUpper());

            if (existingUser != null)
            {
                // The user with the existing username/email already exists
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
	            var salt = LoginUtils.GenerateSalt();
	            var pwdHash = LoginUtils.CreateHash(form["PwdHash"].ToString(), salt.Item1);

                user.PwdHash = pwdHash;
                user.PwdSalt = salt.Item2;
                user.CreatedAt = DateTime.Now;

                userRepository.CreateUser(user.CountryOfResidenceId, user);
                return Redirect("/");
            }

			return RedirectToAction(nameof(Index));
		}
    }
}
