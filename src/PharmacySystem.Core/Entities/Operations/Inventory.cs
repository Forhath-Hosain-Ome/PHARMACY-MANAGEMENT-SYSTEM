using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.Items;

namespace PharmacySystem.Core.Entities.Operations;

/// <summary>
/// Represents inventory tracking for medications
/// Demonstrates operator overloading for stock operations
/// </summary>
public class Inventory : Entity
{
    /// <summary>
    /// Foreign key to Medication
    /// </summary>
    public int MedicationId { get; private set; }

    /// <summary>
    /// Navigation property to Medication (1:1 relationship)
    /// </summary>
    public virtual Medication? Medication { get; private set; }

    /// <summary>
    /// Current quantity in stock
    /// </summary>
    public int CurrentQuantity { get; private set; }

    /// <summary>
    /// Reorder level threshold
    /// </summary>
    public int ReorderLevel { get; private set; }

    /// <summary>
    /// Quantity to reorder when threshold is reached
    /// </summary>
    public int ReorderQuantity { get; private set; }

    /// <summary>
    /// Physical location in warehouse/pharmacy
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Last restock date
    /// </summary>
    public DateTime? LastRestockDate { get; private set; }

    /// <summary>
    /// Foreign key to Supplier
    /// </summary>
    public int? SupplierId { get; private set; }

    /// <summary>
    /// Navigation property to Supplier
    /// </summary>
    public virtual Supplier? Supplier { get; private set; }

    /// <summary>
    /// Additional notes
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private Inventory() : base()
    {
    }

    /// <summary>
    /// Constructor with minimal information (Constructor Overloading - 1)
    /// </summary>
    public Inventory(int medicationId, int quantity)
    {
        if (medicationId <= 0)
            throw new ArgumentException("Medication ID must be positive", nameof(medicationId));

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

        MedicationId = medicationId;
        CurrentQuantity = quantity;
        ReorderLevel = 50; // Default
        ReorderQuantity = 100; // Default
        LastRestockDate = DateTime.Now;
    }

    /// <summary>
    /// Constructor with reorder level (Constructor Overloading - 2)
    /// </summary>
    public Inventory(int medicationId, int quantity, int reorderLevel)
        : this(medicationId, quantity)
    {
        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative", nameof(reorderLevel));

        ReorderLevel = reorderLevel;
    }

    /// <summary>
    /// Constructor with reorder quantities (Constructor Overloading - 3)
    /// </summary>
    public Inventory(int medicationId, int quantity, int reorderLevel, int reorderQuantity)
        : this(medicationId, quantity, reorderLevel)
    {
        if (reorderQuantity <= 0)
            throw new ArgumentException("Reorder quantity must be positive", nameof(reorderQuantity));

        ReorderQuantity = reorderQuantity;
    }

    /// <summary>
    /// Constructor with full information (Constructor Overloading - 4)
    /// </summary>
    public Inventory(int medicationId, int quantity, int reorderLevel, 
                    int reorderQuantity, string location)
        : this(medicationId, quantity, reorderLevel, reorderQuantity)
    {
        Location = location;
    }

    /// <summary>
    /// Constructor with supplier (Constructor Overloading - 5)
    /// </summary>
    public Inventory(int medicationId, int quantity, int reorderLevel,
                    int reorderQuantity, string location, int supplierId)
        : this(medicationId, quantity, reorderLevel, reorderQuantity, location)
    {
        if (supplierId <= 0)
            throw new ArgumentException("Supplier ID must be positive", nameof(supplierId));

        SupplierId = supplierId;
    }

    #region Operator Overloading

    /// <summary>
    /// Addition operator - adds stock to inventory
    /// Usage: inventory = inventory + 50;
    /// </summary>
    public static Inventory operator +(Inventory inventory, int quantity)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        if (quantity < 0)
            throw new ArgumentException("Cannot add negative quantity", nameof(quantity));

