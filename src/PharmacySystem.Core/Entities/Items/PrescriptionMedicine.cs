using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;

namespace PharmacySystem.Core.Entities.Items;

/// <summary>
/// Represents a prescription medication
/// Inherits from Medication and implements IPrescribable interface
/// </summary>
public class PrescriptionMedicine : Medication, IPrescribable
{
    /// <summary>
    /// Dosage form (tablet, capsule, liquid, etc.)
    /// </summary>
    public string DosageForm { get; private set; } = string.Empty;

    /// <summary>
    /// Strength of the medication (e.g., "500mg", "10mg/ml")
    /// </summary>
    public string Strength { get; private set; } = string.Empty;

    /// <summary>
    /// Whether this medication requires a prescription (always true for this type)
    /// </summary>
    public bool RequiresPrescription => true;

    /// <summary>
    /// Whether this is a controlled substance
    /// </summary>
    public bool ControlledSubstance { get; private set; }

    /// <summary>
    /// Maximum number of refills allowed
    /// </summary>
    public int MaxRefills { get; private set; }

    /// <summary>
    /// Special warnings for this medication
    /// </summary>
    public string? Warnings { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private PrescriptionMedicine() : base()
    {
    }

    /// <summary>
    /// Constructor with basic information (Constructor Overloading - 1)
    /// </summary>
    public PrescriptionMedicine(string name, string genericName, Money price,
                               string dosageForm, string strength)
        : base(name, genericName, price)
    {
        if (string.IsNullOrWhiteSpace(dosageForm))
            throw new ArgumentException("Dosage form cannot be empty", nameof(dosageForm));

        if (string.IsNullOrWhiteSpace(strength))
            throw new ArgumentException("Strength cannot be empty", nameof(strength));

        DosageForm = dosageForm;
        Strength = strength;
        MaxRefills = 5; // Default
        ControlledSubstance = false;
    }

    /// <summary>
    /// Constructor with category and manufacturer (Constructor Overloading - 2)
    /// </summary>
    public PrescriptionMedicine(string name, string genericName, Money price,
                               MedicationCategory category, string manufacturer,
                               DateTime expiryDate, string dosageForm, string strength)
        : base(name, genericName, price, category, manufacturer, expiryDate)
    {
        if (string.IsNullOrWhiteSpace(dosageForm))
            throw new ArgumentException("Dosage form cannot be empty", nameof(dosageForm));

        if (string.IsNullOrWhiteSpace(strength))
            throw new ArgumentException("Strength cannot be empty", nameof(strength));

        DosageForm = dosageForm;
        Strength = strength;
        MaxRefills = 5;
        ControlledSubstance = false;
    }

    /// <summary>
    /// Constructor with controlled substance flag (Constructor Overloading - 3)
    /// </summary>
    public PrescriptionMedicine(string name, string genericName, string description, Money price,
                               MedicationCategory category, string manufacturer, DateTime expiryDate,
                               string batchNumber, string dosageForm, string strength,
                               bool controlledSubstance, int maxRefills)
        : base(name, genericName, description, price, category, manufacturer, expiryDate, batchNumber)
    {
        if (string.IsNullOrWhiteSpace(dosageForm))
            throw new ArgumentException("Dosage form cannot be empty", nameof(dosageForm));

        if (string.IsNullOrWhiteSpace(strength))
            throw new ArgumentException("Strength cannot be empty", nameof(strength));

        if (maxRefills < 0)
            throw new ArgumentException("Max refills cannot be negative", nameof(maxRefills));

        DosageForm = dosageForm;
        Strength = strength;
        ControlledSubstance = controlledSubstance;
        MaxRefills = controlledSubstance ? Math.Min(maxRefills, 2) : maxRefills; // Controlled substances max 2 refills
    }

    /// <summary>
    /// Copy constructor for PrescriptionMedicine (Copy Constructor)
    /// </summary>
    public PrescriptionMedicine(PrescriptionMedicine other) : base(other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        DosageForm = other.DosageForm;
        Strength = other.Strength;
        ControlledSubstance = other.ControlledSubstance;
        MaxRefills = other.MaxRefills;
        Warnings = other.Warnings;
    }

    /// <summary>
    /// Updates dosage information
    /// </summary>
    public void UpdateDosageInfo(string dosageForm, string strength)
    {
        if (string.IsNullOrWhiteSpace(dosageForm))
            throw new ArgumentException("Dosage form cannot be empty", nameof(dosageForm));

        if (string.IsNullOrWhiteSpace(strength))
            throw new ArgumentException("Strength cannot be empty", nameof(strength));

        DosageForm = dosageForm;
        Strength = strength;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates warnings
    /// </summary>
    public void UpdateWarnings(string warnings)
    {
        Warnings = warnings;
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets controlled substance status
    /// </summary>
    public void SetControlledSubstance(bool isControlled)
    {
        ControlledSubstance = isControlled;
        if (isControlled && MaxRefills > 2)
        {
            MaxRefills = 2; // Controlled substances limited to 2 refills
        }
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates max refills
    /// </summary>
    public void UpdateMaxRefills(int maxRefills)
    {
        if (maxRefills < 0)
            throw new ArgumentException("Max refills cannot be negative", nameof(maxRefills));

        if (ControlledSubstance && maxRefills > 2)
            throw new ArgumentException("Controlled substances cannot have more than 2 refills");

        MaxRefills = maxRefills;
        UpdateTimestamp();
    }

    #region IPrescribable Implementation

    /// <summary>
    /// Checks if the medication can be dispensed in the specified quantity
    /// </summary>
    public bool CanDispense(int quantity)
    {
        if (quantity <= 0)
            return false;

        if (IsExpired())
            return false;

        if (CurrentStock < quantity)
            return false;

        return true;
    }

    /// <summary>
    /// Gets prescription warnings for this medication
    /// </summary>
    public string GetPrescriptionWarnings()
    {
        var warningsList = new List<string>();

        if (IsExpired())
            warningsList.Add("CRITICAL: Medication is EXPIRED");
        else if (IsExpiringSoon())
            warningsList.Add($"WARNING: Medication expires in {GetDaysUntilExpiry()} days");

        if (ControlledSubstance)
            warningsList.Add("CONTROLLED SUBSTANCE: Special handling required");

        if (IsLowStock())
            warningsList.Add($"LOW STOCK: Only {CurrentStock} units available");

        if (!string.IsNullOrEmpty(Warnings))
            warningsList.Add(Warnings);

        return warningsList.Count > 0
            ? string.Join("\n", warningsList)
            : "No warnings";
    }

    #endregion

    /// <summary>
    /// Gets detailed prescription medicine information
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({GenericName}) - {Strength} {DosageForm}\n" +
               $"{GetMedicationInfo()}\n" +
               $"Controlled Substance: {(ControlledSubstance ? "YES" : "No")}\n" +
               $"Max Refills: {MaxRefills}\n" +
               $"Warnings:\n{GetPrescriptionWarnings()}";
    }
}
