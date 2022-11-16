using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Spotifree.AuthService.Models
{
    public class Credentials
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public class MessageContent
    {
        public Credentials credentials { get; set; }
    }
}
