using Backend.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Applications.Interfaces.Services.users
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateAsync(string username, string password);
        Task<List<UserDto>> GetAllUsersAsync();
        string GenerateJwtToken(string username);
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> RegisterAsync(RegisterUserDto userDto);
        Task UpdateAsync(int id, ChangePasswordDto updateDto);
        Task DeleteAsync(int id);
    }

}
