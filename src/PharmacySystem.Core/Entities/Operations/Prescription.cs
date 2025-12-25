using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.People;
using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;

namespace PharmacySystem.Core.Entities.Operations;

/// <summary>
/// Represents a prescription in the pharmacy system
/// Demonstrates copy constructor and implements ISearchable
/// </summary>
public class Prescription : Entity, ISearchable
{
    /// <summary>
    /// Unique prescription number
    /// </summary>
    public string PrescriptionNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Foreign key to Patient
    /// </summary>
    public int PatientId { get; private set; }

    /// <summary>
    /// Navigation property to Patient
    /// </summary>
    public virtual Patient? Patient { get; private set; }

    /// <summary>
    /// Foreign key to Pharmacist
    /// </summary>
    public int PharmacistId { get; private set; }

    /// <summary>
    /// Navigation property to Pharmacist
    /// </summary>
    public virtual Pharmacist? Pharmacist { get; private set; }

    /// <summary>
    /// Date prescription was written
    /// </summary>
    public DateTime PrescriptionDate { get; private set; }

    /// <summary>
    /// Status of the prescription
    /// </summary>
    public PrescriptionStatus Status { get; private set; }

    /// <summary>
    /// Prescribing doctor's name
    /// </summary>
    public string? DoctorName { get; private set; }

    /// <summary>
    /// Doctor's license number
    /// </summary>
    public string? DoctorLicenseNumber { get; private set; }

    /// <summary>
    /// Collection of medications in this prescription
    /// </summary>
    public virtual ICollection<PrescriptionItem> Items { get; private set; } = new List<PrescriptionItem>();

    /// <summary>
    /// Additional notes
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private Prescription() : base()
    {
    }

    /// <summary>
    /// Constructor with basic information (Constructor Overloading - 1)
    /// </summary>
    public Prescription(int patientId, int pharmacistId)
    {
        if (patientId <= 0)
            throw new ArgumentException("Patient ID must be positive", nameof(patientId));

        if (pharmacistId <= 0)
            throw new ArgumentException("Pharmacist ID must be positive", nameof(pharmacistId));

        PatientId = patientId;
        PharmacistId = pharmacistId;
        PrescriptionDate = DateTime.Now;
        Status = PrescriptionStatus.Pending;
        PrescriptionNumber = GeneratePrescriptionNumber();
    }

    /// <summary>
    /// Constructor with doctor information (Constructor Overloading - 2)
    /// </summary>
    public Prescription(int patientId, int pharmacistId, string doctorName, 
                       string doctorLicenseNumber)
        : this(patientId, pharmacistId)
    {
        DoctorName = doctorName;
        DoctorLicenseNumber = doctorLicenseNumber;
    }

    /// <summary>
    /// Copy constructor - creates a deep copy of prescription (Copy Constructor)
    /// Useful for refills or similar prescriptions
    /// </summary>
    public Prescription(Prescription other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // Copy basic properties
        PatientId = other.PatientId;
        PharmacistId = other.PharmacistId;
        DoctorName = other.DoctorName;
        DoctorLicenseNumber = other.DoctorLicenseNumber;
        
        // Reset for new prescription
        PrescriptionNumber = GeneratePrescriptionNumber();
        PrescriptionDate = DateTime.Now;
        Status = PrescriptionStatus.Pending;
        
        // Deep copy items
        Items = new List<PrescriptionItem>();
        foreach (var item in other.Items)
        {
            Items.Add(new PrescriptionItem(item));
        }

        Notes = $"Refill of prescription {other.PrescriptionNumber}";
    }

