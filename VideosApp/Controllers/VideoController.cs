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

        [HttpGet("Tag/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetTagsFromVideo(int id)
        {
            if (!videoRepository.VideoExists(id))
            {
                return NotFound();
            }

            var tags = mapper.Map<List<TagDto>>(videoRepository.GetVideoTags(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateVideo([FromQuery] int genreId, [FromQuery] int imageId,
            [FromQuery] string tagIds, [FromBody] VideoDto videoToCreate)
        {
            if (videoToCreate == null) return BadRequest(ModelState);

            var video = videoRepository
                .GetVideos()
                .FirstOrDefault(c =>
                    c.Name.Trim().ToUpper() == videoToCreate.Name.Trim().ToUpper() 
                 || c.StreamingUrl.Trim().ToUpper() == videoToCreate.StreamingUrl.Trim().ToUpper());
              

            if (video != null)
            {
                ModelState.AddModelError("", "Video already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var videoMap = mapper.Map<Video>(videoToCreate);
            int[] ids = tagIds.Split(',').Select(int.Parse).ToArray();

            if (!videoRepository.CreateVideo(genreId, imageId, ids, videoMap))
            {
                ModelState.AddModelError("", "Something went wrong with creating the video.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVideo(int id, [FromQuery] int genreId, [FromQuery] int imageId,
            [FromQuery] string tagIds, [FromBody] VideoDto updatedVideo)
        {
            if (updatedVideo == null) return BadRequest(ModelState);
            if (id != updatedVideo.Id) return BadRequest(ModelState);

            if (!videoRepository.VideoExists(id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tagMap = mapper.Map<Video>(updatedVideo);
            int[] ids = tagIds.Split(',').Select(int.Parse).ToArray();

            if (!videoRepository.UpdateVideo(genreId, imageId, ids, tagMap))
            {
                ModelState.AddModelError("", "Something went wrong with updating the tag");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
