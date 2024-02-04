using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideosApp.Dto;
using VideosApp.Interface;
using VideosApp.Model;
using VideosApp.Repository;

namespace VideosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : Controller
    {
        private readonly IVideoRepository videoRepository;
        private readonly IMapper mapper;

        public VideoController(IVideoRepository videoRepository, IMapper mapper)
        {
            this.videoRepository = videoRepository;
            this.mapper = mapper;
        }

        // GET: api/Video
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Video>))]
        public ActionResult<IEnumerable<Video>> GetVideos()
        {
            var videos = mapper.Map<List<VideoDto>>(videoRepository.GetVideos());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(videos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Video))]
        [ProducesResponseType(400)]
        public IActionResult GetVideo(int id)
        {
            if (!videoRepository.VideoExists(id))
            {
                return NotFound();
            }

            var video = mapper.Map<VideoDto>(videoRepository.GetVideo(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(video);
        }
    }
}
