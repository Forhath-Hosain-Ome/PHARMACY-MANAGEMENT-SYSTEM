using PharmacySystem.Core.Entities.Base;

namespace PharmacySystem.Core.Interfaces;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> SaveChangesAsync();
}
