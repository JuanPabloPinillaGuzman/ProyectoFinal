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
                .Include(so => so.Vehicle)
                    .ThenInclude(v => v.Client)
                .Include(so => so.State)
                .Include(so => so.User)
                .FirstOrDefaultAsync(i => i.Id == id) ?? throw new KeyNotFoundException($"ServiceOrder with id {id} was not found");
        }

         public override async Task<(int allRegisters, IEnumerable<ServiceOrder> registers)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.ServiceOrder
                .Include(so => so.Vehicle)
                    .ThenInclude(v => v.Client)
                .Include(so => so.State)
                .Include(so => so.User)
                .AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(so => 
                    so.Vehicle.Client.Name.ToLower().Contains(searchLower) ||
                    so.State.StateType.ToLower().Contains(searchLower) ||
                    so.User.Name.ToLower().Contains(searchLower)
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