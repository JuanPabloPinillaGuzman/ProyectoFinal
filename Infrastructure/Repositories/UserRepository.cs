using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AutoTallerDbContext _context;
        public UserRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.User
                .FirstOrDefaultAsync(cc => cc.Id == id) ?? throw new KeyNotFoundException($"User with id {id} was not found");
        }
        
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.User
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                            .Include(u => u.RefreshTokens)
                            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.User
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                            .Include(u => u.RefreshTokens)
                            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
        }
    }
} 