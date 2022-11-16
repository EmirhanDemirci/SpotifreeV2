using System.ComponentModel.DataAnnotations;

namespace Spotifree.UserService.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        public string Gender { get; set; }

        public byte[] ProfilePicture { get; set; }

        public string Description { get; set; }
    }
}