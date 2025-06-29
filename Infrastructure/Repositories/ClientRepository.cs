using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly AutoTallerDbContext _context;
        public ClientRepository(AutoTallerDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Client
                .FirstOrDefaultAsync(cc => cc.Id == id) ?? throw new KeyNotFoundException($"Client with id {id} was not found");
        }
    
    }
} 