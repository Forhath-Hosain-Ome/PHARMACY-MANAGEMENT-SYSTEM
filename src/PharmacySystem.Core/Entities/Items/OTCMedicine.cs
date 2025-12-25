using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.Items;

/// <summary>
/// Represents an Over-The-Counter (OTC) medication
/// Inherits from Medication
/// </summary>
public class OTCMedicine : Medication
{
    /// <summary>
    /// Usage instructions for the medication
    /// </summary>
    public string? UsageInstructions { get; private set; }

    /// <summary>
    /// Active ingredients in the medication
    /// </summary>
    public string? ActiveIngredients { get; private set; }

    /// <summary>
    /// General warnings for this OTC medicine
    /// </summary>
    public string? Warnings { get; private set; }

    /// <summary>
    /// Recommended minimum age for use
    /// </summary>
    public int? RecommendedMinAge { get; private set; }

    /// <summary>
    /// Whether this medication requires a prescription (always false for OTC)
    /// </summary>
    public bool RequiresPrescription => false;

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private OTCMedicine() : base()
    {
    }

    /// <summary>
    /// Constructor with basic information (Constructor Overloading - 1)
    /// </summary>
    public OTCMedicine(string name, string genericName, Money price)
        : base(name, genericName, price)
    {
    }

    /// <summary>
    /// Constructor with usage instructions (Constructor Overloading - 2)
    /// </summary>
    public OTCMedicine(string name, string genericName, Money price,
                      string usageInstructions)
        : base(name, genericName, price)
    {
        UsageInstructions = usageInstructions;
    }

    /// <summary>
    /// Constructor with category and active ingredients (Constructor Overloading - 3)
    /// </summary>
    public OTCMedicine(string name, string genericName, Money price,
                      MedicationCategory category, string usageInstructions,
                      string activeIngredients)
        : base(name, genericName, price, category)
    {
        UsageInstructions = usageInstructions;
        ActiveIngredients = activeIngredients;
    }

    /// <summary>
    /// Constructor with full information (Constructor Overloading - 4)
    /// </summary>
    public OTCMedicine(string name, string genericName, string description, Money price,
                      MedicationCategory category, string manufacturer, DateTime expiryDate,
                      string batchNumber, string usageInstructions, string activeIngredients,
                      string warnings, int recommendedMinAge)
        : base(name, genericName, description, price, category, manufacturer, expiryDate, batchNumber)
    {
        if (recommendedMinAge < 0)
            throw new ArgumentException("Recommended minimum age cannot be negative", nameof(recommendedMinAge));

        UsageInstructions = usageInstructions;
        ActiveIngredients = activeIngredients;
        Warnings = warnings;
        RecommendedMinAge = recommendedMinAge;
    }

    /// <summary>
    /// Copy constructor for OTCMedicine (Copy Constructor)
    /// </summary>
    public OTCMedicine(OTCMedicine other) : base(other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        UsageInstructions = other.UsageInstructions;
        ActiveIngredients = other.ActiveIngredients;
        Warnings = other.Warnings;
        RecommendedMinAge = other.RecommendedMinAge;
    }

    /// <summary>
    /// Updates usage instructions
    /// </summary>
    public void UpdateUsageInstructions(string usageInstructions)
    {
        UsageInstructions = usageInstructions;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates active ingredients
    /// </summary>
    public void UpdateActiveIngredients(string activeIngredients)
    {
        ActiveIngredients = activeIngredients;
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
    /// Updates recommended minimum age
    /// </summary>
    public void UpdateRecommendedMinAge(int age)
    {
        if (age < 0)
            throw new ArgumentException("Age cannot be negative", nameof(age));

        RecommendedMinAge = age;
        UpdateTimestamp();
    }

    /// <summary>
    /// Checks if the medication can be sold without a prescription
    /// </summary>
    public bool CanBeSoldWithoutPrescription()
    {
        return !IsExpired() && CurrentStock > 0;
    }

    /// <summary>
    /// Gets usage information
    /// </summary>
    public string GetUsageInfo()
    {
        var info = $"Usage Instructions:\n{UsageInstructions ?? "Follow label directions"}\n\n";
        info += $"Active Ingredients:\n{ActiveIngredients ?? "See product label"}\n\n";
        
        if (!string.IsNullOrEmpty(Warnings))
            info += $"Warnings:\n{Warnings}\n\n";
        
        if (RecommendedMinAge.HasValue)
            info += $"Recommended for ages {RecommendedMinAge}+\n";
        
        return info;
    }

    /// <summary>
    /// Checks if medication is safe for a given age
    /// </summary>
    public bool IsSafeForAge(int age)
    {
        if (age < 0)
            throw new ArgumentException("Age cannot be negative", nameof(age));

        if (!RecommendedMinAge.HasValue)
            return true; // No age restriction

        return age >= RecommendedMinAge.Value;
    }

    /// <summary>
    /// Gets all warnings for this OTC medicine
    /// </summary>
    public string GetAllWarnings()
    {
        var warningsList = new List<string>();

        if (IsExpired())
            warningsList.Add("CRITICAL: Medication is EXPIRED - DO NOT SELL");
        else if (IsExpiringSoon())
            warningsList.Add($"WARNING: Medication expires in {GetDaysUntilExpiry()} days");

        if (IsLowStock())
            warningsList.Add($"LOW STOCK: Only {CurrentStock} units available");

        if (!string.IsNullOrEmpty(Warnings))
            warningsList.Add($"General Warnings: {Warnings}");

        if (RecommendedMinAge.HasValue)
            warningsList.Add($"Age Restriction: Not recommended for children under {RecommendedMinAge}");

        return warningsList.Count > 0
            ? string.Join("\n", warningsList)
            : "No warnings";
    }

    /// <summary>
    /// Gets detailed OTC medicine information
    /// </summary>
    public override string ToString()
    {
        return $"{Name} ({GenericName}) - OTC Medicine\n" +
               $"{GetMedicationInfo()}\n" +
               $"{GetUsageInfo()}\n" +
               $"Stock Status: {(CanBeSoldWithoutPrescription() ? "Available" : "Not Available")}\n" +
               $"All Warnings:\n{GetAllWarnings()}";
    }
}
