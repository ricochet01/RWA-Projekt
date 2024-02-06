using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VideosApp.Dto;
using VideosApp.Interface;
using VideosApp.Model;
using VideosApp.Repository;

namespace VideosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {
            var countries = mapper.Map<List<CountryDto>>(countryRepository.GetCountries());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            if (!countryRepository.CountryExists(id))
            {
                return NotFound();
            }

            var country = mapper.Map<CountryDto>(countryRepository.GetCountry(id));

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("User/{userId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetUserCountry(int userId)
        {
            var country = mapper.Map<CountryDto>(countryRepository.GetUserCountry(userId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryToCreate)
        {
            if(countryToCreate == null) return BadRequest(ModelState);

            var country = countryRepository
                .GetCountries()
                .FirstOrDefault(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper());

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var countryMap = mapper.Map<Country>(countryToCreate);
            if (!countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong with creating the country.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int id, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null) return BadRequest(ModelState);
            if (id != updatedCountry.Id) return BadRequest(ModelState);

            if (!countryRepository.CountryExists(id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var countryMap = mapper.Map<Country>(updatedCountry);

            if (!countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong with updating the country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int id)
        {
            if (!countryRepository.CountryExists(id)) return NotFound();

            var countryToDelete = countryRepository.GetCountry(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong with deleting the country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
