using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using VideoApp_Public.Models;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideoApp_Public.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVideoRepository videoRepository;

        public HomeController(ILogger<HomeController> logger, IVideoRepository videoRepository)
        {
            _logger = logger;
            this.videoRepository = videoRepository;
        }

        public IActionResult Index(string searchString)
        {
	        ViewData["CurrentFilter"] = searchString;

	        var videos = videoRepository.GetAsyncVideos()
		        .Include(v => v.Genre)
		        .Include(v => v.Image)
		        .OrderBy(v => v.CreatedAt);

			if (!string.IsNullOrEmpty(searchString))
	        {
		        // We have to order by id every time to turn it into an IOrderedQueryable
		        videos = videos.Where(v
				        => v.Name.Contains(searchString))
			        .OrderBy(v => v.Id);
	        }

			return View(videos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            var video = videoRepository.GetAsyncVideos()
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefault(v => v.Id == id);

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

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}