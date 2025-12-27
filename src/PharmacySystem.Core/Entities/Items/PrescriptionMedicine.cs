using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;

namespace PharmacySystem.Core.Entities.Items;

public class PrescriptionMedicine : Medication, IPrescribable
{
    public string DosageForm { get; private set; } = string.Empty;

    public string Strength { get; private set; } = string.Empty;

    public bool RequiresPrescription => true;

    public bool ControlledSubstance { get; private set; }

    public int MaxRefills { get; private set; }

    public string? Warnings { get; private set; }

    private PrescriptionMedicine() : base()
    {
    }

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

    public void UpdateWarnings(string warnings)
    {
        Warnings = warnings;
        UpdateTimestamp();
    }

    public void SetControlledSubstance(bool isControlled)
    {
        ControlledSubstance = isControlled;
        if (isControlled && MaxRefills > 2)
        {
            MaxRefills = 2; // Controlled substances limited to 2 refills
        }
        UpdateTimestamp();
    }

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
    public override string ToString()
    {
        return $"{Name} ({GenericName}) - {Strength} {DosageForm}\n" +
               $"{GetMedicationInfo()}\n" +
               $"Controlled Substance: {(ControlledSubstance ? "YES" : "No")}\n" +
               $"Max Refills: {MaxRefills}\n" +
               $"Warnings:\n{GetPrescriptionWarnings()}";
    }
}
