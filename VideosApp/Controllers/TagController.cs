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
    public class TagController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IMapper mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            this.tagRepository = tagRepository;
            this.mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public ActionResult<IEnumerable<Tag>> GetTags()
        {
            var tags = mapper.Map<List<TagDto>>(tagRepository.GetTags());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTag(int id)
        {
            if (!tagRepository.TagExists(id))
            {
                return NotFound();
            }

            var tag = mapper.Map<TagDto>(tagRepository.GetTag(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tag);
        }

        [HttpGet("Video/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Video>))]
        [ProducesResponseType(400)]
        public IActionResult GetVideosByTag(int id)
        {
            if (!tagRepository.TagExists(id))
            {
                return NotFound();
            }

            var videos = mapper.Map<List<VideoDto>>(tagRepository.GetVideosByTag(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(videos);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTag([FromBody] TagDto tagToCreate)
        {
            if (tagToCreate == null) return BadRequest(ModelState);

            var tag = tagRepository
                .GetTags()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == tagToCreate.Name.Trim().ToUpper());

            if (tag != null)
            {
                ModelState.AddModelError("", "Tag already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tagMap = mapper.Map<Tag>(tagToCreate);
            if (!tagRepository.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong with creating the tag.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTag(int id, [FromBody] TagDto updatedTag)
        {
            if (updatedTag == null) return BadRequest(ModelState);
            if (id != updatedTag.Id) return BadRequest(ModelState);

            if (!tagRepository.TagExists(id)) return NotFound();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var tagMap = mapper.Map<Tag>(updatedTag);

            if (!tagRepository.UpdateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong with updating the tag");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(int id)
        {
            if (!tagRepository.TagExists(id)) return NotFound();

            var tagToDelete = tagRepository.GetTag(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!tagRepository.DeleteTag(tagToDelete))
            {
                ModelState.AddModelError("", "Something went wrong with deleting the tag");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
