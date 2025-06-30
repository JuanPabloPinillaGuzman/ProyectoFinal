using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime Expire { get; set; }
        public DateTime Created { get; set; }
        public DateTime Revoked { get; set; }
        public int UserId { get; set; }
    }
}