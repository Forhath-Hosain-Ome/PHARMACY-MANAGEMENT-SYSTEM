using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Console.Utilities;

namespace PharmacySystem.Console.Services;

/// <summary>
/// Service for managing medications
/// Demonstrates function overloading with multiple Search method variants
/// </summary>
public class MedicationService
{
    private readonly List<Medication> _medications;
    private readonly List<PrescriptionMedicine> _prescriptionMedicines;
    private readonly List<OTCMedicine> _otcMedicines;
    private static int _nextId = 1;

    public MedicationService()
    {
        _medications = new List<Medication>();
        _prescriptionMedicines = new List<PrescriptionMedicine>();
        _otcMedicines = new List<OTCMedicine>();
    }

    /// <summary>
    /// Assigns an ID to a medication using reflection (simulates database auto-increment)
    /// </summary>
    private void AssignId(Medication medication)
    {
        var idProperty = typeof(Medication).BaseType?.BaseType?.GetProperty("Id");
        if (idProperty != null && idProperty.CanWrite)
        {
            idProperty.SetValue(medication, _nextId++);
        }
    }

    #region Add/Update/Delete Operations

    /// <summary>
    /// Adds a prescription medicine
    /// </summary>
    public void AddPrescriptionMedicine(PrescriptionMedicine medication)
    {
        if (medication == null)
            throw new ArgumentNullException(nameof(medication));

        AssignId(medication);
        _prescriptionMedicines.Add(medication);
        _medications.Add(medication);
        
        ConsoleHelper.PrintSuccess($"Prescription medicine '{medication.Name}' added successfully! (ID: {medication.Id})");
    }

    /// <summary>
    /// Adds an OTC medicine
    /// </summary>
    public void AddOTCMedicine(OTCMedicine medication)
    {
        if (medication == null)
            throw new ArgumentNullException(nameof(medication));

        AssignId(medication);
        _otcMedicines.Add(medication);
        _medications.Add(medication);
        
        ConsoleHelper.PrintSuccess($"OTC medicine '{medication.Name}' added successfully! (ID: {medication.Id})");
    }

    /// <summary>
    /// Gets a medication by ID
    /// </summary>
    public Medication? GetById(int id)
    {
        return _medications.FirstOrDefault(m => m.Id == id);
    }

    /// <summary>
    /// Updates medication price
    /// </summary>
    public void UpdatePrice(int medicationId, Money newPrice)
    {
        var medication = GetById(medicationId);
        if (medication == null)
        {
            ConsoleHelper.PrintError("Medication not found!");
            return;
        }

        medication.UpdatePrice(newPrice);
        ConsoleHelper.PrintSuccess($"Price updated for '{medication.Name}'");
    }

    /// <summary>
    /// Deletes a medication
    /// </summary>
    public bool Delete(int medicationId)
    {
        var medication = GetById(medicationId);
        if (medication == null)
        {
            ConsoleHelper.PrintError("Medication not found!");
            return false;
        }

        _medications.Remove(medication);
        
        if (medication is PrescriptionMedicine pm)
            _prescriptionMedicines.Remove(pm);
        else if (medication is OTCMedicine otc)
            _otcMedicines.Remove(otc);

        ConsoleHelper.PrintSuccess($"Medication '{medication.Name}' deleted successfully!");
        return true;
    }

    #endregion

    #region Search Methods (Function Overloading)

