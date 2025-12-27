using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.Items;

public class OTCMedicine : Medication
{
    public string? UsageInstructions { get; private set; }

    public string? ActiveIngredients { get; private set; }

    public string? Warnings { get; private set; }

    public int? RecommendedMinAge { get; private set; }

    public bool RequiresPrescription => false;

    private OTCMedicine() : base()
    {
    }

    public OTCMedicine(string name, string genericName, Money price)
        : base(name, genericName, price)
    {
    }

    public OTCMedicine(string name, string genericName, Money price,
                      string usageInstructions)
        : base(name, genericName, price)
    {
        UsageInstructions = usageInstructions;
    }

    public OTCMedicine(string name, string genericName, Money price,
                      MedicationCategory category, string usageInstructions,
                      string activeIngredients)
        : base(name, genericName, price, category)
    {
        UsageInstructions = usageInstructions;
        ActiveIngredients = activeIngredients;
    }

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

    public OTCMedicine(OTCMedicine other) : base(other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        UsageInstructions = other.UsageInstructions;
        ActiveIngredients = other.ActiveIngredients;
        Warnings = other.Warnings;
        RecommendedMinAge = other.RecommendedMinAge;
    }

    public void UpdateUsageInstructions(string usageInstructions)
    {
        UsageInstructions = usageInstructions;
        UpdateTimestamp();
    }

    public void UpdateActiveIngredients(string activeIngredients)
    {
        ActiveIngredients = activeIngredients;
        UpdateTimestamp();
    }

    public void UpdateWarnings(string warnings)
    {
        Warnings = warnings;
        UpdateTimestamp();
    }

    public void UpdateRecommendedMinAge(int age)
    {
        if (age < 0)
            throw new ArgumentException("Age cannot be negative", nameof(age));

        RecommendedMinAge = age;
        UpdateTimestamp();
    }

    public bool CanBeSoldWithoutPrescription()
    {
        return !IsExpired() && CurrentStock > 0;
    }

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

    public bool IsSafeForAge(int age)
    {
        if (age < 0)
            throw new ArgumentException("Age cannot be negative", nameof(age));

        if (!RecommendedMinAge.HasValue)
            return true; // No age restriction

        return age >= RecommendedMinAge.Value;
    }

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

    public override string ToString()
    {
        return $"{Name} ({GenericName}) - OTC Medicine\n" +
               $"{GetMedicationInfo()}\n" +
               $"{GetUsageInfo()}\n" +
               $"Stock Status: {(CanBeSoldWithoutPrescription() ? "Available" : "Not Available")}\n" +
               $"All Warnings:\n{GetAllWarnings()}";
    }
}
