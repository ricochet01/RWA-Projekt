using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideosApp.Data;
using VideosApp.Dto;
using VideosApp.Interface;
using VideosApp.Model;
using VideosApp.Repository;

namespace VideosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public UserController(
            IUserRepository userRepository,
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = mapper.Map<List<UserDto>>(userRepository.GetUsers());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id)
        {
            if (!userRepository.UserExists(id))
            {
                return NotFound();
            }

            var user = mapper.Map<UserDto>(userRepository.GetUser(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromQuery] int countryId, [FromBody] UserDto userToCreate)
        {
            if (userToCreate == null) return BadRequest(ModelState);

            // Checking if a user with the existing username/email exists
            var user = userRepository
                .GetUsers()
                .FirstOrDefault(
                    u => u.Username.Trim().ToUpper() == userToCreate.Username.Trim().ToUpper() 
                 || u.Email.Trim().ToUpper() == userToCreate.Email.Trim().ToUpper());

            if (user != null)
            {
                ModelState.AddModelError("", "User with this username/email already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userMap = mapper.Map<User>(userToCreate);

            if (!userRepository.CreateUser(countryId, userMap))
            {
                ModelState.AddModelError("", "Something went wrong with creating the user.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int id, [FromQuery] int countryId, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null) return BadRequest(ModelState);
            if (id != updatedUser.Id) return BadRequest(ModelState);

            if (!userRepository.UserExists(id)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userMap = mapper.Map<User>(updatedUser);

            if (!userRepository.UpdateUser(countryId, userMap))
            {
                ModelState.AddModelError("", "Something went wrong with updating the user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
