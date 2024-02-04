using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideosApp.Dto;
using VideosApp.Interface;
using VideosApp.Model;

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
    }
}
