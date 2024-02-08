using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideosApp.Data;
using VideosApp.Dto;
using VideosApp.Interface;
using VideosApp.Model;
using VideosApp_Admin.Paging;
using VideosApp_Admin.Utils;

namespace VideosApp_Admin.Controllers
{
    public class VideosController : Controller
    {
	    private readonly IVideoRepository videoRepository;
        private readonly ITagRepository tagRepository;
        private readonly DataContext _context;

        public VideosController(
            IVideoRepository videoRepository, ITagRepository tagRepository, DataContext context)
        {
            this.videoRepository = videoRepository;
            this.tagRepository = tagRepository;
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
        public IActionResult Details(int? id)
        {
            if (id == null || videoRepository == null)
            {
                return NotFound();
            }

            var video = videoRepository.GetAsyncVideos()
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefault(m => m.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            var tags = videoRepository.GetVideoTags(video.Id);
            var videoTags = new List<VideoTag>();

            foreach (var tag in tags)
            {
                videoTags.Add(new VideoTag()
                {
                    Tag = tag,
                    Video = video
                });
            }

            video.VideoTags = videoTags;

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
        public IActionResult Create(
	        [Bind("Id,CreatedAt,Name,Description,GenreId,TotalSeconds,StreamingUrl,ImageId,VideoTags")] Video video,
	        IFormCollection form, [FromForm(Name = "videoImage")] IFormFile file)
        {
            ModelState.Remove("Genre");
            ModelState.Remove("videoImage");

            if (ModelState.IsValid)
            {
                string tagText = form["VideoTags"].ToString();
                string[] splitTags = tagText.Split(',');

                foreach (var tag in splitTags)
                {
                    tagRepository.CreateTag(new() { Name = tag });
                }

                byte[] data = null;

                if (file != null)
                {
                    if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg")
                    {
                        data = ImageUtils.FileToByteArray(file);
                        var img = new Image()
                        {
                            Content = data
                        };

                        _context.Add(img);
                        _context.SaveChanges();
                    }
                }

                var dbTags = tagRepository.GetTags().Where(t => splitTags.Contains(t.Name)).ToArray();
                var tagIds = dbTags.Select(t => t.Id).ToArray();

                var imgId = 0;
                if (data != null)
                {
                    imgId = _context.Images.FirstOrDefault(i => Enumerable.SequenceEqual(i.Content, data)).Id;
                }

	            videoRepository.CreateVideo(video.GenreId, imgId, tagIds, video);
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", video.GenreId);
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

            var video = videoRepository.GetAsyncVideos()
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefault(m => m.Id == id);

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
        public IActionResult Edit(
            int id, [Bind("Id,CreatedAt,Name,Description,GenreId,TotalSeconds,StreamingUrl,ImageId")] Video video)
        {
            if (id != video.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Genre");

            if (ModelState.IsValid)
            {
                try
                {
                    var dbTags = videoRepository.GetVideoTags(video.Id);
                    var tagIds = dbTags.Select(t => t.Id).ToArray();

                    videoRepository.UpdateVideo(video.GenreId, (int)video.ImageId, tagIds, video);
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
