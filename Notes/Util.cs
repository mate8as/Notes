using System.ComponentModel.DataAnnotations;

namespace Notes
{
    public class LoginModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class Util
    {
    }
}
