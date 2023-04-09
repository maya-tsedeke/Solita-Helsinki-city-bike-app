
using System.ComponentModel.DataAnnotations;

using static Backend.Domain.Enum.Enum;

namespace Backend.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Role role { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<Journey> Journeys { get; set; }

        
        public User() { }

    }
}
