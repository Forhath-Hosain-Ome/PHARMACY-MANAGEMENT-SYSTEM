using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.ValueObjects;

namespace PharmacySystem.Core.Entities.Items;

public abstract class Item : Entity
{
    public string Name { get; protected set; } = string.Empty;
    public string? Description { get; protected set; }
    public Money Price { get; protected set; } = new Money(0);
    public string SKU { get; protected set; } = string.Empty;

    protected Item()
    {
    }

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

    protected Item(string name, string description, Money price)
        : this(name, price)
    {
        Description = description;
    }

    public virtual void UpdatePrice(Money newPrice)
    {
        if (newPrice == null)
            throw new ArgumentNullException(nameof(newPrice));

        if (newPrice.Amount < 0)
            throw new ArgumentException("Price cannot be negative");

        Price = newPrice;
        UpdateTimestamp();
    }

    public virtual void UpdateDescription(string description)
    {
        Description = description;
        UpdateTimestamp();
    }

    public virtual string GetItemInfo()
    {
        return $"{Name} - {Price.ToDisplayString()}\n{Description ?? "No description"}";
    }

    protected virtual string GenerateSKU()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var namePrefix = Name.Length >= 3 ? Name.Substring(0, 3).ToUpper() : Name.ToUpper();
        return $"{namePrefix}-{timestamp}";
    }

    public virtual void UpdateSKU(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU cannot be empty", nameof(sku));

        SKU = sku;
        UpdateTimestamp();
    }
}
