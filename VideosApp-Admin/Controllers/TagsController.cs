using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp_Admin.Controllers
{
    public class TagsController : Controller
    {
        private readonly DataContext _context;
        private readonly ITagRepository tagRepository;

        public TagsController(DataContext context, ITagRepository tagRepository)
        {
            _context = context;
            this.tagRepository = tagRepository;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
              return _context.Tags != null ? 
                          View(await _context.Tags.ToListAsync()) :
                          Problem("Entity set 'DataContext.Tags'  is null.");
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
	            tagRepository.CreateTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || tagRepository == null)
            {
                return NotFound();
            }

            var tag = tagRepository.GetTag((int)id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
	                tagRepository.UpdateTag(tag);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tagRepository.TagExists(id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || tagRepository == null)
            {
                return NotFound();
            }

            var tag = tagRepository.GetTag((int)id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (tagRepository == null)
            {
                return Problem("Entity set 'DataContext.Tags'  is null.");
            }
            var tag = tagRepository.GetTag(id);
            if (tag != null)
            {
	            tagRepository.DeleteTag(tag);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
