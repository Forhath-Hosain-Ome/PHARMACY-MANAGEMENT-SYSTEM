using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;
using PharmacySystem.Core.Entities.Operations;

namespace PharmacySystem.Core.Entities.People;

/// <summary>
/// Represents a pharmacist in the system
/// Inherits from Employee and implements ISearchable
/// </summary>
public class Pharmacist : Employee, ISearchable
{
    /// <summary>
    /// Pharmacist license number
    /// </summary>
    public string LicenseNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Specialization area
    /// </summary>
    public string? Specialization { get; private set; }

    /// <summary>
    /// License expiry date
    /// </summary>
    public DateTime LicenseExpiryDate { get; private set; }

    /// <summary>
    /// Navigation property for processed prescriptions
    /// </summary>
    public virtual ICollection<Prescription> ProcessedPrescriptions { get; private set; } = new List<Prescription>();

    /// <summary>
    /// Navigation property for handled transactions
    /// </summary>
    public virtual ICollection<Transaction> HandledTransactions { get; private set; } = new List<Transaction>();

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private Pharmacist() : base()
    {
    }

    /// <summary>
    /// Constructor with basic pharmacist information (Constructor Overloading - 1)
    /// </summary>
    public Pharmacist(string username, string email, string firstName,
                     string lastName, DateTime dateOfBirth, string employeeId,
                     DateTime hireDate, decimal salary, string licenseNumber,
                     DateTime licenseExpiryDate)
        : base(username, email, UserRole.Pharmacist, firstName, lastName, dateOfBirth,
              employeeId, hireDate, salary)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number cannot be empty", nameof(licenseNumber));

        if (licenseExpiryDate <= DateTime.Now)
            throw new ArgumentException("License expiry date must be in the future", nameof(licenseExpiryDate));

        LicenseNumber = licenseNumber;
        LicenseExpiryDate = licenseExpiryDate;
        Department = "Pharmacy";
    }

    /// <summary>
    /// Constructor with specialization (Constructor Overloading - 2)
    /// </summary>
    public Pharmacist(string username, string email, string firstName,
                     string lastName, DateTime dateOfBirth, string phoneNumber,
                     string employeeId, DateTime hireDate, decimal salary,
                     string licenseNumber, DateTime licenseExpiryDate,
                     string specialization)
        : base(username, email, UserRole.Pharmacist, firstName, lastName, dateOfBirth,
              phoneNumber, employeeId, hireDate, salary, "Pharmacy")
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number cannot be empty", nameof(licenseNumber));

        if (licenseExpiryDate <= DateTime.Now)
            throw new ArgumentException("License expiry date must be in the future", nameof(licenseExpiryDate));

        LicenseNumber = licenseNumber;
        LicenseExpiryDate = licenseExpiryDate;
        Specialization = specialization;
    }

    /// <summary>
    /// Constructor with full information (Constructor Overloading - 3)
    /// </summary>
    public Pharmacist(string username, string email, string firstName,
                     string lastName, DateTime dateOfBirth, string phoneNumber,
                     Address address, string employeeId, DateTime hireDate,
                     decimal salary, string licenseNumber, DateTime licenseExpiryDate,
                     string specialization)
        : base(username, email, UserRole.Pharmacist, firstName, lastName, dateOfBirth,
              phoneNumber, employeeId, hireDate, salary, "Pharmacy")
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number cannot be empty", nameof(licenseNumber));

        if (licenseExpiryDate <= DateTime.Now)
            throw new ArgumentException("License expiry date must be in the future", nameof(licenseExpiryDate));

        LicenseNumber = licenseNumber;
        LicenseExpiryDate = licenseExpiryDate;
        Specialization = specialization;
        UpdateContactInfo(phoneNumber, address);
    }

    /// <summary>
    /// Verifies if the license is valid
    /// </summary>
    public bool VerifyLicense()
    {
        return LicenseExpiryDate > DateTime.Now && IsEmployed && IsActive;
    }

    /// <summary>
    /// Updates license information
    /// </summary>
    public void UpdateLicense(string licenseNumber, DateTime expiryDate)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number cannot be empty", nameof(licenseNumber));

        if (expiryDate <= DateTime.Now)
            throw new ArgumentException("License expiry date must be in the future", nameof(expiryDate));

        LicenseNumber = licenseNumber;
        LicenseExpiryDate = expiryDate;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates specialization
    /// </summary>
    public void UpdateSpecialization(string specialization)
    {
        Specialization = specialization;
        UpdateTimestamp();
    }

    /// <summary>
    /// Processes a prescription
    /// </summary>
    public void ProcessPrescription(Prescription prescription)
    {
        if (prescription == null)
            throw new ArgumentNullException(nameof(prescription));

        if (!VerifyLicense())
            throw new InvalidOperationException("Cannot process prescription with invalid license");

        ProcessedPrescriptions.Add(prescription);
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets the count of processed prescriptions
    /// </summary>
    public int GetProcessedPrescriptionCount()
    {
        return ProcessedPrescriptions.Count;
    }

    /// <summary>
    /// Gets the count of handled transactions
    /// </summary>
    public int GetHandledTransactionCount()
    {
        return HandledTransactions.Count;
    }

    /// <summary>
    /// Gets days until license expiry
    /// </summary>
    public int GetDaysUntilLicenseExpiry()
    {
        return (LicenseExpiryDate - DateTime.Now).Days;
    }

    /// <summary>
    /// Checks if license is expiring soon (within 30 days)
    /// </summary>
    public bool IsLicenseExpiringSoon()
    {
        return GetDaysUntilLicenseExpiry() <= 30 && GetDaysUntilLicenseExpiry() > 0;
    }

    /// <summary>
    /// Gets detailed pharmacist information
    /// </summary>
    public override string GetEmployeeInfo()
    {
        return base.GetEmployeeInfo() +
               $"\nLicense Number: {LicenseNumber}" +
               $"\nLicense Expiry: {LicenseExpiryDate:yyyy-MM-dd}" +
               $"\nLicense Status: {(VerifyLicense() ? "Valid" : "Invalid/Expired")}" +
               $"\nSpecialization: {Specialization ?? "General"}" +
               $"\nPrescriptions Processed: {GetProcessedPrescriptionCount()}" +
               $"\nTransactions Handled: {GetHandledTransactionCount()}";
    }

    #region ISearchable Implementation

    /// <summary>
    /// Gets all searchable text for this pharmacist
    /// </summary>
    public string GetSearchableText()
    {
        return $"{GetFullName()} {Username} {Email} {EmployeeId} {LicenseNumber} {Specialization}".ToLower();
    }

    /// <summary>
    /// Checks if pharmacist matches the search term
    /// </summary>
    public bool MatchesSearch(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        var lowerSearch = searchTerm.ToLower();
        return GetSearchableText().Contains(lowerSearch);
    }

    #endregion
}
