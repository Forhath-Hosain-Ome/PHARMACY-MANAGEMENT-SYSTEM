using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;
using PharmacySystem.Core.Entities.Operations;

namespace PharmacySystem.Core.Entities.People;

public class Patient : Person, ISearchable
{
    public string? MedicalHistory { get; private set; }
    public string? Allergies { get; private set; }
    public string? InsuranceNumber { get; private set; }
    public string? EmergencyContactName { get; private set; }
    public string? EmergencyContactPhone { get; private set; }
    public virtual ICollection<Prescription> Prescriptions { get; private set; } = new List<Prescription>();
    public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
    private Patient() : base()
    {
    }
    public Patient(string username, string email, string firstName, 
                   string lastName, DateTime dateOfBirth)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth)
    {
    }
    public Patient(string username, string email, string firstName,
                   string lastName, DateTime dateOfBirth, string phoneNumber)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth, phoneNumber)
    {
    }
    public Patient(string username, string email, string firstName,
                   string lastName, DateTime dateOfBirth, string phoneNumber,
                   string insuranceNumber)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth, phoneNumber)
    {
        InsuranceNumber = insuranceNumber;
    }
    public Patient(string username, string email, string firstName,
                   string lastName, DateTime dateOfBirth, string phoneNumber,
                   Address address, string insuranceNumber, string? allergies = null)
        : base(username, email, UserRole.Patient, firstName, lastName, dateOfBirth, phoneNumber, address)
    {
        InsuranceNumber = insuranceNumber;
        Allergies = allergies;
    }
    public void UpdateMedicalHistory(string medicalHistory)
    {
        MedicalHistory = medicalHistory;
        UpdateTimestamp();
    }
    public void UpdateAllergies(string allergies)
    {
        Allergies = allergies;
        UpdateTimestamp();
    }
    public void UpdateInsurance(string insuranceNumber)
    {
        if (string.IsNullOrWhiteSpace(insuranceNumber))
            throw new ArgumentException("Insurance number cannot be empty", nameof(insuranceNumber));

        InsuranceNumber = insuranceNumber;
        UpdateTimestamp();
    }
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
    public void AddPrescription(Prescription prescription)
    {
        if (prescription == null)
            throw new ArgumentNullException(nameof(prescription));

        Prescriptions.Add(prescription);
        UpdateTimestamp();
    }
    public string GetMedicalInfo()
    {
        return $"Patient: {GetFullName()}\n" +
               $"Age: {GetAge()}\n" +
               $"Allergies: {Allergies ?? "None"}\n" +
               $"Insurance: {InsuranceNumber ?? "None"}\n" +
               $"Medical History: {(string.IsNullOrEmpty(MedicalHistory) ? "None recorded" : "On file")}";
    }
    public bool HasAllergy(string medication)
    {
        if (string.IsNullOrEmpty(Allergies))
            return false;

        return Allergies.Contains(medication, StringComparison.OrdinalIgnoreCase);
    }

    #region ISearchable Implementation
    public string GetSearchableText()
    {
        return $"{GetFullName()} {Username} {Email} {PhoneNumber} {InsuranceNumber}".ToLower();
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
