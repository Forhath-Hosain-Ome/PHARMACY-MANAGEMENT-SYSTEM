using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.Items;

namespace PharmacySystem.Core.Entities.Operations;

public class Inventory : Entity
{
    public int MedicationId { get; private set; }
    public virtual Medication? Medication { get; private set; }
    public int CurrentQuantity { get; private set; }
    public int ReorderLevel { get; private set; }
    public int ReorderQuantity { get; private set; }
    public string? Location { get; private set; }
    public DateTime? LastRestockDate { get; private set; }
    public int? SupplierId { get; private set; }
    public virtual Supplier? Supplier { get; private set; }
    public string? Notes { get; private set; }

    private Inventory() : base()
    {
    }

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


    public Inventory(int medicationId, int quantity, int reorderLevel)
        : this(medicationId, quantity)
    {
        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative", nameof(reorderLevel));

        ReorderLevel = reorderLevel;
    }


    public Inventory(int medicationId, int quantity, int reorderLevel, int reorderQuantity)
        : this(medicationId, quantity, reorderLevel)
    {
        if (reorderQuantity <= 0)
            throw new ArgumentException("Reorder quantity must be positive", nameof(reorderQuantity));

        ReorderQuantity = reorderQuantity;
    }


    public Inventory(int medicationId, int quantity, int reorderLevel, 
                    int reorderQuantity, string location)
        : this(medicationId, quantity, reorderLevel, reorderQuantity)
    {
        Location = location;
    }


    public Inventory(int medicationId, int quantity, int reorderLevel,
                    int reorderQuantity, string location, int supplierId)
        : this(medicationId, quantity, reorderLevel, reorderQuantity, location)
    {
        if (supplierId <= 0)
            throw new ArgumentException("Supplier ID must be positive", nameof(supplierId));

        SupplierId = supplierId;
    }

    #region Operator Overloading

    public static Inventory operator +(Inventory inventory, int quantity)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        if (quantity < 0)
            throw new ArgumentException("Cannot add negative quantity", nameof(quantity));

        inventory.AddStock(quantity);
        return inventory;
    }


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


    public static bool operator <(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity < threshold;
    }


    public static bool operator >(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity > threshold;
    }


    public static bool operator <=(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity <= threshold;
    }


    public static bool operator >=(Inventory inventory, int threshold)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        return inventory.CurrentQuantity >= threshold;
    }


    public static bool operator ==(Inventory? left, Inventory? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }


    public static bool operator !=(Inventory? left, Inventory? right)
    {
        return !(left == right);
    }

    #endregion


    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        CurrentQuantity += quantity;
        LastRestockDate = DateTime.Now;
        UpdateTimestamp();
    }


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


    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

        CurrentQuantity = newQuantity;
        UpdateTimestamp();
    }


    public bool IsLowStock()
    {
        return CurrentQuantity <= ReorderLevel;
    }


    public bool RequiresReorder()
    {
        return CurrentQuantity < ReorderLevel;
    }


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


    public void UpdateLocation(string location)
    {
        Location = location;
        UpdateTimestamp();
    }


    public void UpdateSupplier(int supplierId)
    {
        if (supplierId <= 0)
            throw new ArgumentException("Supplier ID must be positive", nameof(supplierId));

        SupplierId = supplierId;
        UpdateTimestamp();
    }


    public void AddNotes(string notes)
    {
        Notes = notes;
        UpdateTimestamp();
    }


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


    public override bool Equals(object? obj)
    {
        return obj is Inventory other && Id == other.Id;
    }


    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
