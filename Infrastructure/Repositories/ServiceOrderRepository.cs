using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ServiceOrderRepository : GenericRepository<ServiceOrder>, IServiceOrderRepository
    {
        private readonly AutoTallerDbContext _context;
        public ServiceOrderRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ServiceOrder> GetByIdAsync(int id)
        {
            return await _context.ServiceOrder
                .FirstOrDefaultAsync(i => i.IdOrder == id) ?? throw new KeyNotFoundException($"ServiceOrde with id {id} was not found");
        }
    }
} 