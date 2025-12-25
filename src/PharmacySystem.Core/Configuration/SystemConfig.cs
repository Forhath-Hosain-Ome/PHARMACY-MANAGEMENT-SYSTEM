namespace PharmacySystem.Core.Configuration;

/// <summary>
/// System-wide configuration and utility methods
/// Demonstrates static fields and static methods
/// </summary>
public static class SystemConfig
{
    #region Static Fields (Configuration Constants)

    /// <summary>
    /// Tax rate applied to transactions (15%)
    /// </summary>
    public static readonly decimal TaxRate = 0.15m;

    /// <summary>
    /// Default reorder level for medications
    /// </summary>
    public static readonly int DefaultReorderLevel = 50;

    /// <summary>
    /// Default reorder quantity for medications
    /// </summary>
    public static readonly int DefaultReorderQuantity = 100;

    /// <summary>
    /// Low stock threshold
    /// </summary>
    public static readonly int LowStockThreshold = 20;

    /// <summary>
    /// Discount threshold amount (bulk purchases)
    /// </summary>
    public static readonly decimal DiscountThreshold = 1000m;

    /// <summary>
    /// Bulk discount rate (10%)
    /// </summary>
    public static readonly decimal BulkDiscountRate = 0.10m;

    /// <summary>
    /// Maximum prescription refills for non-controlled substances
    /// </summary>
    public static readonly int MaxPrescriptionRefills = 5;

    /// <summary>
    /// Maximum prescription refills for controlled substances
    /// </summary>
    public static readonly int MaxControlledSubstanceRefills = 2;

    /// <summary>
    /// Days before expiry to show warning
    /// </summary>
    public static readonly int ExpiryWarningDays = 90;

    /// <summary>
    /// Days before license expiry to show warning
    /// </summary>
    public static readonly int LicenseExpiryWarningDays = 30;

    /// <summary>
    /// Minimum password length
    /// </summary>
    public static readonly int MinPasswordLength = 8;

    #endregion

    #region Static Methods (Utility Functions)

    /// <summary>
    /// Calculates tax amount for a given subtotal (Static Method)
    /// </summary>
    public static decimal CalculateTax(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        return Math.Round(amount * TaxRate, 2);
    }

    /// <summary>
    /// Applies discount to an amount (Static Method - Function Overloading 1)
    /// </summary>
    public static decimal ApplyDiscount(decimal amount, decimal discountRate)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (discountRate < 0 || discountRate > 1)
            throw new ArgumentException("Discount rate must be between 0 and 1", nameof(discountRate));

