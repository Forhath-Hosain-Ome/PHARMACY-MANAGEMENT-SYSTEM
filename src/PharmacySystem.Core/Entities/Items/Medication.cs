using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;
using PharmacySystem.Core.Entities.Operations;

namespace PharmacySystem.Core.Entities.Items;

/// <summary>
/// Abstract class representing a medication
/// Inherits from Item and implements IInventoryItem and ISearchable
/// Demonstrates constructor overloading and copy constructor
/// </summary>
public abstract class Medication : Item, IInventoryItem, ISearchable
{
    /// <summary>
    /// Generic name of the medication
    /// </summary>
    public string GenericName { get; protected set; } = string.Empty;

    /// <summary>
    /// Manufacturer of the medication
    /// </summary>
    public string? Manufacturer { get; protected set; }

    /// <summary>
    /// Expiry date of the medication
    /// </summary>
    public DateTime ExpiryDate { get; protected set; }

    /// <summary>
    /// Category of the medication
    /// </summary>
    public MedicationCategory Category { get; protected set; }

    /// <summary>
    /// Batch number
    /// </summary>
    public string? BatchNumber { get; protected set; }

    /// <summary>
    /// Navigation property for inventory (1:1 relationship)
    /// </summary>
    public virtual Inventory? Inventory { get; protected set; }

    /// <summary>
    /// Protected constructor for Entity Framework Core
    /// </summary>
    protected Medication() : base()
    {
    }

    /// <summary>
    /// Constructor with minimal information (Constructor Overloading - 1)
    /// </summary>
    public Medication(string name, Money price)
        : base(name, price)
    {
        GenericName = name; // Default to same as name
        Category = MedicationCategory.Other;
        ExpiryDate = DateTime.Now.AddYears(2); // Default 2 years
    }

    /// <summary>
    /// Constructor with generic name (Constructor Overloading - 2)
    /// </summary>
    public Medication(string name, string genericName, Money price)
        : base(name, price)
    {
        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name cannot be empty", nameof(genericName));

        GenericName = genericName;
        Category = MedicationCategory.Other;
        ExpiryDate = DateTime.Now.AddYears(2);
    }

    /// <summary>
    /// Constructor with category (Constructor Overloading - 3)
    /// </summary>
    public Medication(string name, string genericName, Money price, MedicationCategory category)
        : base(name, price)
    {
        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name cannot be empty", nameof(genericName));

        GenericName = genericName;
        Category = category;
        ExpiryDate = DateTime.Now.AddYears(2);
    }

    /// <summary>
    /// Constructor with manufacturer and expiry date (Constructor Overloading - 4)
    /// </summary>
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

    /// <summary>
    /// Full constructor with all details (Constructor Overloading - 5)
    /// </summary>
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

    /// <summary>
    /// Copy constructor - creates a deep copy of a medication (Copy Constructor)
    /// Useful for creating similar medications or templates
    /// </summary>
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

    /// <summary>
    /// Checks if the medication is expired
    /// </summary>
    public bool IsExpired()
    {
        return DateTime.Now > ExpiryDate;
    }

    /// <summary>
    /// Gets days until expiry
    /// </summary>
    public int GetDaysUntilExpiry()
    {
        return (ExpiryDate - DateTime.Now).Days;
    }

    /// <summary>
    /// Checks if medication is expiring soon (within 90 days)
    /// </summary>
    public bool IsExpiringSoon()
    {
        return GetDaysUntilExpiry() <= 90 && GetDaysUntilExpiry() > 0;
    }

    /// <summary>
    /// Updates medication information
    /// </summary>
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

    /// <summary>
    /// Gets detailed medication information
    /// </summary>
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

    /// <summary>
    /// Gets current stock from inventory
    /// </summary>
    public int CurrentStock => Inventory?.CurrentQuantity ?? 0;

    /// <summary>
    /// Gets reorder level from inventory
    /// </summary>
    public int ReorderLevel => Inventory?.ReorderLevel ?? 50;

    /// <summary>
    /// Checks if stock is low
    /// </summary>
    public bool IsLowStock()
    {
        return Inventory != null && Inventory.IsLowStock();
    }

    /// <summary>
    /// Checks if reorder is required
    /// </summary>
    public bool RequiresReorder()
    {
        return Inventory != null && Inventory.RequiresReorder();
    }

    /// <summary>
    /// Updates stock quantity
    /// </summary>
    public void UpdateStock(int quantity)
    {
        if (Inventory == null)
            throw new InvalidOperationException("No inventory record exists for this medication");

        Inventory.UpdateQuantity(quantity);
        UpdateTimestamp();
    }

    #endregion

    #region ISearchable Implementation

    /// <summary>
    /// Gets all searchable text for this medication
    /// </summary>
    public string GetSearchableText()
    {
        return $"{Name} {GenericName} {Manufacturer} {Category} {BatchNumber} {SKU}".ToLower();
    }

    /// <summary>
    /// Checks if medication matches the search term
    /// </summary>
    public bool MatchesSearch(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        var lowerSearch = searchTerm.ToLower();
        return GetSearchableText().Contains(lowerSearch);
    }

    #endregion
}
