using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InventoryDetailRepository : GenericRepository<InventoryDetail>, IInventoryDetailRepository
    {
        private readonly AutoTallerDbContext _context;
        public InventoryDetailRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }
        
    }
} 