namespace PharmacySystem.Core.Entities.Base;

/// <summary>
/// Base class for all entities in the system
/// Provides common properties like Id, CreatedAt, and UpdatedAt
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public int Id { get; protected set; }

    /// <summary>
    /// Timestamp when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Timestamp when the entity was last updated
    /// </summary>
    public DateTime UpdatedAt { get; protected set; }

    /// <summary>
    /// Protected constructor for Entity Framework Core
    /// </summary>
    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the UpdatedAt timestamp
    /// </summary>
    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the entity's ID
    /// </summary>
    /// <returns>The entity ID</returns>
    public int GetId()
    {
        return Id;
    }

    /// <summary>
    /// Determines whether the specified entity is equal to the current entity
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == 0 || other.Id == 0)
            return false;

        return Id == other.Id;
    }

    /// <summary>
    /// Gets the hash code for this entity
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
