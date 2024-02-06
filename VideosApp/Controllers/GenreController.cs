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
    public class GenreController : Controller
    {
        private readonly IGenreRepository genreRepository;
        private readonly IMapper mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            this.genreRepository = genreRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Genre>))]
        public ActionResult<IEnumerable<Genre>> GetGenres()
        {
            var genres = mapper.Map<List<GenreDto>>(genreRepository.GetGenres());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(genres);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Genre))]
        [ProducesResponseType(400)]
        public IActionResult GetGenre(int id)
        {
            if (!genreRepository.GenreExists(id))
            {
                return NotFound();
            }

            var genre = mapper.Map<GenreDto>(genreRepository.GetGenre(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(genre);
        }

        [HttpGet("Video/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Video>))]
        [ProducesResponseType(400)]
        public IActionResult GetVideosByGenre(int id)
        {
            if (!genreRepository.GenreExists(id))
            {
                return NotFound();
            }

            var videos = mapper.Map<List<VideoDto>>(genreRepository.GetVideosByGenre(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(videos);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGenre([FromBody] GenreDto genreToCreate)
        {
            if (genreToCreate == null) return BadRequest(ModelState);

            var genre = genreRepository
                .GetGenres()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == genreToCreate.Name.Trim().ToUpper());

            // The genre with the same name already exists!
            if (genre != null)
            {
                ModelState.AddModelError("", "Genre already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var genreMap = mapper.Map<Genre>(genreToCreate);
            if (!genreRepository.CreateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong with creating the genre.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateGenre(int id, [FromBody] GenreDto updatedGenre)
        {
            if (updatedGenre == null) return BadRequest(ModelState);
            if (id != updatedGenre.Id) return BadRequest(ModelState);

            if (!genreRepository.GenreExists(id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var genreMap = mapper.Map<Genre>(updatedGenre);

            if (!genreRepository.UpdateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong with updating the tag");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
