using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.ValueObjects;

namespace PharmacySystem.Core.Entities.Items;

/// <summary>
/// Abstract base class for all items in the pharmacy
/// Inherits from Entity
/// </summary>
public abstract class Item : Entity
{
    /// <summary>
    /// Item name
    /// </summary>
    public string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// Item description
    /// </summary>
    public string? Description { get; protected set; }

    /// <summary>
    /// Item price
    /// </summary>
    public Money Price { get; protected set; } = new Money(0);

    /// <summary>
    /// Stock Keeping Unit identifier
    /// </summary>
    public string SKU { get; protected set; } = string.Empty;

    /// <summary>
    /// Protected constructor for Entity Framework Core
    /// </summary>
    protected Item()
    {
    }

    /// <summary>
    /// Protected constructor with basic item information (Constructor Overloading - 1)
    /// </summary>
    protected Item(string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (price == null)
            throw new ArgumentNullException(nameof(price));

        Name = name;
        Price = price;
        SKU = GenerateSKU();
    }

    /// <summary>
    /// Protected constructor with description (Constructor Overloading - 2)
    /// </summary>
    protected Item(string name, string description, Money price)
        : this(name, price)
    {
        Description = description;
    }

    /// <summary>
    /// Updates the item price
    /// </summary>
    public virtual void UpdatePrice(Money newPrice)
    {
        if (newPrice == null)
            throw new ArgumentNullException(nameof(newPrice));

        if (newPrice.Amount < 0)
            throw new ArgumentException("Price cannot be negative");

        Price = newPrice;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates the item description
    /// </summary>
    public virtual void UpdateDescription(string description)
    {
        Description = description;
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets basic item information
    /// </summary>
    public virtual string GetItemInfo()
    {
        return $"{Name} - {Price.ToDisplayString()}\n{Description ?? "No description"}";
    }

    /// <summary>
    /// Generates a unique SKU for the item
    /// </summary>
    protected virtual string GenerateSKU()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var namePrefix = Name.Length >= 3 ? Name.Substring(0, 3).ToUpper() : Name.ToUpper();
        return $"{namePrefix}-{timestamp}";
    }

    /// <summary>
    /// Updates the SKU
    /// </summary>
    public virtual void UpdateSKU(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU cannot be empty", nameof(sku));

        SKU = sku;
        UpdateTimestamp();
    }
}
