using System.ComponentModel.DataAnnotations;

namespace TweetWebAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string LoginId { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] Salt { get; set; }

        [Required]
        [StringLength(10)]
        public string ContactNumber { get; set; }

        public ICollection<Tweet> Tweets { get; set; }
    }
}
