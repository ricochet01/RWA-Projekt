using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideosApp.Data;
using VideosApp.Model;

namespace VideoApp_Public.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            //var dataContext = _context.Users.Include(u => u.CountryOfResidence);
            return View();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Code");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,DeletedAt,Username,FirstName,LastName,Email,PwdHash,PwdSalt,Phone,IsConfirmed,SecurityToken,CountryOfResidenceId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Code", user.CountryOfResidenceId);
            return View(user);
        }
    }
}
