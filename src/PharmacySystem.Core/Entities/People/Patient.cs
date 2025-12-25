using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;
using PharmacySystem.Core.Entities.Operations;

namespace PharmacySystem.Core.Entities.People;

/// <summary>
/// Represents a patient in the pharmacy system
/// Inherits from Person and implements ISearchable interface
/// </summary>
public class Patient : Person, ISearchable
{
    /// <summary>
    /// Patient's medical history
    /// </summary>
    public string? MedicalHistory { get; private set; }

    /// <summary>
    /// Known allergies
    /// </summary>
    public string? Allergies { get; private set; }

    /// <summary>
    /// Insurance number
    /// </summary>
    public string? InsuranceNumber { get; private set; }

    /// <summary>
    /// Emergency contact name
    /// </summary>
    public string? EmergencyContactName { get; private set; }

    /// <summary>
    /// Emergency contact phone
    /// </summary>
    public string? EmergencyContactPhone { get; private set; }

    /// <summary>
    /// Navigation property for prescriptions
    /// </summary>
    public virtual ICollection<Prescription> Prescriptions { get; private set; } = new List<Prescription>();

    /// <summary>
    /// Navigation property for transactions
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private Patient() : base()
    {
    }

    /// <summary>
    /// Constructor with basic patient information (Constructor Overloading - 1)
    /// </summary>
    public Patient(string username, string email, string firstName, 
                   string lastName, DateTime dateOfBirth)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth)
    {
    }

    /// <summary>
    /// Constructor with contact information (Constructor Overloading - 2)
    /// </summary>
    public Patient(string username, string email, string firstName,
                   string lastName, DateTime dateOfBirth, string phoneNumber)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth, phoneNumber)
    {
    }

    /// <summary>
    /// Constructor with insurance information (Constructor Overloading - 3)
    /// </summary>
    public Patient(string username, string email, string firstName,
                   string lastName, DateTime dateOfBirth, string phoneNumber,
                   string insuranceNumber)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth, phoneNumber)
    {
        InsuranceNumber = insuranceNumber;
    }

    /// <summary>
    /// Constructor with full information (Constructor Overloading - 4)
    /// </summary>
    public Patient(string username, string email, string firstName,
                   string lastName, DateTime dateOfBirth, string phoneNumber,
                   Address address, string insuranceNumber, string? allergies = null)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth, phoneNumber, address)
    {
        InsuranceNumber = insuranceNumber;
        Allergies = allergies;
    }

    /// <summary>
    /// Updates medical history
    /// </summary>
    public void UpdateMedicalHistory(string medicalHistory)
    {
        MedicalHistory = medicalHistory;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates allergy information
    /// </summary>
    public void UpdateAllergies(string allergies)
    {
        Allergies = allergies;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates insurance information
    /// </summary>
    public void UpdateInsurance(string insuranceNumber)
    {
        if (string.IsNullOrWhiteSpace(insuranceNumber))
            throw new ArgumentException("Insurance number cannot be empty", nameof(insuranceNumber));

        InsuranceNumber = insuranceNumber;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates emergency contact information
    /// </summary>
    public void UpdateEmergencyContact(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Emergency contact name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Emergency contact phone cannot be empty", nameof(phone));

        EmergencyContactName = name;
        EmergencyContactPhone = phone;
        UpdateTimestamp();
    }

    /// <summary>
    /// Adds a prescription to the patient
    /// </summary>
    public void AddPrescription(Prescription prescription)
    {
        if (prescription == null)
            throw new ArgumentNullException(nameof(prescription));

        Prescriptions.Add(prescription);
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets medical information summary
    /// </summary>
    public string GetMedicalInfo()
    {
        return $"Patient: {GetFullName()}\n" +
               $"Age: {GetAge()}\n" +
               $"Allergies: {Allergies ?? "None"}\n" +
               $"Insurance: {InsuranceNumber ?? "None"}\n" +
               $"Medical History: {(string.IsNullOrEmpty(MedicalHistory) ? "None recorded" : "On file")}";
    }

    /// <summary>
    /// Checks if patient has a specific allergy
    /// </summary>
    public bool HasAllergy(string medication)
    {
        if (string.IsNullOrEmpty(Allergies))
            return false;

        return Allergies.Contains(medication, StringComparison.OrdinalIgnoreCase);
    }

    #region ISearchable Implementation

    /// <summary>
    /// Gets all searchable text for this patient
    /// </summary>
    public string GetSearchableText()
    {
        return $"{GetFullName()} {Username} {Email} {PhoneNumber} {InsuranceNumber}".ToLower();
    }

    /// <summary>
    /// Checks if patient matches the search term
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
