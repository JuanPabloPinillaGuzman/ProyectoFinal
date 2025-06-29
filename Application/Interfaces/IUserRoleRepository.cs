using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAllAsync();
        void Remove(UserRole entity);
        void Update(UserRole entity);
        Task<UserRole?> GetByIdsAsync(int userId, int rolId);
    }
} 