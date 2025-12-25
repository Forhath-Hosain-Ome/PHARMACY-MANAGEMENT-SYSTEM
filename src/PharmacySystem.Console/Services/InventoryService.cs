using PharmacySystem.Core.Entities.Operations;
using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Console.Utilities;

namespace PharmacySystem.Console.Services;

/// <summary>
/// Service for managing inventory
/// Demonstrates function overloading with multiple AddStock method variants
/// Also demonstrates operator overloading usage
/// </summary>
public class InventoryService
{
    private readonly List<Inventory> _inventories;
    private readonly MedicationService _medicationService;

    public InventoryService(MedicationService medicationService)
    {
        _inventories = new List<Inventory>();
        _medicationService = medicationService ?? throw new ArgumentNullException(nameof(medicationService));
    }

    #region Basic Operations

    /// <summary>
    /// Creates inventory for a medication
    /// </summary>
    public void CreateInventory(Inventory inventory)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));

        if (_inventories.Any(i => i.MedicationId == inventory.MedicationId))
        {
            ConsoleHelper.PrintError("Inventory already exists for this medication!");
            return;
        }

        _inventories.Add(inventory);
        ConsoleHelper.PrintSuccess($"Inventory created for medication ID {inventory.MedicationId}");
    }

    /// <summary>
    /// Gets inventory by medication ID
    /// </summary>
    public Inventory? GetByMedicationId(int medicationId)
    {
        return _inventories.FirstOrDefault(i => i.MedicationId == medicationId);
    }

    /// <summary>
    /// Gets all inventories
    /// </summary>
    public List<Inventory> GetAll()
    {
        return _inventories.ToList();
    }

    #endregion

    #region AddStock Methods (Function Overloading)

    /// <summary>
    /// Adds stock using medication ID (Function Overloading - 1)
    /// </summary>
    public void AddStock(int medicationId, int quantity)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found for this medication!");
            return;
        }

        // Using operator overloading: inventory + quantity
        inventory = inventory + quantity;
        
        var medication = _medicationService.GetById(medicationId);
        ConsoleHelper.PrintSuccess(
            $"Added {quantity} units to {medication?.Name ?? "medication"}. " +
            $"New stock: {inventory.CurrentQuantity}"
        );
    }

    /// <summary>
    /// Adds stock with notes (Function Overloading - 2)
    /// </summary>
    public void AddStock(int medicationId, int quantity, string notes)
    {
        AddStock(medicationId, quantity);
        
        var inventory = GetByMedicationId(medicationId);
        if (inventory != null)
        {
            inventory.AddNotes(notes);
            ConsoleHelper.PrintInfo($"Notes added: {notes}");
        }
    }

    /// <summary>
    /// Adds stock using Medication object (Function Overloading - 3)
    /// </summary>
    public void AddStock(Medication medication, int quantity)
    {
        if (medication == null)
            throw new ArgumentNullException(nameof(medication));

        AddStock(medication.Id, quantity);
    }

    /// <summary>
    /// Adds stock with supplier information (Function Overloading - 4)
    /// </summary>
    public void AddStock(Medication medication, int quantity, int supplierId)
    {
        if (medication == null)
            throw new ArgumentNullException(nameof(medication));

        var inventory = GetByMedicationId(medication.Id);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found for this medication!");
            return;
        }

        // Using operator overloading
        inventory = inventory + quantity;
        inventory.UpdateSupplier(supplierId);

        ConsoleHelper.PrintSuccess(
            $"Added {quantity} units to {medication.Name} from supplier ID {supplierId}. " +
            $"New stock: {inventory.CurrentQuantity}"
        );
    }

    /// <summary>
    /// Adds stock with full information (Function Overloading - 5)
    /// </summary>
    public void AddStock(Medication medication, int quantity, int supplierId, string notes)
    {
        if (medication == null)
            throw new ArgumentNullException(nameof(medication));

        AddStock(medication, quantity, supplierId);
        
        var inventory = GetByMedicationId(medication.Id);
        if (inventory != null)
        {
            inventory.AddNotes(notes);
            ConsoleHelper.PrintInfo($"Notes: {notes}");
        }
    }

    #endregion

    #region RemoveStock Methods (Function Overloading)

    /// <summary>
    /// Removes stock (Function Overloading - 1)
    /// </summary>
    public bool RemoveStock(int medicationId, int quantity)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found!");
            return false;
        }

        try
        {
            // Using operator overloading: inventory - quantity
            inventory = inventory - quantity;
            
            var medication = _medicationService.GetById(medicationId);
            ConsoleHelper.PrintSuccess(
                $"Removed {quantity} units from {medication?.Name ?? "medication"}. " +
                $"Remaining stock: {inventory.CurrentQuantity}"
            );

            // Check if now low stock
            if (inventory.IsLowStock())
            {
                ConsoleHelper.PrintWarning($"LOW STOCK ALERT: Only {inventory.CurrentQuantity} units remaining!");
            }

            return true;
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.PrintError($"Cannot remove stock: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Removes stock with reason (Function Overloading - 2)
    /// </summary>
    public bool RemoveStock(int medicationId, int quantity, string reason)
    {
        bool success = RemoveStock(medicationId, quantity);
        if (success)
        {
            var inventory = GetByMedicationId(medicationId);
            if (inventory != null)
            {
                inventory.AddNotes($"Stock removed: {reason}");
                ConsoleHelper.PrintInfo($"Reason: {reason}");
            }
        }
        return success;
    }

    #endregion

    #region Demonstration of Operator Overloading

    /// <summary>
    /// Demonstrates operator overloading usage
    /// </summary>
    public void DemonstrateOperatorOverloading(int medicationId)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found!");
            return;
        }

        var medication = _medicationService.GetById(medicationId);
        ConsoleHelper.PrintHeader($"Operator Overloading Demo: {medication?.Name ?? "Medication"}");

        System.Console.WriteLine($"Initial Stock: {inventory.CurrentQuantity}");
        System.Console.WriteLine();

        // Demonstrate addition operator
        System.Console.WriteLine("Using + operator to add 50 units:");
        System.Console.WriteLine("  inventory = inventory + 50;");
        inventory = inventory + 50;
        System.Console.WriteLine($"  Result: {inventory.CurrentQuantity} units");
        System.Console.WriteLine();

        // Demonstrate subtraction operator
        System.Console.WriteLine("Using - operator to remove 20 units:");
        System.Console.WriteLine("  inventory = inventory - 20;");
        inventory = inventory - 20;
        System.Console.WriteLine($"  Result: {inventory.CurrentQuantity} units");
        System.Console.WriteLine();

        // Demonstrate comparison operators
        System.Console.WriteLine("Using comparison operators:");
        System.Console.WriteLine($"  inventory < 100: {inventory < 100}");
        System.Console.WriteLine($"  inventory > 50: {inventory > 50}");
        System.Console.WriteLine($"  inventory <= {inventory.ReorderLevel}: {inventory <= inventory.ReorderLevel}");
        System.Console.WriteLine();
    }

    #endregion

    #region Stock Management

    /// <summary>
    /// Updates reorder settings
    /// </summary>
    public void UpdateReorderSettings(int medicationId, int reorderLevel, int reorderQuantity)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found!");
            return;
        }

        inventory.UpdateReorderSettings(reorderLevel, reorderQuantity);
        ConsoleHelper.PrintSuccess("Reorder settings updated successfully!");
    }

    /// <summary>
    /// Updates location
    /// </summary>
    public void UpdateLocation(int medicationId, string location)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found!");
            return;
        }

        inventory.UpdateLocation(location);
        ConsoleHelper.PrintSuccess($"Location updated to: {location}");
    }

    #endregion

    #region Queries

    /// <summary>
    /// Gets low stock items
    /// </summary>
    public List<Inventory> GetLowStock()
    {
        return _inventories
            .Where(i => i.IsLowStock())
            .ToList();
    }

    /// <summary>
    /// Gets items requiring reorder
    /// </summary>
    public List<Inventory> GetRequiringReorder()
    {
        return _inventories
            .Where(i => i.RequiresReorder())
            .ToList();
    }

    /// <summary>
    /// Gets out of stock items
    /// </summary>
    public List<Inventory> GetOutOfStock()
    {
        return _inventories
            .Where(i => i.CurrentQuantity == 0)
            .ToList();
    }

    /// <summary>
    /// Checks stock availability
    /// </summary>
    public bool IsStockAvailable(int medicationId, int requiredQuantity)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
            return false;

        return inventory.CurrentQuantity >= requiredQuantity;
    }

    #endregion

    #region Display Methods

    /// <summary>
    /// Displays inventory details
    /// </summary>
    public void DisplayInventoryDetails(int medicationId)
    {
        var inventory = GetByMedicationId(medicationId);
        if (inventory == null)
        {
            ConsoleHelper.PrintError("Inventory not found!");
            return;
        }

        var medication = _medicationService.GetById(medicationId);
        ConsoleHelper.PrintHeader($"Inventory: {medication?.Name ?? "Unknown"}");
        
        System.Console.WriteLine(inventory.GetInventoryStatus());
        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays inventory list
    /// </summary>
    public void DisplayInventoryList(List<Inventory> inventories, string title = "Inventory")
    {
        if (!inventories.Any())
        {
            ConsoleHelper.PrintWarning("No inventory records found.");
            return;
        }

        ConsoleHelper.PrintSection(title);

        var rows = new List<string[]>();
        foreach (var inv in inventories)
        {
            var medication = _medicationService.GetById(inv.MedicationId);
            string status = inv.CurrentQuantity == 0 ? "OUT" :
                           inv.RequiresReorder() ? "REORDER" :
                           inv.IsLowStock() ? "LOW" : "OK";

            rows.Add(new[]
            {
                inv.MedicationId.ToString(),
                ConsoleHelper.Truncate(medication?.Name ?? "Unknown", 25),
                inv.CurrentQuantity.ToString(),
                inv.ReorderLevel.ToString(),
                inv.ReorderQuantity.ToString(),
                inv.Location ?? "N/A",
                status
            });
        }

        ConsoleHelper.DisplayTable(
            new[] { "Med ID", "Medication", "Stock", "Reorder Lvl", "Reorder Qty", "Location", "Status" },
            rows
        );

        ConsoleHelper.PrintInfo($"Total: {inventories.Count} inventory record(s)");
    }

    /// <summary>
    /// Displays low stock report
    /// </summary>
    public void DisplayLowStockReport()
    {
        var lowStock = GetLowStock();
        
        if (!lowStock.Any())
        {
            ConsoleHelper.PrintSuccess("All items are adequately stocked!");
            return;
        }

        ConsoleHelper.PrintHeader("Low Stock Report");
        ConsoleHelper.PrintWarning($"{lowStock.Count} item(s) are running low!");
        System.Console.WriteLine();

        DisplayInventoryList(lowStock, "Low Stock Items");
    }

    /// <summary>
    /// Displays reorder report
    /// </summary>
    public void DisplayReorderReport()
    {
        var needReorder = GetRequiringReorder();
        
        if (!needReorder.Any())
        {
            ConsoleHelper.PrintSuccess("No items require reordering at this time.");
            return;
        }

        ConsoleHelper.PrintHeader("Reorder Report");
        ConsoleHelper.PrintWarning($"{needReorder.Count} item(s) need to be reordered!");
        System.Console.WriteLine();

        DisplayInventoryList(needReorder, "Items Requiring Reorder");
    }

    /// <summary>
    /// Displays inventory statistics
    /// </summary>
    public void DisplayStatistics()
    {
        ConsoleHelper.PrintHeader("Inventory Statistics");

        int total = _inventories.Count;
        int lowStock = GetLowStock().Count;
        int needReorder = GetRequiringReorder().Count;
        int outOfStock = GetOutOfStock().Count;
        int totalUnits = _inventories.Sum(i => i.CurrentQuantity);

        System.Console.WriteLine($"Total Items Tracked:         {total}");
        System.Console.WriteLine($"Total Units in Stock:        {totalUnits:N0}");
        System.Console.WriteLine($"Out of Stock:                {outOfStock}");
        System.Console.WriteLine($"Low Stock:                   {lowStock}");
        System.Console.WriteLine($"Require Reorder:             {needReorder}");
        System.Console.WriteLine($"Adequately Stocked:          {total - lowStock}");
        System.Console.WriteLine();

        if (total > 0)
        {
            decimal avgStock = (decimal)totalUnits / total;
            System.Console.WriteLine($"Average Stock per Item:      {avgStock:N2} units");
        }
    }

    #endregion
}
