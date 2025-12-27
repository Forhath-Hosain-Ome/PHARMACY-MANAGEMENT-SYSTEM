namespace PharmacySystem.Core.Interfaces;


public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int CurrentStock { get; }
    int ReorderLevel { get; }
    bool IsLowStock();
    bool RequiresReorder();
    void UpdateStock(int quantity);
}
public interface IPrescribable
{
    string Name { get; }
    string GenericName { get; }
    string DosageForm { get; }
    string Strength { get; }
    bool RequiresPrescription { get; }
    bool CanDispense(int quantity);
    string GetPrescriptionWarnings();
}

public interface ISearchable
{
    int Id { get; }
    string GetSearchableText();
    bool MatchesSearch(string searchTerm);
}
