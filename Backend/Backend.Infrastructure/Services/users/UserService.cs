using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services.users;
using Backend.Domain.AppException;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Infrastructure.Services.users 
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, IUserRepository userRepository, IPasswordHasher<User> passwordHasher,IConfiguration configuration)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<UserDto> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            var userDto = _mapper.Map<UserDto>(user);
            return userDto.WithoutPassword();
        }
        public string GenerateJwtToken(string username)
        {
            return GenerateToken(username);
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new AppException($"User with id {id} not found.", StatusCodes.Status404NotFound);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto registerDto)
        {
            if (registerDto == null)
                throw new AppException("Registration details are required");

            if (string.IsNullOrWhiteSpace(registerDto.password))
                throw new AppException("Password is required");

            if (registerDto.password != registerDto.ConfirmPassword)
                throw new AppException("Password and Confirm Password do not match");
            if (!CheckPasswordStrength(registerDto.password))
                throw new AppException("Password is not strong enough. Minimum password is 8, with at least one digit, upper and lower ,lower and any especial symbol must added ");
            var existingUser = await _userRepository.GetByUsernameAsync(registerDto.username);
            if (existingUser != null)
                throw new AppException($"Username '{registerDto.username}' is already taken");

            var user = _mapper.Map<User>(registerDto);
            _passwordHasher.CreatePasswordHash(registerDto.password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Token = GenerateToken(user.Username);

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDto>(user);
        }


       
        public async Task UpdateAsync(int id, ChangePasswordDto updateDto)
        {
        
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new AppException($"User not found. {StatusCodes.Status404NotFound}");
            if (!CheckPasswordStrength(updateDto.newPassword))
                throw new AppException("Password is not strong enough. Minimum password is 8, with at least one digit, upper and lower ,lower and any especial symbol must added ");

            if (!string.IsNullOrEmpty(updateDto.newPassword) && updateDto.newPassword == updateDto.confirmPassword)
            {
                if (VerifyPasswordHash(updateDto.oldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    byte[] passwordHash, passwordSalt;
                    _passwordHasher.CreatePasswordHash(updateDto.newPassword, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = updateDto.username;

                    await _userRepository.UpdateAsync(user);
                   
                }
                else
                {
                    throw new AppException("Old password is incorrect");
                }
            }
            else
            {
                throw new AppException("New password and confirm password do not match");
            }

        }



        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new AppException($"User not found . {StatusCodes.Status404NotFound}");

            await _userRepository.DeleteAsync(user);
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
            return true;
        }
        private string GenerateToken(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be null or empty", nameof(username));

            //var secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var secret = _configuration["JwtConfig:SecretKey"];
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];

            // Create a list of claims for the token
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username)
    };

            // Create a security key from the secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            // Create signing credentials using the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a new JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool CheckPasswordStrength(string password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 64;

            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < MIN_LENGTH || password.Length > MAX_LENGTH)
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(char.IsLower))
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return false;

            return true;
        }


    }

}