        var discount = amount * discountRate;
        return Math.Round(amount - discount, 2);
    }

    /// <summary>
    /// Applies fixed discount amount (Static Method - Function Overloading 2)
    /// </summary>
    public static decimal ApplyDiscount(decimal amount, decimal discountAmount, bool isFixedAmount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (!isFixedAmount)
            return ApplyDiscount(amount, discountAmount);

        if (discountAmount < 0)
            throw new ArgumentException("Discount amount cannot be negative", nameof(discountAmount));

        if (discountAmount > amount)
            throw new ArgumentException("Discount amount cannot exceed total amount");

        return Math.Round(amount - discountAmount, 2);
    }

    /// <summary>
    /// Checks if stock quantity is low (Static Method)
    /// </summary>
    public static bool IsLowStock(int quantity)
    {
        return quantity <= LowStockThreshold;
    }

    /// <summary>
    /// Checks if reorder is required (Static Method)
    /// </summary>
    public static bool RequiresReorder(int currentQuantity, int reorderLevel)
    {
        return currentQuantity < reorderLevel;
    }

    /// <summary>
    /// Generates a unique prescription number (Static Method)
    /// </summary>
    public static string GeneratePrescriptionNumber()
    {
        return $"RX-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    /// <summary>
    /// Generates a unique transaction number (Static Method)
    /// </summary>
    public static string GenerateTransactionNumber()
    {
        return $"TXN-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    /// <summary>
    /// Generates a unique employee ID (Static Method)
    /// </summary>
    public static string GenerateEmployeeId(string prefix = "EMP")
    {
        return $"{prefix}-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
    }

    /// <summary>
    /// Validates email format (Static Method)
    /// </summary>
    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates phone number format (Static Method)
    /// </summary>
    public static bool ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Remove common formatting characters
        var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());

        // Phone number should have 10-15 digits
        return digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
    }

    /// <summary>
    /// Formats currency for display (Static Method)
    /// </summary>
    public static string FormatCurrency(decimal amount, string currencySymbol = "$")
    {
        return $"{currencySymbol}{amount:N2}";
    }

    /// <summary>
    /// Calculates age from date of birth (Static Method)
    /// </summary>
    public static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age;
    }

    /// <summary>
    /// Checks if a date is expiring soon (Static Method)
    /// </summary>
    public static bool IsExpiringSoon(DateTime expiryDate, int warningDays = -1)
    {
        var daysToCheck = warningDays > 0 ? warningDays : ExpiryWarningDays;
        var daysUntilExpiry = (expiryDate - DateTime.Now).Days;
        return daysUntilExpiry > 0 && daysUntilExpiry <= daysToCheck;
    }

    /// <summary>
    /// Checks if a date is expired (Static Method)
    /// </summary>
    public static bool IsExpired(DateTime expiryDate)
    {
        return DateTime.Now > expiryDate;
    }

    /// <summary>
    /// Validates password strength (Static Method)
    /// </summary>
    public static bool ValidatePasswordStrength(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < MinPasswordLength)
            return false;

        // Must contain at least one uppercase, one lowercase, and one digit
        bool hasUpperCase = password.Any(char.IsUpper);
        bool hasLowerCase = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);

        return hasUpperCase && hasLowerCase && hasDigit;
    }

    /// <summary>
    /// Calculates total with tax (Static Method)
    /// </summary>
    public static decimal CalculateTotalWithTax(decimal subtotal)
    {
        if (subtotal < 0)
            throw new ArgumentException("Subtotal cannot be negative", nameof(subtotal));

        return Math.Round(subtotal * (1 + TaxRate), 2);
    }

    /// <summary>
    /// Determines if bulk discount should be applied (Static Method)
    /// </summary>
    public static bool ShouldApplyBulkDiscount(decimal totalAmount)
    {
        return totalAmount >= DiscountThreshold;
    }

    /// <summary>
    /// Gets suggested reorder quantity based on average consumption (Static Method)
    /// </summary>
    public static int GetSuggestedReorderQuantity(int currentStock, int averageMonthlyConsumption, int leadTimeDays = 30)
    {
        if (averageMonthlyConsumption <= 0)
            return DefaultReorderQuantity;

        // Calculate daily consumption
        var dailyConsumption = (double)averageMonthlyConsumption / 30;

        // Calculate quantity needed for lead time + safety stock (1 month)
        var leadTimeQuantity = (int)Math.Ceiling(dailyConsumption * leadTimeDays);
        var safetyStock = averageMonthlyConsumption;

        return Math.Max(leadTimeQuantity + safetyStock - currentStock, DefaultReorderQuantity);
    }

    /// <summary>
    /// Formats a date for display (Static Method)
    /// </summary>
    public static string FormatDate(DateTime date, bool includeTime = false)
    {
        return includeTime ? date.ToString("yyyy-MM-dd HH:mm:ss") : date.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Gets a friendly time description (Static Method)
    /// </summary>
    public static string GetFriendlyTimeDescription(DateTime date)
    {
        var timeSpan = DateTime.Now - date;

        if (timeSpan.TotalMinutes < 1)
            return "just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minute{((int)timeSpan.TotalMinutes != 1 ? "s" : "")} ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hour{((int)timeSpan.TotalHours != 1 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)timeSpan.TotalDays} day{((int)timeSpan.TotalDays != 1 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} month{((int)(timeSpan.TotalDays / 30) != 1 ? "s" : "")} ago";

        return $"{(int)(timeSpan.TotalDays / 365)} year{((int)(timeSpan.TotalDays / 365) != 1 ? "s" : "")} ago";
    }

    #endregion
}
