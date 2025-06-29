using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly AutoTallerDbContext _context;
        public InventoryRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        } 
        
        public override async Task<Inventory> GetByIdAsync(int id)
        {
            return await _context.Inventory
                .FirstOrDefaultAsync(cc => cc.Id == id) ?? throw new KeyNotFoundException($"Inventory with id {id} was not found");
        }
    }
} 