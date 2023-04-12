using AutoMapper;
using Backend.Applications.Interfaces.Services.users;
using Backend.Domain.AppException;
using Backend.Domain.DTOs;
using Backend.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest request)
        {
            try
            {
                var userDto = await _userService.AuthenticateAsync(request.Username, request.Password);

                if (userDto == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                var token = _userService.GenerateJwtToken(userDto.Username);
                var loginResponse = new AuthResponse(userDto.Id, userDto.Username, token);
                return Ok(loginResponse);
            }
            catch (Exception)
            {

                throw; // Rethrow the exception so it can be caught by the calling method
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserDto request)
        {
            var userDto = _mapper.Map<UserDto>(request);

            try
            {
                userDto = await _userService.RegisterAsync(request);
                return Ok(userDto);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, ChangePasswordDto changePasswordDto)
        {
            try
            {
                await _userService.UpdateAsync(id, changePasswordDto);
                return Ok(new { message = "Successfully updated password." });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok(new { message = "Successfully deleted user." });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var userDto = await _userService.GetByIdAsync(id);
                return Ok(userDto);
            }
            catch (AppException ex)
            {
                if (ex.StatusCode == StatusCodes.Status404NotFound)
                {
                    return NotFound(new { message = ex.Message });
                }
                else if (ex.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    return Unauthorized(new { message = "Access is unauthorized" });
                }
                else
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

    }
}
