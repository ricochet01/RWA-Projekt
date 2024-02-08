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
using VideosApp_Admin.Paging;

namespace VideosApp_Admin.Controllers
{
    public class VideosController : Controller
    {
	    private readonly IVideoRepository videoRepository;
        private readonly DataContext _context;

        public VideosController(IVideoRepository videoRepository, DataContext context)
        {
            this.videoRepository = videoRepository;
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Initializing all URL parameters
            ViewData["CurrentSort"] = sortOrder;
			ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewData["CurrentFilter"] = searchString;

            // Applying genres and images to each video
			var videos = videoRepository.GetAsyncVideos()
	            .Include(v => v.Genre)
	            .Include(v => v.Image)
	            .OrderBy(v => v.Id);

			if (!string.IsNullOrEmpty(searchString))
			{
				// We have to order by id every time to turn it into an IOrderedQueryable
				videos = videos.Where(v 
                    => v.Name.Contains(searchString))
					.OrderBy(v => v.Id);
			}

			switch (sortOrder)
            {
	            case "name_desc":
		            videos = videos.OrderByDescending(v => v.Name);
		            break;
	            case "Genre":
		            videos = videos.OrderBy(v => v.Genre.Name);
		            break;
	            case "genre_desc":
		            videos = videos.OrderByDescending(v => v.Genre.Name);
		            break;
	            default:
		            videos = videos.OrderBy(v => v.Name);
		            break;
            }

			int pageSize = 2;
            int pageNum = (pageNumber ?? 1);

            if (pageNum <= 0) pageNum = 1;
			return View(await
	            PaginatedList<Video>.CreateAsync(videos, pageNum, pageSize));
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id");
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CreatedAt,Name,Description,GenreId,TotalSeconds,StreamingUrl,ImageId")] Video video)
        {
            if (ModelState.IsValid)
            {
	            // videoRepository.CreateVideo(video);
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", video.ImageId);
            return View(video);
        }

        // GET: Videos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || videoRepository == null)
            {
                return NotFound();
            }

            var video = videoRepository.GetVideo((int)id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", video.ImageId);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,Name,Description,GenreId,TotalSeconds,StreamingUrl,ImageId")] Video video)
        {
            if (id != video.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!videoRepository.VideoExists(id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", video.ImageId);
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || videoRepository == null)
            {
                return NotFound();
            }

            var video = await videoRepository.GetAsyncVideos()
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (videoRepository == null)
            {
                return Problem("Entity set 'DataContext.Videos'  is null.");
            }

            var video = videoRepository.GetVideo(id);

            if (video != null)
            {
	            videoRepository.DeleteVideo(video);
            }
            
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
