namespace PharmacySystem.Core.Interfaces;

/// <summary>
/// Interface for items that can be tracked in inventory
/// </summary>
public interface IInventoryItem
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Name of the inventory item
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Current stock quantity
    /// </summary>
    int CurrentStock { get; }

    /// <summary>
    /// Reorder level threshold
    /// </summary>
    int ReorderLevel { get; }

    /// <summary>
    /// Checks if the item is low in stock
    /// </summary>
    bool IsLowStock();

    /// <summary>
    /// Checks if the item requires reordering
    /// </summary>
    bool RequiresReorder();

    /// <summary>
    /// Updates the stock quantity
    /// </summary>
    void UpdateStock(int quantity);
}

/// <summary>
/// Interface for medications that require prescriptions
/// </summary>
public interface IPrescribable
{
    /// <summary>
    /// Medication name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Generic medication name
    /// </summary>
    string GenericName { get; }

    /// <summary>
    /// Dosage form (tablet, capsule, liquid, etc.)
    /// </summary>
    string DosageForm { get; }

    /// <summary>
    /// Strength of the medication
    /// </summary>
    string Strength { get; }

    /// <summary>
    /// Whether this medication requires a prescription
    /// </summary>
    bool RequiresPrescription { get; }

    /// <summary>
    /// Checks if the medication can be dispensed in the specified quantity
    /// </summary>
    bool CanDispense(int quantity);

    /// <summary>
    /// Gets prescription warnings for this medication
    /// </summary>
    string GetPrescriptionWarnings();
}

/// <summary>
/// Interface for entities that can be searched
/// </summary>
public interface ISearchable
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Gets all searchable text concatenated
    /// </summary>
    string GetSearchableText();

    /// <summary>
    /// Checks if the entity matches the search term
    /// </summary>
    bool MatchesSearch(string searchTerm);
}