    /// <summary>
    /// Search by name (Function Overloading - 1)
    /// </summary>
    public List<Medication> Search(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new List<Medication>();

        return _medications
            .Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                       m.GenericName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Search by category (Function Overloading - 2)
    /// </summary>
    public List<Medication> Search(MedicationCategory category)
    {
        return _medications
            .Where(m => m.Category == category)
            .ToList();
    }

    /// <summary>
    /// Search by name and category (Function Overloading - 3)
    /// </summary>
    public List<Medication> Search(string name, MedicationCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Search(category);

        return _medications
            .Where(m => m.Category == category &&
                       (m.Name.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        m.GenericName.Contains(name, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    /// <summary>
    /// Search by price range (Function Overloading - 4)
    /// </summary>
    public List<Medication> Search(decimal minPrice, decimal maxPrice)
    {
        if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            return new List<Medication>();

        return _medications
            .Where(m => m.Price.Amount >= minPrice && m.Price.Amount <= maxPrice)
            .ToList();
    }

    /// <summary>
    /// Search by name and price range (Function Overloading - 5)
    /// </summary>
    public List<Medication> Search(string name, decimal minPrice, decimal maxPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Search(minPrice, maxPrice);

        return _medications
            .Where(m => (m.Name.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        m.GenericName.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                       m.Price.Amount >= minPrice && m.Price.Amount <= maxPrice)
            .ToList();
    }

    /// <summary>
    /// Search using ISearchable interface
    /// </summary>
    public List<Medication> SearchUsingInterface(string searchTerm)
    {
        return _medications
            .Where(m => m.MatchesSearch(searchTerm))
            .ToList();
    }

    #endregion

    #region Filter Methods

    /// <summary>
    /// Gets all medications
    /// </summary>
    public List<Medication> GetAll()
    {
        return _medications.ToList();
    }

    /// <summary>
    /// Gets all prescription medicines
    /// </summary>
    public List<PrescriptionMedicine> GetAllPrescriptionMedicines()
    {
        return _prescriptionMedicines.ToList();
    }

    /// <summary>
    /// Gets all OTC medicines
    /// </summary>
    public List<OTCMedicine> GetAllOTCMedicines()
    {
        return _otcMedicines.ToList();
    }

    /// <summary>
    /// Gets expired medications
    /// </summary>
    public List<Medication> GetExpired()
    {
        return _medications
            .Where(m => m.IsExpired())
            .ToList();
    }

    /// <summary>
    /// Gets medications expiring soon
    /// </summary>
    public List<Medication> GetExpiringSoon()
    {
        return _medications
            .Where(m => m.IsExpiringSoon())
            .ToList();
    }

    /// <summary>
    /// Gets low stock medications
    /// </summary>
    public List<Medication> GetLowStock()
    {
        return _medications
            .Where(m => m.IsLowStock())
            .ToList();
    }

    /// <summary>
    /// Gets medications requiring reorder
    /// </summary>
    public List<Medication> GetRequiringReorder()
    {
        return _medications
            .Where(m => m.RequiresReorder())
            .ToList();
    }

    #endregion

    #region Display Methods

    /// <summary>
    /// Displays medication details
    /// </summary>
    public void DisplayMedicationDetails(Medication medication)
    {
        if (medication == null)
        {
            ConsoleHelper.PrintError("Medication not found!");
            return;
        }

        ConsoleHelper.PrintHeader($"Medication Details: {medication.Name}");
        System.Console.WriteLine(medication.GetMedicationInfo());

        if (medication is PrescriptionMedicine pm)
        {
            System.Console.WriteLine($"\nType: Prescription Medicine");
            System.Console.WriteLine($"Dosage Form: {pm.DosageForm}");
            System.Console.WriteLine($"Strength: {pm.Strength}");
            System.Console.WriteLine($"Controlled Substance: {(pm.ControlledSubstance ? "YES" : "No")}");
            System.Console.WriteLine($"\nWarnings:\n{pm.GetPrescriptionWarnings()}");
        }
        else if (medication is OTCMedicine otc)
        {
            System.Console.WriteLine($"\nType: Over-The-Counter Medicine");
            System.Console.WriteLine(otc.GetUsageInfo());
        }

        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays a list of medications
    /// </summary>
    public void DisplayMedicationList(List<Medication> medications, string title = "Medications")
    {
        if (!medications.Any())
        {
            ConsoleHelper.PrintWarning("No medications found.");
            return;
        }

        ConsoleHelper.PrintSection(title);

        var rows = new List<string[]>();
        foreach (var med in medications)
        {
            string type = med is PrescriptionMedicine ? "Rx" : "OTC";
            string stock = med.CurrentStock.ToString();
            string status = med.IsExpired() ? "EXPIRED" :
                           med.IsExpiringSoon() ? "Expiring Soon" :
                           med.IsLowStock() ? "Low Stock" : "OK";

            rows.Add(new[]
            {
                med.Id.ToString(),
                ConsoleHelper.Truncate(med.Name, 25),
                type,
                med.Category.ToString(),
                med.Price.ToDisplayString(),
                stock,
                status
            });
        }

        ConsoleHelper.DisplayTable(
            new[] { "ID", "Name", "Type", "Category", "Price", "Stock", "Status" },
            rows
        );

        ConsoleHelper.PrintInfo($"Total: {medications.Count} medication(s)");
    }

    #endregion

    #region Statistics

    /// <summary>
    /// Gets medication statistics
    /// </summary>
    public void DisplayStatistics()
    {
        ConsoleHelper.PrintHeader("Medication Statistics");

        System.Console.WriteLine($"Total Medications:           {_medications.Count}");
        System.Console.WriteLine($"  - Prescription Medicines:  {_prescriptionMedicines.Count}");
        System.Console.WriteLine($"  - OTC Medicines:           {_otcMedicines.Count}");
        System.Console.WriteLine();
        System.Console.WriteLine($"Expired:                     {GetExpired().Count}");
        System.Console.WriteLine($"Expiring Soon:               {GetExpiringSoon().Count}");
        System.Console.WriteLine($"Low Stock:                   {GetLowStock().Count}");
        System.Console.WriteLine($"Require Reorder:             {GetRequiringReorder().Count}");
        System.Console.WriteLine();

        // Category breakdown
        System.Console.WriteLine("By Category:");
        var categories = _medications.GroupBy(m => m.Category);
        foreach (var category in categories.OrderByDescending(g => g.Count()))
        {
            System.Console.WriteLine($"  {category.Key,-20}: {category.Count()}");
        }
    }

    #endregion
}