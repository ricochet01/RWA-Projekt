using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideosApp.Dto;
using VideosApp.Interface;
using VideosApp.Model;

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
    }
}
