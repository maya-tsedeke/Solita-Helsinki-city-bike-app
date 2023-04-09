using Microsoft.AspNetCore.Http;
namespace Backend.Domain.Response 
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthResponse(int id, string username, string token)
        {
            Id = id;
            Username = username;
            Token = token;
        }
    }
}
