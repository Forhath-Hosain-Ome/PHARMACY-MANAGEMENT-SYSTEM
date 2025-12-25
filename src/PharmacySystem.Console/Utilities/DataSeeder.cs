using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.People;
using PharmacySystem.Core.Entities.Operations;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Console.Utilities;

/// <summary>
/// Static class for seeding initial sample data
/// Demonstrates static methods for data initialization
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Seeds sample patients
    /// </summary>
    public static List<Patient> SeedPatients()
    {
        var patients = new List<Patient>
        {
            new Patient(
                "john.doe", "john.doe@email.com", "John", "Doe",
                new DateTime(1990, 5, 15), "555-0101", "INS123456"
            ),
            new Patient(
                "jane.smith", "jane.smith@email.com", "Jane", "Smith",
                new DateTime(1985, 8, 22), "555-0102", "INS789012"
            ),
            new Patient(
                "bob.wilson", "bob.wilson@email.com", "Bob", "Wilson",
                new DateTime(1978, 3, 10), "555-0103", "INS345678"
            )
        };

        // Update some patient details
        patients[0].UpdateAllergies("Penicillin");
        patients[1].UpdateMedicalHistory("Diabetes Type 2");
        patients[2].UpdateEmergencyContact("Alice Wilson", "555-0199");

        return patients;
    }

    /// <summary>
    /// Seeds sample pharmacists
    /// </summary>
    public static List<Pharmacist> SeedPharmacists()
    {
        var pharmacists = new List<Pharmacist>
        {
            new Pharmacist(
                "sarah.johnson", "sarah.j@pharmacy.com",
                "Sarah", "Johnson", new DateTime(1988, 7, 12),
                "555-0201", "EMP-001", new DateTime(2015, 1, 15),
                75000m, "LIC-PH-12345", DateTime.Now.AddYears(2),
                "Clinical Pharmacy"
            ),
            new Pharmacist(
                "mike.brown", "mike.b@pharmacy.com",
                "Mike", "Brown", new DateTime(1992, 11, 5),
                "555-0202", "EMP-002", new DateTime(2018, 6, 1),
                68000m, "LIC-PH-67890", DateTime.Now.AddYears(3),
                "Compounding"
            )
        };

        return pharmacists;
    }

    /// <summary>
    /// Seeds sample prescription medications
    /// </summary>
    public static List<PrescriptionMedicine> SeedPrescriptionMedicines()
    {
        var medications = new List<PrescriptionMedicine>
        {
            new PrescriptionMedicine(
                "Amoxicillin", "Amoxicillin Trihydrate",
                "Antibiotic for bacterial infections",
                new Money(15.99m),
                MedicationCategory.Antibiotic,
                "PharmaCorp Inc.",
                DateTime.Now.AddYears(2),
                "BATCH-001",
                "Capsule", "500mg",
                false, 5
            ),
            new PrescriptionMedicine(
                "Lisinopril", "Lisinopril",
                "ACE inhibitor for high blood pressure",
                new Money(12.50m),
                MedicationCategory.Cardiovascular,
                "CardioMed",
                DateTime.Now.AddYears(1).AddMonths(6),
                "BATCH-002",
                "Tablet", "10mg",
                false, 5
            ),
            new PrescriptionMedicine(
                "Metformin", "Metformin Hydrochloride",
                "Oral diabetes medication",
                new Money(8.99m),
                MedicationCategory.Endocrine,
                "DiabetesCare",
                DateTime.Now.AddYears(1).AddMonths(8),
                "BATCH-003",
                "Tablet", "500mg",
                false, 5
            ),
            new PrescriptionMedicine(
                "Alprazolam", "Alprazolam",
                "Benzodiazepine for anxiety (CONTROLLED)",
                new Money(25.00m),
                MedicationCategory.Neurological,
                "MindPharm",
                DateTime.Now.AddYears(1),
                "BATCH-004",
                "Tablet", "0.5mg",
                true, 2 // Controlled substance
            )
        };

        medications[3].UpdateWarnings("CONTROLLED SUBSTANCE - Schedule IV. May cause drowsiness. Do not operate machinery.");

        return medications;
    }

    /// <summary>
    /// Seeds sample OTC medications
    /// </summary>
    public static List<OTCMedicine> SeedOTCMedicines()
    {
        var medications = new List<OTCMedicine>
        {
            new OTCMedicine(
                "Ibuprofen", "Ibuprofen",
                "Pain reliever and fever reducer",
                new Money(9.99m),
                MedicationCategory.Painkiller,
                "PainRelief Co.",
                DateTime.Now.AddYears(3),
                "BATCH-OTC-001",
                "Take 1-2 tablets every 4-6 hours as needed. Do not exceed 6 tablets in 24 hours.",
                "Ibuprofen 200mg",
                "Do not use if you have stomach ulcers. Consult doctor if symptoms persist.",
                12 // Age restriction
            ),
            new OTCMedicine(
                "Vitamin D3", "Cholecalciferol",
                "Vitamin D supplement",
                new Money(14.99m),
                MedicationCategory.Vitamin,
                "VitaHealth",
                DateTime.Now.AddYears(2),
                "BATCH-OTC-002",
                "Take 1 tablet daily with food.",
                "Vitamin D3 1000 IU",
                "Consult healthcare provider if pregnant or nursing.",
                0 // No age restriction
            ),
            new OTCMedicine(
                "Omeprazole", "Omeprazole",
                "Proton pump inhibitor for heartburn",
                new Money(18.50m),
                MedicationCategory.Gastrointestinal,
                "DigestWell",
                DateTime.Now.AddYears(1).AddMonths(9),
                "BATCH-OTC-003",
                "Take 1 tablet daily before breakfast for 14 days.",
                "Omeprazole 20mg",
                "Do not use for more than 14 days unless directed by doctor.",
                18 // Age restriction
            ),
            new OTCMedicine(
                "Acetaminophen", "Acetaminophen",
                "Pain reliever and fever reducer",
                new Money(7.99m),
                MedicationCategory.Painkiller,
                "PainRelief Co.",
                DateTime.Now.AddYears(2).AddMonths(6),
                "BATCH-OTC-004",
                "Adults: Take 2 tablets every 4-6 hours. Do not exceed 8 tablets in 24 hours.",
                "Acetaminophen 325mg",
                "Severe liver damage may occur if you take more than 4000mg in 24 hours.",
                6 // Age restriction
            )
        };

        return medications;
    }

    /// <summary>
    /// Seeds sample inventory for medications
    /// </summary>
    public static List<Inventory> SeedInventory(List<Medication> medications)
    {
        var inventories = new List<Inventory>();
        var random = new Random();

        foreach (var medication in medications)
        {
            int quantity = random.Next(10, 200);
            int reorderLevel = random.Next(20, 60);
            int reorderQuantity = random.Next(50, 150);
            string location = $"Shelf-{random.Next(1, 10)}-{(char)('A' + random.Next(0, 5))}";

            var inventory = new Inventory(
                medication.Id,
                quantity,
                reorderLevel,
                reorderQuantity,
                location
            );

            inventories.Add(inventory);
        }

        return inventories;
    }

    /// <summary>
    /// Seeds sample suppliers
    /// </summary>
    public static List<Supplier> SeedSuppliers()
    {
        var suppliers = new List<Supplier>
        {
            new Supplier(
                "MediSupply Corp",
                "Tom Anderson",
                "contact@medisupply.com",
                "555-1000",
                new Address("123 Medical Plaza", "New York", "NY", "10001", "USA")
            ),
            new Supplier(
                "PharmaDirect",
                "Lisa Chen",
                "info@pharmadirect.com",
                "555-2000",
                new Address("456 Health Street", "Los Angeles", "CA", "90001", "USA")
            ),
            new Supplier(
                "GlobalMeds Inc",
                "Robert Smith",
                "sales@globalmeds.com",
                "555-3000",
                new Address("789 Pharma Avenue", "Chicago", "IL", "60601", "USA")
            )
        };

        suppliers[0].UpdateRating(4.5m);
        suppliers[1].UpdateRating(4.8m);
        suppliers[2].UpdateRating(4.2m);

        suppliers[0].UpdateWebsite("https://www.medisupply.com");
        suppliers[1].UpdateWebsite("https://www.pharmadirect.com");
        suppliers[2].UpdateWebsite("https://www.globalmeds.com");

        return suppliers;
    }

    /// <summary>
    /// Gets a welcome message
    /// </summary>
    public static string GetWelcomeMessage()
    {
        return @"
╔═══════════════════════════════════════════════════════════════╗
║                                                               ║
║          PHARMACY MANAGEMENT SYSTEM v1.0                      ║
║                                                               ║
║          A comprehensive OOP-based system for                 ║
║          managing pharmacy operations                         ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝

Welcome to the Pharmacy Management System!

This system demonstrates all key OOP concepts:
• Classes & Inheritance (4-level deep hierarchies)
• Interfaces (IInventoryItem, IPrescribable, ISearchable)
• Constructor & Function Overloading
• Copy Constructors (Medication, Prescription)
• Operator Overloading (Inventory, Money)
• Static Methods & Fields (SystemConfig)

Sample data has been loaded for demonstration purposes.
";
    }

    /// <summary>
    /// Gets system statistics message
    /// </summary>
    public static string GetSystemStats(int patients, int pharmacists, int medications, int prescriptions, int transactions)
    {
        return $@"
System Statistics:
─────────────────────────────────────────────────────────────
  Registered Patients:     {patients,6}
  Active Pharmacists:      {pharmacists,6}
  Total Medications:       {medications,6}
  Prescriptions Issued:    {prescriptions,6}
  Transactions Completed:  {transactions,6}
─────────────────────────────────────────────────────────────
";
    }
}
