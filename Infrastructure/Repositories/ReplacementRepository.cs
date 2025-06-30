using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReplacementRepository : GenericRepository<Replacement>, IReplacementRepository
    {
        private readonly AutoTallerDbContext _context;
        public ReplacementRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Replacement> GetByIdAsync(int id)
        {
            return await _context.Replacement
                .FirstOrDefaultAsync(i => i.Id == id) ?? throw new KeyNotFoundException($"Replacement with id {id} was not found");
        }

        public override async Task<(int allRegisters, IEnumerable<Replacement> registers)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Replacement as IQueryable<Replacement>;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(r => 
                    EF.Functions.Like(r.Description.ToLower(), $"%{search.ToLower()}%") ||
                    EF.Functions.Like(r.Category.ToLower(), $"%{search.ToLower()}%") || 
                    EF.Functions.Like(r.MinimumStock.ToString(), $"%{search}%")
                );
            }

            var allRegisters = await query.CountAsync();

            var registers = await query
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (allRegisters, registers);
        }
    }
} 