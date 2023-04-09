
using static Backend.Domain.Enum.Enum;

namespace Backend.Domain.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Role role { get; set; }
        public string Username { get; set; }
        public UserDto WithoutPassword()
        {
            return new UserDto
            {
                Id = this.Id,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
                Email = this.Email,
                role = this.role,
                Username = this.Username,
                Token = this.Token
               
            };
        }
       
    }
    public class AuthenticateRequest
    {
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
