using Domain.Entities;

namespace Application.Interfaces
{
    public interface IServiceOrderRepository : IGenericRepository<ServiceOrder>
    {
         Task<bool> GetOrdersByVehicleAsync(int idVehicle);
        Task<IEnumerable<ServiceOrder>> GetOrdersByClientAsync(int idClient);
    }
} 