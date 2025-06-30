using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<DataUserDto> LoginAsync(LoginDto dto);
        Task<string> AddRoleAsync(AddRoleDto dto);
        Task<DataUserDto> RefreshTokenAsync(string refreshToken);
    }
}