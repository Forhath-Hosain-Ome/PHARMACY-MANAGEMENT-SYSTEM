using PharmacySystem.Core.Entities.Base;

namespace PharmacySystem.Core.Interfaces;

/// <summary>
/// Generic repository interface for data access operations
/// </summary>
/// <typeparam name="T">Entity type that inherits from Entity base class</typeparam>
public interface IRepository<T> where T : Entity
{
    /// <summary>
    /// Gets an entity by its ID asynchronously
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all entities asynchronously
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity asynchronously
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity asynchronously
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by ID asynchronously
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Checks if an entity exists by ID asynchronously
    /// </summary>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Saves all changes to the database asynchronously
    /// </summary>
    Task<int> SaveChangesAsync();
}
