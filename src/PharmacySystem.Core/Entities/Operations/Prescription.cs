using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.People;
using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Interfaces;

namespace PharmacySystem.Core.Entities.Operations;

public class Prescription : Entity, ISearchable
{
    public string PrescriptionNumber { get; private set; } = string.Empty;
    public int PatientId { get; private set; }
    public virtual Patient? Patient { get; private set; }
    public int PharmacistId { get; private set; }
    public virtual Pharmacist? Pharmacist { get; private set; }
    public DateTime PrescriptionDate { get; private set; }
    public PrescriptionStatus Status { get; private set; }
    public string? DoctorName { get; private set; }
    public string? DoctorLicenseNumber { get; private set; }
    public virtual ICollection<PrescriptionItem> Items { get; private set; } = new List<PrescriptionItem>();

    public string? Notes { get; private set; }

    private Prescription() : base()
    {
    }

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


    public Prescription(int patientId, int pharmacistId, string doctorName, 
                       string doctorLicenseNumber)
        : this(patientId, pharmacistId)
    {
        DoctorName = doctorName;
        DoctorLicenseNumber = doctorLicenseNumber;
    }


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


    private static string GeneratePrescriptionNumber()
    {
        return $"RX-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }


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


    public void Cancel(string reason)
    {
        if (Status == PrescriptionStatus.Dispensed)
            throw new InvalidOperationException("Cannot cancel a dispensed prescription");

        Status = PrescriptionStatus.Cancelled;
        Notes = $"Cancelled: {reason}";
        UpdateTimestamp();
    }


    public Money GetTotalCost()
    {
        if (!Items.Any())
            return new Money(0);

        // This would need to be calculated with actual medication prices from database
        // For now, returning a placeholder
        return new Money(0);
    }


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


    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdateTimestamp();
    }


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


    public string GetSearchableText()
    {
        return $"{PrescriptionNumber} {DoctorName} {DoctorLicenseNumber} {PatientId} {Status}".ToLower();
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


public class PrescriptionItem : Entity
{
    public int PrescriptionId { get; private set; }
    public int MedicationId { get; private set; }
    public virtual Medication? Medication { get; private set; }
    public int Quantity { get; private set; }
    public string Dosage { get; private set; } = string.Empty;
    public string Frequency { get; private set; } = string.Empty;
    public string Duration { get; private set; } = string.Empty;
    public string? Instructions { get; private set; }
    private PrescriptionItem() : base()
    {
    }

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
