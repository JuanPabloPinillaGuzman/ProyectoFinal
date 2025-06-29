using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AutoTallerDbContext _context;
        public RoleRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Role
                .FirstOrDefaultAsync(i => i.IdRole == id) ?? throw new KeyNotFoundException($"Role with id {id} was not found");
        }
    }
} 