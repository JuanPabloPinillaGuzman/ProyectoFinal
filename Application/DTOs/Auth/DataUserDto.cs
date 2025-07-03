using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DataUserDto
    {
        public int Id { get; set; }
        public string  Name      { get; set; } = "";
        public string  LastName  { get; set; } = "";
        
        public bool    IsActive  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // New
        [JsonPropertyName("message")]
        public string? Mensaje { get; set; }
        [JsonPropertyName("isAuthenticated")]
        public bool EstaAutenticado { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Rols { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}