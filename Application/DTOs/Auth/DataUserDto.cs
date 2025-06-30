using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DataUserDto
    {
        public string? Mensaje { get; set; }
        public bool EstaAutenticado { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Rols { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}