using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Applications.Interfaces.Repositories
{
        public interface IUserRepository
        {
            Task<List<User>> GetAllUsersAsync();
            Task<User> GetByIdAsync(int id);
            Task<User> GetByUsernameAsync(string username);
            Task<User> AddAsync(User user);
            Task UpdateAsync(User user);
            Task DeleteAsync(User user);
    }
}
