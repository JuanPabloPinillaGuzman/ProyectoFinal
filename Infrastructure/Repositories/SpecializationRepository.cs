using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        private readonly AutoTallerDbContext _context;
        public SpecializationRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Specialization> GetByIdAsync(int id)
        {
            return await _context.Specialization
                .FirstOrDefaultAsync(cc => cc.Id == id) ?? throw new KeyNotFoundException($"Specialization with id {id} was not found");
        }
    }
} 