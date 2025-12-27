using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;
using PharmacySystem.Core.Entities.Operations;

namespace PharmacySystem.Core.Entities.Items;

public abstract class Medication : Item, IInventoryItem, ISearchable
{
    public string GenericName { get; protected set; } = string.Empty;
    public string? Manufacturer { get; protected set; }
    public DateTime ExpiryDate { get; protected set; }
    public MedicationCategory Category { get; protected set; }
    public string? BatchNumber { get; protected set; }
    public virtual Inventory? Inventory { get; protected set; }

    protected Medication() : base()
    {
    }

    public Medication(string name, Money price)
        : base(name, price)
    {
        GenericName = name; // Default to same as name
        Category = MedicationCategory.Other;
        ExpiryDate = DateTime.Now.AddYears(2); // Default 2 years
    }

    public Medication(string name, string genericName, Money price)
        : base(name, price)
    {
        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name cannot be empty", nameof(genericName));

        GenericName = genericName;
        Category = MedicationCategory.Other;
        ExpiryDate = DateTime.Now.AddYears(2);
    }

    public Medication(string name, string genericName, Money price, MedicationCategory category)
        : base(name, price)
    {
        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name cannot be empty", nameof(genericName));

        GenericName = genericName;
        Category = category;
        ExpiryDate = DateTime.Now.AddYears(2);
    }

    public Medication(string name, string genericName, Money price,
                     MedicationCategory category, string manufacturer, DateTime expiryDate)
        : base(name, price)
    {
        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name cannot be empty", nameof(genericName));

        if (expiryDate <= DateTime.Now)
            throw new ArgumentException("Expiry date must be in the future", nameof(expiryDate));

        GenericName = genericName;
        Category = category;
        Manufacturer = manufacturer;
        ExpiryDate = expiryDate;
    }

    public Medication(string name, string genericName, string description, Money price,
                     MedicationCategory category, string manufacturer, DateTime expiryDate,
                     string batchNumber)
        : base(name, description, price)
    {
        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name cannot be empty", nameof(genericName));

        if (expiryDate <= DateTime.Now)
            throw new ArgumentException("Expiry date must be in the future", nameof(expiryDate));

        GenericName = genericName;
        Category = category;
        Manufacturer = manufacturer;
        ExpiryDate = expiryDate;
        BatchNumber = batchNumber;
    }

    public Medication(Medication other) : base()
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // Copy all properties except Id (new entity)
        Name = other.Name;
        GenericName = other.GenericName;
        Description = other.Description;
        Price = new Money(other.Price.Amount, other.Price.Currency);
        Category = other.Category;
        Manufacturer = other.Manufacturer;
        ExpiryDate = other.ExpiryDate;
        BatchNumber = other.BatchNumber;
        SKU = GenerateSKU(); // Generate new SKU
    }

    public bool IsExpired()
    {
        return DateTime.Now > ExpiryDate;
    }

    public int GetDaysUntilExpiry()
    {
        return (ExpiryDate - DateTime.Now).Days;
    }

    public bool IsExpiringSoon()
    {
        return GetDaysUntilExpiry() <= 90 && GetDaysUntilExpiry() > 0;
    }

    public virtual void UpdateMedicationInfo(string? manufacturer, DateTime? expiryDate, string? batchNumber)
    {
        if (expiryDate.HasValue && expiryDate.Value <= DateTime.Now)
            throw new ArgumentException("Expiry date must be in the future");

        if (manufacturer != null)
            Manufacturer = manufacturer;

        if (expiryDate.HasValue)
            ExpiryDate = expiryDate.Value;

        if (batchNumber != null)
            BatchNumber = batchNumber;

        UpdateTimestamp();
    }

    public string GetMedicationInfo()
    {
        return $"Name: {Name} ({GenericName})\n" +
               $"Category: {Category}\n" +
               $"Manufacturer: {Manufacturer ?? "Unknown"}\n" +
               $"Price: {Price.ToDisplayString()}\n" +
               $"Expiry Date: {ExpiryDate:yyyy-MM-dd}\n" +
               $"Days Until Expiry: {GetDaysUntilExpiry()}\n" +
               $"Status: {(IsExpired() ? "EXPIRED" : IsExpiringSoon() ? "Expiring Soon" : "Valid")}\n" +
               $"Batch: {BatchNumber ?? "N/A"}\n" +
               $"SKU: {SKU}";
    }

    #region IInventoryItem Implementation

    public int CurrentStock => Inventory?.CurrentQuantity ?? 0;
    public int ReorderLevel => Inventory?.ReorderLevel ?? 50;

    public bool IsLowStock()
    {
        return Inventory != null && Inventory.IsLowStock();
    }

    public bool RequiresReorder()
    {
        return Inventory != null && Inventory.RequiresReorder();
    }

    public void UpdateStock(int quantity)
    {
        if (Inventory == null)
            throw new InvalidOperationException("No inventory record exists for this medication");

        Inventory.UpdateQuantity(quantity);
        UpdateTimestamp();
    }

    #endregion

    #region ISearchable Implementation

    public string GetSearchableText()
    {
        return $"{Name} {GenericName} {Manufacturer} {Category} {BatchNumber} {SKU}".ToLower();
    }
    
    public bool MatchesSearch(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        var lowerSearch = searchTerm.ToLower();
        return GetSearchableText().Contains(lowerSearch);
    }

    #endregion
}
