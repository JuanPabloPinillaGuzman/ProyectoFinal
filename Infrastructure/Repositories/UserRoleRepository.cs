using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AutoTallerDbContext _context;
        public UserRoleRepository(AutoTallerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return await _context.UserRole.ToListAsync();
        }

        public async Task<UserRole?> GetByIdsAsync(int userId, int rolId)
        {
            return await _context.UserRole
                .FirstOrDefaultAsync(ur => ur.IdUser == userId && ur.IdRole == rolId)
                ?? throw new KeyNotFoundException($"UserRol with UserId {userId} and RolId {rolId} was not found");
        }

        public void Remove(UserRole entity)
        {
            _context.UserRole.Remove(entity);
        }

        public void Update(UserRole entity)
        {
            _context.UserRole.Update(entity);
        }
    }
} 