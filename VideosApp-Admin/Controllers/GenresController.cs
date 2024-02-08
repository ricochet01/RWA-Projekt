using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp_Admin.Controllers
{
    public class GenresController : Controller
    {
        private readonly DataContext _context;
        private readonly IGenreRepository genreRepository;

        public GenresController(DataContext context, IGenreRepository genreRepository)
        {
            _context = context;
            this.genreRepository = genreRepository;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
              return _context.Genres != null ? 
                          View(await _context.Genres.ToListAsync()) :
                          Problem("Entity set 'DataContext.Genres'  is null.");
        }

        // GET: Genres/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || genreRepository == null)
            {
                return NotFound();
            }

            var genre = genreRepository.GetGenre((int) id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description")] Genre genre)
        {
            if (ModelState.IsValid)
            {
	            genreRepository.CreateGenre(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || genreRepository == null)
            {
                return NotFound();
            }

            var genre = genreRepository.GetGenre((int) id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
	                genreRepository.UpdateGenre(genre);
                }
                catch (DbUpdateConcurrencyException)
                {
	                if (!genreRepository.GenreExists(id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = genreRepository.GetGenre((int)id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'DataContext.Genres'  is null.");
            }
            var genre = genreRepository.GetGenre((int)id);
            if (genre != null)
            {
	            genreRepository.DeleteGenre(genre);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
