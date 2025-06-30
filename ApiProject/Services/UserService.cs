using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiProject.Helpers;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ApiProject.Services;
using Microsoft.IdentityModel.Tokens;
using Application.DTOs;

namespace ApiProject.Services
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt,
            IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
            _passwordHasher = passwordHasher;
            _httpContextAccesor = httpContextAccessor;
        }

        public string GetCurrentUser()
        {
            var user = _httpContextAccesor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return "Anonymous";

            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "UnKnown";
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var usuario = new User
            {
                Name = registerDto.Name,
                LastName = registerDto.LastName,
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = registerDto.PasswordHash,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, registerDto.PasswordHash!);

            var UsuarioExiste = _unitOfWork.User
                .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                .FirstOrDefault();

            if (UsuarioExiste == null)
            {
                try
                {
                    _unitOfWork.User.Add(usuario);
                    await _unitOfWork.SaveAsync();

                    return $"The user {registerDto.Username} has been registered successfully.";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {
                return $"The user {registerDto.Username} already exists.";
            }
        }


        public async Task<DataUserDto> GetTokenAsync(LoginDto model)
        {
            DataUserDto dataUserDto = new DataUserDto();
            var user = await _unitOfWork.User
                .GetByUsernameAsync(model.Username);

            if (user == null)
            {
                dataUserDto.EstaAutenticado = false;
                dataUserDto.Mensaje = $"Doesn't exist a user with the username {model.Username}.";
                return dataUserDto;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (resultado == PasswordVerificationResult.Success)
            {
                dataUserDto.EstaAutenticado = true;
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
                dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                dataUserDto.UserName = user.Username;
                dataUserDto.Email = user.Email;
                dataUserDto.Roles = user.UserRoles
                    .Select(ur => ur.Role.Description)
                    .ToList();
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive).FirstOrDefault();
                    dataUserDto.RefreshToken = activeRefreshToken.Token;
                    dataUserDto.RefreshTokenExpiration = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = CreateRefreshToken(user.Id);
                    dataUserDto.RefreshToken = refreshToken.Token;
                    dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
                    user.RefreshTokens.Add(refreshToken);
                    _unitOfWork.User.Update(user);
                    await _unitOfWork.SaveAsync();
                }
                return dataUserDto;
            }

            dataUserDto.EstaAutenticado = false;
            dataUserDto.Mensaje = $"Incorrect credentials for user {model.Username}.";
            return dataUserDto;
        }

        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
            var usuario = await _unitOfWork.User.GetByUsernameAsync(model.Username);

            if (usuario == null)
            {
                return $"Doesn't exist a user with the username '{model.Username}'.";
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, model.Password);

            if (resultado != PasswordVerificationResult.Success)
            {
                return $"Incorrect credentials for user {model.Username}.";
            }

            var rolExiste = _unitOfWork.Role
                .Find(r => r.Description.ToLower() == model.Role.ToLower())
                .FirstOrDefault();

            if (rolExiste == null)
            {
                return $"The role '{model.Role}' doesn't exist.";
            }

            // Verifica si ya tiene el rol asignado
            var yaTieneRol = usuario.UserRoles.Any(ur => ur.IdRole == rolExiste.Id);

            if (yaTieneRol)
            {
                return $"The user {model.Username} already has the role '{model.Role}'.";
            }

            // Asignar el rol mediante la tabla intermedia
            var userRol = new UserRole
            {
                IdUser = usuario.Id,
                IdRole = rolExiste.Id
            };

            usuario.UserRoles ??= new List<UserRole>();
            usuario.UserRoles.Add(userRol);

            _unitOfWork.User.Update(usuario);
            await _unitOfWork.SaveAsync();

            return $"Role '{model.Role}' added to user '{model.Username}' successfully.";
        }

        private RefreshToken CreateRefreshToken(int userId)
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    UserId = userId,
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
        }

        public async Task<DataUserDto> RefreshTokenAsync(string refreshToken)
        {
            var datosUsuarioDto = new DataUserDto();

            var usuario = await _unitOfWork.User
                .GetByRefreshTokenAsync(refreshToken);

            if (usuario == null)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = $"The token doesn't have a user.";
                return datosUsuarioDto;
            }

            var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

            if (!refreshTokenBd.IsActive)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = $"The token isn't active.";
                return datosUsuarioDto;
            }

            //Revocamos el Refresh Token actual y
            refreshTokenBd.Revoked = DateTime.UtcNow;
            //generamos un nuevo Refresh Token y lo guardamos en la Base de Datos
            var newRefreshToken = CreateRefreshToken(usuario.Id);
            usuario.RefreshTokens.Add(newRefreshToken);
            _unitOfWork.User.Update(usuario);
            await _unitOfWork.SaveAsync();
            //Generamos un nuevo Json Web Token 😊
            datosUsuarioDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUsuarioDto.Email = usuario.Email;
            datosUsuarioDto.UserName = usuario.Username;
            datosUsuarioDto.Roles = usuario.UserRoles
                                                .Select(ur => ur.Role.Description)
                                                .ToList();
            datosUsuarioDto.RefreshToken = newRefreshToken.Token;
            datosUsuarioDto.RefreshTokenExpiration = newRefreshToken.Expires;
            return datosUsuarioDto;
        }
        
        private JwtSecurityToken CreateJwtToken(User usuario)
        {
            var roles = usuario.Roles;
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role.Description));
            }
            var claims = new[]
            {
                                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                                    new Claim("uid", usuario.Id.ToString())
                            }
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
       