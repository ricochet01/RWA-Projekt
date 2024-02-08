using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using VideoApp_Public.Models;
using VideoApp_Public.Utils;
using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideoApp_Public.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository userRepository;

        public LoginController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
	            return RedirectToAction(nameof(Index));
            }

            var username = login.Username;
            var password = login.Password;

            if (userRepository.UserExists(username))
            {
	            var user = userRepository.GetUser(username);

	            string pwdHash = LoginUtils.CreateHash(password, Convert.FromBase64String(user.PwdSalt));
	            if (userRepository.UserExists(username, pwdHash))
	            {
                    // Add cookie
		            var claims = new List<Claim> { new (ClaimTypes.Name, user.Username) };
		            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		            HttpContext.SignInAsync(
			            CookieAuthenticationDefaults.AuthenticationScheme,
			            new ClaimsPrincipal(claimsIdentity),
			            new AuthenticationProperties()).Wait();

		            return Redirect("/");
				}
            }

			return RedirectToAction(nameof(Index));
		}

        [HttpGet]
		public async Task<IActionResult> Logout()
        {
	        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

			return Redirect("/");
        }
    }
}
