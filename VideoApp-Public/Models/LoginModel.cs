using System.ComponentModel.DataAnnotations;

namespace VideoApp_Public.Models
{
	public class LoginModel
	{
		[Required]
		public string Username { get; set; }

		[Required] public string Password { get; set; }
	}
}