    /// <summary>
    /// Generates a unique prescription number
    /// </summary>
    private static string GeneratePrescriptionNumber()
    {
        return $"RX-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    /// <summary>
    /// Adds a medication to the prescription
    /// </summary>
    public void AddMedication(int medicationId, int quantity, string dosage, 
                            string frequency, string duration, string? instructions = null)
    {
        if (medicationId <= 0)
            throw new ArgumentException("Medication ID must be positive", nameof(medicationId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (Status != PrescriptionStatus.Pending)
            throw new InvalidOperationException("Cannot modify a prescription that is not pending");

        var item = new PrescriptionItem(Id, medicationId, quantity, dosage, frequency, duration, instructions);
        Items.Add(item);
        UpdateTimestamp();
    }

    /// <summary>
    /// Removes a medication from the prescription
    /// </summary>
    public bool RemoveMedication(int medicationId)
    {
        if (Status != PrescriptionStatus.Pending)
            throw new InvalidOperationException("Cannot modify a prescription that is not pending");

        var item = Items.FirstOrDefault(i => i.MedicationId == medicationId);
        if (item == null)
            return false;

        Items.Remove(item);
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Dispenses the prescription
    /// </summary>
    public bool Dispense()
    {
        if (Status != PrescriptionStatus.Pending)
            return false;

        if (!Items.Any())
            throw new InvalidOperationException("Cannot dispense empty prescription");

        Status = PrescriptionStatus.Dispensed;
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Cancels the prescription
    /// </summary>
    public void Cancel(string reason)
    {
        if (Status == PrescriptionStatus.Dispensed)
            throw new InvalidOperationException("Cannot cancel a dispensed prescription");

        Status = PrescriptionStatus.Cancelled;
        Notes = $"Cancelled: {reason}";
        UpdateTimestamp();
    }

    /// <summary>
    /// Calculates total cost of prescription
    /// </summary>
    public Money GetTotalCost()
    {
        if (!Items.Any())
            return new Money(0);

        // This would need to be calculated with actual medication prices from database
        // For now, returning a placeholder
        return new Money(0);
    }

    /// <summary>
    /// Validates the prescription
    /// </summary>
    public bool Validate()
    {
        if (!Items.Any())
            return false;

        if (string.IsNullOrWhiteSpace(DoctorName))
            return false;

        if (string.IsNullOrWhiteSpace(DoctorLicenseNumber))
            return false;

        if (PatientId <= 0 || PharmacistId <= 0)
            return false;

        return true;
    }

    /// <summary>
    /// Updates notes
    /// </summary>
    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets prescription summary
    /// </summary>
    public string GetPrescriptionSummary()
    {
        return $"Prescription: {PrescriptionNumber}\n" +
               $"Date: {PrescriptionDate:yyyy-MM-dd}\n" +
               $"Patient ID: {PatientId}\n" +
               $"Pharmacist ID: {PharmacistId}\n" +
               $"Doctor: {DoctorName ?? "Not specified"}\n" +
               $"Doctor License: {DoctorLicenseNumber ?? "Not specified"}\n" +
               $"Status: {Status}\n" +
               $"Number of Items: {Items.Count}\n" +
               $"Notes: {Notes ?? "None"}";
    }

    #region ISearchable Implementation

    /// <summary>
    /// Gets all searchable text for this prescription
    /// </summary>
    public string GetSearchableText()
    {
        return $"{PrescriptionNumber} {DoctorName} {DoctorLicenseNumber} {PatientId} {Status}".ToLower();
    }

    /// <summary>
    /// Checks if prescription matches the search term
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

/// <summary>
/// Represents an individual medication item in a prescription
/// </summary>
public class PrescriptionItem : Entity
{
    /// <summary>
    /// Foreign key to Prescription
    /// </summary>
    public int PrescriptionId { get; private set; }

    /// <summary>
    /// Foreign key to Medication
    /// </summary>
    public int MedicationId { get; private set; }

    /// <summary>
    /// Navigation property to Medication
    /// </summary>
    public virtual Medication? Medication { get; private set; }

    /// <summary>
    /// Quantity prescribed
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Dosage (e.g., "500mg", "10ml")
    /// </summary>
    public string Dosage { get; private set; } = string.Empty;

    /// <summary>
    /// Frequency (e.g., "3 times daily")
    /// </summary>
    public string Frequency { get; private set; } = string.Empty;

    /// <summary>
    /// Duration (e.g., "10 days", "2 weeks")
    /// </summary>
    public string Duration { get; private set; } = string.Empty;

    /// <summary>
    /// Special instructions
    /// </summary>
    public string? Instructions { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private PrescriptionItem() : base()
    {
    }

    /// <summary>
    /// Constructor for prescription item
    /// </summary>
    public PrescriptionItem(int prescriptionId, int medicationId, int quantity,
                           string dosage, string frequency, string duration,
                           string? instructions = null)
    {
        if (prescriptionId <= 0)
            throw new ArgumentException("Prescription ID must be positive", nameof(prescriptionId));

        if (medicationId <= 0)
            throw new ArgumentException("Medication ID must be positive", nameof(medicationId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (string.IsNullOrWhiteSpace(dosage))
            throw new ArgumentException("Dosage cannot be empty", nameof(dosage));

        if (string.IsNullOrWhiteSpace(frequency))
            throw new ArgumentException("Frequency cannot be empty", nameof(frequency));

        if (string.IsNullOrWhiteSpace(duration))
            throw new ArgumentException("Duration cannot be empty", nameof(duration));

        PrescriptionId = prescriptionId;
        MedicationId = medicationId;
        Quantity = quantity;
        Dosage = dosage;
        Frequency = frequency;
        Duration = duration;
        Instructions = instructions;
    }

    /// <summary>
    /// Copy constructor for prescription item
    /// </summary>
    public PrescriptionItem(PrescriptionItem other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        MedicationId = other.MedicationId;
        Quantity = other.Quantity;
        Dosage = other.Dosage;
        Frequency = other.Frequency;
        Duration = other.Duration;
        Instructions = other.Instructions;
    }
}
