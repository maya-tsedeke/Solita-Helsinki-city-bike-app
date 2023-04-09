
using static Backend.Domain.Enum.Enum;

namespace Backend.Domain.DTOs
{
    public class RegisterUserDto
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string ConfirmPassword { get; set; }
        public string username { get; set; }
        public Role role { get; set; }
    }
    public class ChangePasswordDto
    {
        public string username { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
        public string oldPassword { get; set; }
    }

}
