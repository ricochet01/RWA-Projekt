using Microsoft.AspNetCore.Mvc;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Notification>))]
        public ActionResult<IEnumerable<Notification>> GetNotifications()
        {
            var notifications = notificationRepository.GetNotifications();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(notifications);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Notification))]
        [ProducesResponseType(400)]
        public IActionResult GetNotification(int id)
        {
            if (!notificationRepository.NotificationExists(id))
            {
                return NotFound();
            }

            var notification = notificationRepository.GetNotification(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(notification);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateNotification([FromBody] Notification notification)
        {
            if (notification == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!notificationRepository.CreateNotification(notification))
            {
                ModelState.AddModelError("", "Something went wrong with creating the notification.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
