using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly AutoTallerDbContext _context;
        public VehicleRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }
        
        public override async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _context.Vehicle
                .FirstOrDefaultAsync(cc => cc.Id == id) ?? throw new KeyNotFoundException($"Vehicle with id {id} was not found");
        }

        public override async Task<(int allRegisters, IEnumerable<Vehicle> registers)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Vehicle as IQueryable<Vehicle>;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(v => 
                    EF.Functions.Like(v.Client.Name.ToLower(), $"%{search.ToLower()}%") ||
                    EF.Functions.Like(v.SerialNumberVIN.ToLower(), $"%{search.ToLower()}%")
                );
            }

            var allRegisters = await query.CountAsync();

            var registers = await query
                                    .Include(v => v.Client)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (allRegisters, registers);
        }
    }
} 