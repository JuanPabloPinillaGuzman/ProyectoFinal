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
                .FirstOrDefaultAsync(cc => cc.IdVehicle == id) ?? throw new KeyNotFoundException($"Vehicle with id {id} was not found");
        }
    }
} 