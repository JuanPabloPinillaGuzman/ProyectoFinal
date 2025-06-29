using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ServiceTypeRepository : GenericRepository<ServiceType>, IServiceTypeRepository
    {
        private readonly AutoTallerDbContext _context;
        public ServiceTypeRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ServiceType> GetByIdAsync(int id)
        {
            return await _context.ServiceType
                .FirstOrDefaultAsync(cc => cc.Id == id) ?? throw new KeyNotFoundException($"ServiceType with id {id} was not found");
        }
    }
} 