        inventory.AddStock(quantity);
        return inventory;
    }

    /// <summary>
    /// Subtraction operator - removes stock from inventory
    /// Usage: inventory = inventory - 20;
    /// </summary>
    public static Inventory operator -(Inventory inventory, int quantity)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        if (quantity < 0)
            throw new ArgumentException("Cannot subtract negative quantity", nameof(quantity));

        if (inventory.CurrentQuantity < quantity)
            throw new InvalidOperationException(
                $"Insufficient stock. Available: {inventory.CurrentQuantity}, Requested: {quantity}");

        inventory.RemoveStock(quantity);
        return inventory;
    }

    /// <summary>
    /// Less than operator - checks if current quantity is below threshold
    /// Usage: if (inventory < 100) { ... }
    /// </summary>
    public static bool operator <(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity < threshold;
    }

    /// <summary>
    /// Greater than operator - checks if current quantity is above threshold
    /// Usage: if (inventory > 50) { ... }
    /// </summary>
    public static bool operator >(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity > threshold;
    }

    /// <summary>
    /// Less than or equal operator
    /// </summary>
    public static bool operator <=(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity <= threshold;
    }

    /// <summary>
    /// Greater than or equal operator
    /// </summary>
    public static bool operator >=(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity >= threshold;
    }

    /// <summary>
    /// Equality operator
    /// </summary>
    public static bool operator ==(Inventory? left, Inventory? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator
    /// </summary>
    public static bool operator !=(Inventory? left, Inventory? right)
    {
        return !(left == right);
    }

    #endregion

    /// <summary>
    /// Adds stock to inventory
    /// </summary>
    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        CurrentQuantity += quantity;
        LastRestockDate = DateTime.Now;
        UpdateTimestamp();
    }

    /// <summary>
    /// Removes stock from inventory
    /// </summary>
    public bool RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (CurrentQuantity < quantity)
            return false;

        CurrentQuantity -= quantity;
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Updates the current quantity directly
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

        CurrentQuantity = newQuantity;
        UpdateTimestamp();
    }

    /// <summary>
    /// Checks if stock is low
    /// </summary>
    public bool IsLowStock()
    {
        return CurrentQuantity <= ReorderLevel;
    }

    /// <summary>
    /// Checks if reorder is required
    /// </summary>
    public bool RequiresReorder()
    {
        return CurrentQuantity < ReorderLevel;
    }

    /// <summary>
    /// Updates reorder settings
    /// </summary>
    public void UpdateReorderSettings(int reorderLevel, int reorderQuantity)
    {
        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative", nameof(reorderLevel));

        if (reorderQuantity <= 0)
            throw new ArgumentException("Reorder quantity must be positive", nameof(reorderQuantity));

        ReorderLevel = reorderLevel;
        ReorderQuantity = reorderQuantity;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates location
    /// </summary>
    public void UpdateLocation(string location)
    {
        Location = location;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates supplier
    /// </summary>
    public void UpdateSupplier(int supplierId)
    {
        if (supplierId <= 0)
            throw new ArgumentException("Supplier ID must be positive", nameof(supplierId));

        SupplierId = supplierId;
        UpdateTimestamp();
    }

    /// <summary>
    /// Adds notes
    /// </summary>
    public void AddNotes(string notes)
    {
        Notes = notes;
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets inventory status
    /// </summary>
    public string GetInventoryStatus()
    {
        var status = $"Medication ID: {MedicationId}\n" +
                    $"Current Quantity: {CurrentQuantity}\n" +
                    $"Reorder Level: {ReorderLevel}\n" +
                    $"Reorder Quantity: {ReorderQuantity}\n" +
                    $"Location: {Location ?? "Not specified"}\n" +
                    $"Last Restock: {LastRestockDate?.ToString("yyyy-MM-dd") ?? "Never"}\n" +
                    $"Status: ";

        if (CurrentQuantity == 0)
            status += "OUT OF STOCK";
        else if (RequiresReorder())
            status += "NEEDS REORDER";
        else if (IsLowStock())
            status += "LOW STOCK";
        else
            status += "ADEQUATE";

        return status;
    }

    /// <summary>
    /// Overrides Equals for proper comparison
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Inventory other && Id == other.Id;
    }

    /// <summary>
    /// Overrides GetHashCode
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
