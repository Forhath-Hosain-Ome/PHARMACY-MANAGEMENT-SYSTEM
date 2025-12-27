using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;
using PharmacySystem.Core.Entities.Operations;

namespace PharmacySystem.Core.Entities.People;

public class Pharmacist : Employee, ISearchable
{
    public string LicenseNumber { get; private set; } = string.Empty;
    public string? Specialization { get; private set; }
    public DateTime LicenseExpiryDate { get; private set; }
    public virtual ICollection<Prescription> ProcessedPrescriptions { get; private set; } = new List<Prescription>();
    public virtual ICollection<Transaction> HandledTransactions { get; private set; } = new List<Transaction>();
    private Pharmacist() : base()
    {
    }
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
    public bool VerifyLicense()
    {
        return LicenseExpiryDate > DateTime.Now && IsEmployed && IsActive;
    }
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
    public void UpdateSpecialization(string specialization)
    {
        Specialization = specialization;
        UpdateTimestamp();
    }
    public void ProcessPrescription(Prescription prescription)
    {
        if (prescription == null)
            throw new ArgumentNullException(nameof(prescription));

        if (!VerifyLicense())
            throw new InvalidOperationException("Cannot process prescription with invalid license");

        ProcessedPrescriptions.Add(prescription);
        UpdateTimestamp();
    }
    public int GetProcessedPrescriptionCount()
    {
        return ProcessedPrescriptions.Count;
    }
    public int GetHandledTransactionCount()
    {
        return HandledTransactions.Count;
    }
    public int GetDaysUntilLicenseExpiry()
    {
        return (LicenseExpiryDate - DateTime.Now).Days;
    }
    public bool IsLicenseExpiringSoon()
    {
        return GetDaysUntilLicenseExpiry() <= 30 && GetDaysUntilLicenseExpiry() > 0;
    }
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
    public string GetSearchableText()
    {
        return $"{GetFullName()} {Username} {Email} {EmployeeId} {LicenseNumber} {Specialization}".ToLower();
    }
    public bool MatchesSearch(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        var lowerSearch = searchTerm.ToLower();
        return GetSearchableText().Contains(lowerSearch);
    }

    #endregion
}
