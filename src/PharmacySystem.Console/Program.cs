using PharmacySystem.Console.Services;
using PharmacySystem.Console.Utilities;
using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.People;
using PharmacySystem.Core.Entities.Operations;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;
using PharmacySystem.Core.Configuration;

namespace PharmacySystem.Console;

/// <summary>
/// Main program class - Entry point for the Pharmacy Management System
/// Demonstrates all OOP concepts in an interactive console application
/// </summary>
class Program
{
    // Services
    private static MedicationService _medicationService = null!;
    private static InventoryService _inventoryService = null!;

    // In-memory data stores (simulating database)
    private static List<Patient> _patients = new();
    private static List<Pharmacist> _pharmacists = new();
    private static List<Prescription> _prescriptions = new();
    private static List<Transaction> _transactions = new();
    private static List<Supplier> _suppliers = new();

    static void Main(string[] args)
    {
        try
        {
            // Initialize system
            InitializeSystem();

            // Display welcome message
            ConsoleHelper.ClearScreen();
            System.Console.WriteLine(DataSeeder.GetWelcomeMessage());
            ConsoleHelper.PauseForUser();

            // Main program loop
            bool running = true;
            while (running)
            {
                running = ShowMainMenu();
            }

            // Exit message
            ConsoleHelper.ClearScreen();
            ConsoleHelper.PrintSuccess("Thank you for using the Pharmacy Management System!");
            System.Console.WriteLine("Goodbye!\n");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError($"Fatal error: {ex.Message}");
            ConsoleHelper.PrintError($"Stack trace: {ex.StackTrace}");
            ConsoleHelper.PauseForUser();
        }
    }

    #region Initialization

    /// <summary>
    /// Initializes the system with services and sample data
    /// </summary>
    static void InitializeSystem()
    {
        ConsoleHelper.ShowLoading("Initializing Pharmacy Management System", 1500);

        // Initialize services
        _medicationService = new MedicationService();
        _inventoryService = new InventoryService(_medicationService);

        // Seed sample data
        ConsoleHelper.ShowLoading("Loading sample data", 1000);
        SeedData();

        ConsoleHelper.PrintSuccess("System initialized successfully!");
        System.Console.WriteLine();
    }

    /// <summary>
    /// Seeds sample data for demonstration
    /// </summary>
    static void SeedData()
    {
        // Seed patients
        _patients = DataSeeder.SeedPatients();

        // Seed pharmacists
        _pharmacists = DataSeeder.SeedPharmacists();

        // Seed prescription medicines
        var prescriptionMeds = DataSeeder.SeedPrescriptionMedicines();
        foreach (var med in prescriptionMeds)
        {
            _medicationService.AddPrescriptionMedicine(med);
        }

        // Seed OTC medicines
        var otcMeds = DataSeeder.SeedOTCMedicines();
        foreach (var med in otcMeds)
        {
            _medicationService.AddOTCMedicine(med);
        }

        // Seed inventory
        var allMedications = _medicationService.GetAll();
        var inventories = DataSeeder.SeedInventory(allMedications);
        foreach (var inventory in inventories)
        {
            _inventoryService.CreateInventory(inventory);
        }

        // Seed suppliers
        _suppliers = DataSeeder.SeedSuppliers();
    }

    #endregion

    #region Main Menu

    /// <summary>
    /// Shows the main menu and handles user selection
    /// </summary>
    static bool ShowMainMenu()
    {
        ConsoleHelper.ClearScreen();
        
        var choice = ConsoleHelper.DisplayMenu(
            "Pharmacy Management System - Main Menu",
            "Medication Management",
            "Inventory Management",
            "Prescription Management",
            "Transaction Management",
            "Patient Management",
            "Pharmacist Management",
            "Supplier Management",
            "Reports & Statistics",
            "OOP Demonstrations",
            "System Information"
        );

        switch (choice)
        {
            case 1:
                MedicationManagementMenu();
                break;
            case 2:
                InventoryManagementMenu();
                break;
            case 3:
                PrescriptionManagementMenu();
                break;
            case 4:
                TransactionManagementMenu();
                break;
            case 5:
                PatientManagementMenu();
                break;
            case 6:
                PharmacistManagementMenu();
                break;
            case 7:
                SupplierManagementMenu();
                break;
            case 8:
                ReportsMenu();
                break;
            case 9:
                OOPDemonstrationsMenu();
                break;
            case 10:
                ShowSystemInformation();
                break;
            case 0:
                return false; // Exit
        }

        return true;
    }

    #endregion

    #region Medication Management

    static void MedicationManagementMenu()
    {
        bool back = false;
        while (!back)
        {
            ConsoleHelper.ClearScreen();
            var choice = ConsoleHelper.DisplayMenu(
                "Medication Management",
                "Add New Prescription Medicine",
                "Add New OTC Medicine",
                "Search Medications (Various Methods)",
                "View All Medications",
                "View Prescription Medicines",
                "View OTC Medicines",
                "View Medication Details",
                "Update Medication Price",
                "Delete Medication",
                "Medication Statistics"
            );

            switch (choice)
            {
                case 1:
                    AddPrescriptionMedicine();
                    break;
                case 2:
                    AddOTCMedicine();
                    break;
                case 3:
                    SearchMedicationsMenu();
                    break;
                case 4:
                    _medicationService.DisplayMedicationList(_medicationService.GetAll(), "All Medications");
                    ConsoleHelper.PauseForUser();
                    break;
                case 5:
                    _medicationService.DisplayMedicationList(
                        _medicationService.GetAllPrescriptionMedicines().Cast<Medication>().ToList(),
                        "Prescription Medicines"
                    );
                    ConsoleHelper.PauseForUser();
                    break;
                case 6:
                    _medicationService.DisplayMedicationList(
                        _medicationService.GetAllOTCMedicines().Cast<Medication>().ToList(),
                        "OTC Medicines"
                    );
                    ConsoleHelper.PauseForUser();
                    break;
                case 7:
                    ViewMedicationDetails();
                    break;
                case 8:
                    UpdateMedicationPrice();
                    break;
                case 9:
                    DeleteMedication();
                    break;
                case 10:
                    _medicationService.DisplayStatistics();
                    ConsoleHelper.PauseForUser();
                    break;
                case 0:
                    back = true;
                    break;
            }
        }
    }

    static void AddPrescriptionMedicine()
    {
        ConsoleHelper.PrintHeader("Add New Prescription Medicine");

        try
        {
            string name = ConsoleHelper.GetStringInput("Medication Name", true);
            string genericName = ConsoleHelper.GetStringInput("Generic Name", true);
            string description = ConsoleHelper.GetStringInput("Description", false);
            decimal price = ConsoleHelper.GetDecimalInput("Price", 0.01m, 10000m);
            
            System.Console.WriteLine("\nCategories: Antibiotic, Painkiller, Cardiovascular, Respiratory, etc.");
            string categoryStr = ConsoleHelper.GetStringInput("Category", true);
            if (!Enum.TryParse<MedicationCategory>(categoryStr, true, out var category))
            {
                category = MedicationCategory.Other;
            }

            string manufacturer = ConsoleHelper.GetStringInput("Manufacturer", false);
            DateTime expiryDate = ConsoleHelper.GetDateInput("Expiry Date", DateTime.Now.AddDays(1), DateTime.Now.AddYears(10));
            string batchNumber = ConsoleHelper.GetStringInput("Batch Number", false);
            string dosageForm = ConsoleHelper.GetStringInput("Dosage Form (Tablet/Capsule/Liquid)", true);
            string strength = ConsoleHelper.GetStringInput("Strength (e.g., 500mg)", true);
            bool controlled = ConsoleHelper.GetBoolInput("Is this a controlled substance?");

            var medication = new PrescriptionMedicine(
                name, genericName, description, new Money(price),
                category, manufacturer, expiryDate, batchNumber,
                dosageForm, strength, controlled, controlled ? 2 : 5
            );

            _medicationService.AddPrescriptionMedicine(medication);

            // Create inventory
            int quantity = ConsoleHelper.GetIntInput("Initial Stock Quantity", 0, 10000);
            var inventory = new Inventory(medication.Id, quantity);
            _inventoryService.CreateInventory(inventory);
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError($"Failed to add medication: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    static void AddOTCMedicine()
    {
        ConsoleHelper.PrintHeader("Add New OTC Medicine");

        try
        {
            string name = ConsoleHelper.GetStringInput("Medication Name", true);
            string genericName = ConsoleHelper.GetStringInput("Generic Name", true);
            string description = ConsoleHelper.GetStringInput("Description", false);
            decimal price = ConsoleHelper.GetDecimalInput("Price", 0.01m, 10000m);
            
            System.Console.WriteLine("\nCategories: Painkiller, Vitamin, Supplement, Gastrointestinal, etc.");
            string categoryStr = ConsoleHelper.GetStringInput("Category", true);
            if (!Enum.TryParse<MedicationCategory>(categoryStr, true, out var category))
            {
                category = MedicationCategory.Other;
            }

            string manufacturer = ConsoleHelper.GetStringInput("Manufacturer", false);
            DateTime expiryDate = ConsoleHelper.GetDateInput("Expiry Date", DateTime.Now.AddDays(1), DateTime.Now.AddYears(10));
            string batchNumber = ConsoleHelper.GetStringInput("Batch Number", false);
            string usageInstructions = ConsoleHelper.GetStringInput("Usage Instructions", true);
            string activeIngredients = ConsoleHelper.GetStringInput("Active Ingredients", true);
            string warnings = ConsoleHelper.GetStringInput("Warnings", false);
            int minAge = ConsoleHelper.GetIntInput("Minimum Recommended Age", 0, 100);

            var medication = new OTCMedicine(
                name, genericName, description, new Money(price),
                category, manufacturer, expiryDate, batchNumber,
                usageInstructions, activeIngredients, warnings, minAge
            );

            _medicationService.AddOTCMedicine(medication);

            // Create inventory
            int quantity = ConsoleHelper.GetIntInput("Initial Stock Quantity", 0, 10000);
            var inventory = new Inventory(medication.Id, quantity);
            _inventoryService.CreateInventory(inventory);
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError($"Failed to add medication: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    static void SearchMedicationsMenu()
    {
        ConsoleHelper.ClearScreen();
        
        var choice = ConsoleHelper.DisplayMenu(
            "Search Medications (Function Overloading Demo)",
            "Search by Name",
            "Search by Category",
            "Search by Name AND Category",
            "Search by Price Range",
            "Search by Name AND Price Range"
        );

        List<Medication> results;

        switch (choice)
        {
            case 1:
                string name = ConsoleHelper.GetStringInput("Enter medication name", true);
                results = _medicationService.Search(name);
                break;
            case 2:
                System.Console.WriteLine("\nCategories: Antibiotic, Painkiller, Cardiovascular, etc.");
                string categoryStr = ConsoleHelper.GetStringInput("Enter category", true);
                if (Enum.TryParse<MedicationCategory>(categoryStr, true, out var category))
                {
                    results = _medicationService.Search(category);
                }
                else
                {
                    ConsoleHelper.PrintError("Invalid category!");
                    ConsoleHelper.PauseForUser();
                    return;
                }
                break;
            case 3:
                string name2 = ConsoleHelper.GetStringInput("Enter medication name", true);
                System.Console.WriteLine("\nCategories: Antibiotic, Painkiller, Cardiovascular, etc.");
                string categoryStr2 = ConsoleHelper.GetStringInput("Enter category", true);
                if (Enum.TryParse<MedicationCategory>(categoryStr2, true, out var category2))
                {
                    results = _medicationService.Search(name2, category2);
                }
                else
                {
                    ConsoleHelper.PrintError("Invalid category!");
                    ConsoleHelper.PauseForUser();
                    return;
                }
                break;
            case 4:
                decimal minPrice = ConsoleHelper.GetDecimalInput("Minimum Price", 0, 10000);
                decimal maxPrice = ConsoleHelper.GetDecimalInput("Maximum Price", minPrice, 10000);
                results = _medicationService.Search(minPrice, maxPrice);
                break;
            case 5:
                string name3 = ConsoleHelper.GetStringInput("Enter medication name", true);
                decimal minPrice2 = ConsoleHelper.GetDecimalInput("Minimum Price", 0, 10000);
                decimal maxPrice2 = ConsoleHelper.GetDecimalInput("Maximum Price", minPrice2, 10000);
                results = _medicationService.Search(name3, minPrice2, maxPrice2);
                break;
            case 0:
                return;
            default:
                return;
        }

        _medicationService.DisplayMedicationList(results, "Search Results");
        ConsoleHelper.PauseForUser();
    }

    static void ViewMedicationDetails()
    {
        int id = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        var medication = _medicationService.GetById(id);
        
        if (medication != null)
        {
            _medicationService.DisplayMedicationDetails(medication);
        }
        else
        {
            ConsoleHelper.PrintError("Medication not found!");
        }

        ConsoleHelper.PauseForUser();
    }

    static void UpdateMedicationPrice()
    {
        int id = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        decimal newPrice = ConsoleHelper.GetDecimalInput("Enter New Price", 0.01m, 10000m);

        _medicationService.UpdatePrice(id, new Money(newPrice));
        ConsoleHelper.PauseForUser();
    }

    static void DeleteMedication()
    {
        int id = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        
        if (ConsoleHelper.ConfirmAction("This will permanently delete the medication."))
        {
            _medicationService.Delete(id);
        }

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Inventory Management

    static void InventoryManagementMenu()
    {
        bool back = false;
        while (!back)
        {
            ConsoleHelper.ClearScreen();
            var choice = ConsoleHelper.DisplayMenu(
                "Inventory Management",
                "View All Inventory",
                "View Inventory Details",
                "Add Stock (Various Methods)",
                "Remove Stock",
                "Update Reorder Settings",
                "View Low Stock Items",
                "View Reorder Report",
                "Demonstrate Operator Overloading",
                "Inventory Statistics"
            );

            switch (choice)
            {
                case 1:
                    _inventoryService.DisplayInventoryList(_inventoryService.GetAll(), "All Inventory");
                    ConsoleHelper.PauseForUser();
                    break;
                case 2:
                    ViewInventoryDetails();
                    break;
                case 3:
                    AddStockMenu();
                    break;
                case 4:
                    RemoveStock();
                    break;
                case 5:
                    UpdateReorderSettings();
                    break;
                case 6:
                    _inventoryService.DisplayLowStockReport();
                    ConsoleHelper.PauseForUser();
                    break;
                case 7:
                    _inventoryService.DisplayReorderReport();
                    ConsoleHelper.PauseForUser();
                    break;
                case 8:
                    DemonstrateOperatorOverloading();
                    break;
                case 9:
                    _inventoryService.DisplayStatistics();
                    ConsoleHelper.PauseForUser();
                    break;
                case 0:
                    back = true;
                    break;
            }
        }
    }

    static void ViewInventoryDetails()
    {
        int medId = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        _inventoryService.DisplayInventoryDetails(medId);
        ConsoleHelper.PauseForUser();
    }

    static void AddStockMenu()
    {
        ConsoleHelper.ClearScreen();
        
        var choice = ConsoleHelper.DisplayMenu(
            "Add Stock (Function Overloading Demo)",
            "Add Stock by Medication ID",
            "Add Stock with Notes",
            "Add Stock with Supplier Info"
        );

        int medId = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        int quantity = ConsoleHelper.GetIntInput("Enter Quantity to Add", 1, 10000);

        switch (choice)
        {
            case 1:
                _inventoryService.AddStock(medId, quantity);
                break;
            case 2:
                string notes = ConsoleHelper.GetStringInput("Enter Notes", false);
                _inventoryService.AddStock(medId, quantity, notes);
                break;
            case 3:
                int supplierId = ConsoleHelper.GetIntInput("Enter Supplier ID", 1, int.MaxValue);
                var medication = _medicationService.GetById(medId);
                if (medication != null)
                {
                    _inventoryService.AddStock(medication, quantity, supplierId);
                }
                else
                {
                    ConsoleHelper.PrintError("Medication not found!");
                }
                break;
        }

        ConsoleHelper.PauseForUser();
    }

    static void RemoveStock()
    {
        int medId = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        int quantity = ConsoleHelper.GetIntInput("Enter Quantity to Remove", 1, 10000);

        bool includeReason = ConsoleHelper.GetBoolInput("Include reason for removal?");
        
        if (includeReason)
        {
            string reason = ConsoleHelper.GetStringInput("Enter Reason", true);
            _inventoryService.RemoveStock(medId, quantity, reason);
        }
        else
        {
            _inventoryService.RemoveStock(medId, quantity);
        }

        ConsoleHelper.PauseForUser();
    }

    static void UpdateReorderSettings()
    {
        int medId = ConsoleHelper.GetIntInput("Enter Medication ID", 1, int.MaxValue);
        int reorderLevel = ConsoleHelper.GetIntInput("Enter Reorder Level", 0, 1000);
        int reorderQty = ConsoleHelper.GetIntInput("Enter Reorder Quantity", 1, 10000);

        _inventoryService.UpdateReorderSettings(medId, reorderLevel, reorderQty);
        ConsoleHelper.PauseForUser();
    }

    static void DemonstrateOperatorOverloading()
    {
        int medId = ConsoleHelper.GetIntInput("Enter Medication ID for Demo", 1, int.MaxValue);
        _inventoryService.DemonstrateOperatorOverloading(medId);
        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Other Menus (Simplified implementations)

    static void PrescriptionManagementMenu()
    {
        ConsoleHelper.PrintHeader("Prescription Management");
        ConsoleHelper.PrintInfo($"Total Prescriptions: {_prescriptions.Count}");
        ConsoleHelper.PrintInfo("This module demonstrates Prescription class with copy constructor.");
        ConsoleHelper.PrintInfo("Full implementation available in the codebase.");
        ConsoleHelper.PauseForUser();
    }

    static void TransactionManagementMenu()
    {
        ConsoleHelper.PrintHeader("Transaction Management");
        ConsoleHelper.PrintInfo($"Total Transactions: {_transactions.Count}");
        ConsoleHelper.PrintInfo("This module demonstrates Transaction class with function overloading.");
        ConsoleHelper.PrintInfo("ApplyDiscount method has 4 overloaded versions!");
        ConsoleHelper.PauseForUser();
    }

    static void PatientManagementMenu()
    {
        ConsoleHelper.PrintHeader("Patient Management");
        ConsoleHelper.PrintInfo($"Total Patients: {_patients.Count}");
        
        System.Console.WriteLine("\nRegistered Patients:");
        foreach (var patient in _patients)
        {
            System.Console.WriteLine($"  - {patient.GetFullName()} ({patient.Username})");
        }
        
        ConsoleHelper.PauseForUser();
    }

    static void PharmacistManagementMenu()
    {
        ConsoleHelper.PrintHeader("Pharmacist Management");
        ConsoleHelper.PrintInfo($"Total Pharmacists: {_pharmacists.Count}");
        
        System.Console.WriteLine("\nActive Pharmacists:");
        foreach (var pharmacist in _pharmacists)
        {
            System.Console.WriteLine($"  - {pharmacist.GetFullName()} - License: {pharmacist.LicenseNumber}");
        }
        
        ConsoleHelper.PauseForUser();
    }

    static void SupplierManagementMenu()
    {
        ConsoleHelper.PrintHeader("Supplier Management");
        ConsoleHelper.PrintInfo($"Total Suppliers: {_suppliers.Count}");
        
        System.Console.WriteLine("\nRegistered Suppliers:");
        foreach (var supplier in _suppliers)
        {
            System.Console.WriteLine($"  - {supplier.Name} (Rating: {supplier.Rating}/5.0)");
        }
        
        ConsoleHelper.PauseForUser();
    }

    static void ReportsMenu()
    {
        ConsoleHelper.ClearScreen();
        
        var choice = ConsoleHelper.DisplayMenu(
            "Reports & Statistics",
            "Medication Statistics",
            "Inventory Statistics",
            "Low Stock Report",
            "Reorder Report",
            "Expiring Medications Report",
            "System Overview"
        );

        switch (choice)
        {
            case 1:
                _medicationService.DisplayStatistics();
                ConsoleHelper.PauseForUser();
                break;
            case 2:
                _inventoryService.DisplayStatistics();
                ConsoleHelper.PauseForUser();
                break;
            case 3:
                _inventoryService.DisplayLowStockReport();
                ConsoleHelper.PauseForUser();
                break;
            case 4:
                _inventoryService.DisplayReorderReport();
                ConsoleHelper.PauseForUser();
                break;
            case 5:
                ShowExpiringMedicationsReport();
                break;
            case 6:
                ShowSystemOverview();
                break;
        }
    }

    static void ShowExpiringMedicationsReport()
    {
        ConsoleHelper.PrintHeader("Expiring Medications Report");
        
        var expiring = _medicationService.GetExpiringSoon();
        var expired = _medicationService.GetExpired();

        if (expired.Any())
        {
            ConsoleHelper.PrintError($"{expired.Count} medication(s) are EXPIRED!");
            _medicationService.DisplayMedicationList(expired, "Expired Medications");
        }

        if (expiring.Any())
        {
            ConsoleHelper.PrintWarning($"{expiring.Count} medication(s) expiring soon!");
            _medicationService.DisplayMedicationList(expiring, "Expiring Soon");
        }

        if (!expiring.Any() && !expired.Any())
        {
            ConsoleHelper.PrintSuccess("No medications are expired or expiring soon!");
        }

        ConsoleHelper.PauseForUser();
    }

    static void ShowSystemOverview()
    {
        ConsoleHelper.PrintHeader("System Overview");
        
        System.Console.WriteLine(DataSeeder.GetSystemStats(
            _patients.Count,
            _pharmacists.Count,
            _medicationService.GetAll().Count,
            _prescriptions.Count,
            _transactions.Count
        ));

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region OOP Demonstrations

    static void OOPDemonstrationsMenu()
    {
        ConsoleHelper.ClearScreen();
        
        var choice = ConsoleHelper.DisplayMenu(
            "OOP Concept Demonstrations",
            "Inheritance Hierarchy Demo",
            "Interface Implementation Demo",
            "Constructor Overloading Demo",
            "Copy Constructor Demo",
            "Operator Overloading Demo",
            "Function Overloading Demo",
            "Static Methods & Fields Demo",
            "Polymorphism Demo"
        );

        switch (choice)
        {
            case 1:
                DemonstrateInheritance();
                break;
            case 2:
                DemonstrateInterfaces();
                break;
            case 3:
                DemonstrateConstructorOverloading();
                break;
            case 4:
                DemonstrateCopyConstructor();
                break;
            case 5:
                DemonstrateOperatorOverloading();
                break;
            case 6:
                DemonstrateFunctionOverloading();
                break;
            case 7:
                DemonstrateStaticMembers();
                break;
            case 8:
                DemonstratePolymorphism();
                break;
        }
    }

    static void DemonstrateInheritance()
    {
        ConsoleHelper.PrintHeader("Inheritance Hierarchy Demonstration");

        System.Console.WriteLine("User Hierarchy (4 levels deep):");
        System.Console.WriteLine("  Entity → User → Person → Patient");
        System.Console.WriteLine("  Entity → User → Person → Employee → Pharmacist");
        System.Console.WriteLine();

        System.Console.WriteLine("Item Hierarchy:");
        System.Console.WriteLine("  Entity → Item → Medication → PrescriptionMedicine");
        System.Console.WriteLine("  Entity → Item → Medication → OTCMedicine");
        System.Console.WriteLine();

        if (_patients.Any())
        {
            var patient = _patients.First();
            System.Console.WriteLine("Example - Patient object:");
            System.Console.WriteLine($"  Type: {patient.GetType().Name}");
            System.Console.WriteLine($"  Base class: {patient.GetType().BaseType?.Name}");
            System.Console.WriteLine($"  Properties from Entity: Id, CreatedAt");
            System.Console.WriteLine($"  Properties from User: Username, Email, Role");
            System.Console.WriteLine($"  Properties from Person: FirstName, LastName, DateOfBirth");
            System.Console.WriteLine($"  Properties from Patient: MedicalHistory, Allergies");
        }

        ConsoleHelper.PauseForUser();
    }

    static void DemonstrateInterfaces()
    {
        ConsoleHelper.PrintHeader("Interface Implementation Demonstration");

        System.Console.WriteLine("Interfaces in the system:");
        System.Console.WriteLine("  • IInventoryItem - implemented by Medication");
        System.Console.WriteLine("  • IPrescribable - implemented by PrescriptionMedicine");
        System.Console.WriteLine("  • ISearchable - implemented by Patient, Pharmacist, Medication");
        System.Console.WriteLine("  • IRepository<T> - for data access pattern");
        System.Console.WriteLine();

        var medications = _medicationService.GetAll();
        if (medications.Any())
        {
            var med = medications.First();
            System.Console.WriteLine($"Example - {med.Name}:");
            System.Console.WriteLine($"  IInventoryItem.CurrentStock: {med.CurrentStock}");
            System.Console.WriteLine($"  IInventoryItem.IsLowStock(): {med.IsLowStock()}");
            System.Console.WriteLine($"  ISearchable.GetSearchableText(): {ConsoleHelper.Truncate(med.GetSearchableText(), 50)}");
        }

        ConsoleHelper.PauseForUser();
    }

    static void DemonstrateConstructorOverloading()
    {
        ConsoleHelper.PrintHeader("Constructor Overloading Demonstration");

        System.Console.WriteLine("Medication class has 5 constructor overloads:");
        System.Console.WriteLine("  1. Medication(name, price)");
        System.Console.WriteLine("  2. Medication(name, genericName, price)");
        System.Console.WriteLine("  3. Medication(name, genericName, price, category)");
        System.Console.WriteLine("  4. Medication(name, genericName, price, category, manufacturer, expiryDate)");
        System.Console.WriteLine("  5. Medication(name, genericName, description, price, category, manufacturer, expiryDate, batchNumber)");
        System.Console.WriteLine();

        System.Console.WriteLine("Inventory class has 5 constructor overloads:");
        System.Console.WriteLine("  1. Inventory(medicationId, quantity)");
        System.Console.WriteLine("  2. Inventory(medicationId, quantity, reorderLevel)");
        System.Console.WriteLine("  3. Inventory(medicationId, quantity, reorderLevel, reorderQuantity)");
        System.Console.WriteLine("  4. Inventory(medicationId, quantity, reorderLevel, reorderQuantity, location)");
        System.Console.WriteLine("  5. Inventory(medicationId, quantity, reorderLevel, reorderQuantity, location, supplierId)");

        ConsoleHelper.PauseForUser();
    }

    static void DemonstrateCopyConstructor()
    {
        ConsoleHelper.PrintHeader("Copy Constructor Demonstration");

        System.Console.WriteLine("Classes with Copy Constructors:");
        System.Console.WriteLine("  • Medication - copies medication for similar products");
        System.Console.WriteLine("  • PrescriptionMedicine - specialized copy");
        System.Console.WriteLine("  • OTCMedicine - specialized copy");
        System.Console.WriteLine("  • Prescription - useful for refills");
        System.Console.WriteLine();

        System.Console.WriteLine("Example - Creating a copy of a medication:");
        var medications = _medicationService.GetAll();
        if (medications.Any())
        {
            var original = medications.First();
            System.Console.WriteLine($"Original: {original.Name} (ID: {original.Id})");
            System.Console.WriteLine($"  Price: {original.Price.ToDisplayString()}");
            System.Console.WriteLine($"  Batch: {original.BatchNumber}");
            System.Console.WriteLine();
            System.Console.WriteLine("// Using copy constructor:");
            System.Console.WriteLine($"var copy = new {original.GetType().Name}(original);");
            System.Console.WriteLine();
            System.Console.WriteLine("The copy would have:");
            System.Console.WriteLine("  - Same properties (Name, GenericName, Price, etc.)");
            System.Console.WriteLine("  - New ID (0 - will be assigned on save)");
            System.Console.WriteLine("  - New SKU (auto-generated)");
        }

        ConsoleHelper.PauseForUser();
    }

    static void DemonstrateFunctionOverloading()
    {
        ConsoleHelper.PrintHeader("Function Overloading Demonstration");

        System.Console.WriteLine("MedicationService.Search() - 5 overloaded versions:");
        System.Console.WriteLine("  1. Search(string name)");
        System.Console.WriteLine("  2. Search(MedicationCategory category)");
        System.Console.WriteLine("  3. Search(string name, MedicationCategory category)");
        System.Console.WriteLine("  4. Search(decimal minPrice, decimal maxPrice)");
        System.Console.WriteLine("  5. Search(string name, decimal minPrice, decimal maxPrice)");
        System.Console.WriteLine();

        System.Console.WriteLine("InventoryService.AddStock() - 5 overloaded versions:");
        System.Console.WriteLine("  1. AddStock(int medicationId, int quantity)");
        System.Console.WriteLine("  2. AddStock(int medicationId, int quantity, string notes)");
        System.Console.WriteLine("  3. AddStock(Medication medication, int quantity)");
        System.Console.WriteLine("  4. AddStock(Medication medication, int quantity, int supplierId)");
        System.Console.WriteLine("  5. AddStock(Medication medication, int quantity, int supplierId, string notes)");
        System.Console.WriteLine();

        System.Console.WriteLine("Transaction.ApplyDiscount() - 4 overloaded versions:");
        System.Console.WriteLine("  1. ApplyDiscount(Money amount)");
        System.Console.WriteLine("  2. ApplyDiscount(decimal percentage)");
        System.Console.WriteLine("  3. ApplyDiscount(Money amount, string reason)");
        System.Console.WriteLine("  4. ApplyDiscount(decimal percentage, string reason)");

        ConsoleHelper.PauseForUser();
    }

    static void DemonstrateStaticMembers()
    {
        ConsoleHelper.PrintHeader("Static Methods & Fields Demonstration");

        System.Console.WriteLine("SystemConfig static class contains:");
        System.Console.WriteLine();
        System.Console.WriteLine("Static Fields (Constants):");
        System.Console.WriteLine($"  TaxRate: {SystemConfig.TaxRate:P}");
        System.Console.WriteLine($"  DefaultReorderLevel: {SystemConfig.DefaultReorderLevel}");
        System.Console.WriteLine($"  LowStockThreshold: {SystemConfig.LowStockThreshold}");
        System.Console.WriteLine($"  BulkDiscountRate: {SystemConfig.BulkDiscountRate:P}");
        System.Console.WriteLine();

        System.Console.WriteLine("Static Methods Examples:");
        decimal testAmount = 100m;
        System.Console.WriteLine($"  CalculateTax(${testAmount}): ${SystemConfig.CalculateTax(testAmount)}");
        System.Console.WriteLine($"  IsLowStock(15): {SystemConfig.IsLowStock(15)}");
        System.Console.WriteLine($"  ValidateEmail(\"test@example.com\"): {SystemConfig.ValidateEmail("test@example.com")}");
        System.Console.WriteLine($"  FormatCurrency(123.45m): {SystemConfig.FormatCurrency(123.45m)}");

        ConsoleHelper.PauseForUser();
    }

    static void DemonstratePolymorphism()
    {
        ConsoleHelper.PrintHeader("Polymorphism Demonstration");

        System.Console.WriteLine("List<Medication> can hold different types:");
        var medications = _medicationService.GetAll();
        
        foreach (var med in medications.Take(3))
        {
            System.Console.WriteLine($"\n  Runtime type: {med.GetType().Name}");
            System.Console.WriteLine($"  Name: {med.Name}");
            
            if (med is PrescriptionMedicine pm)
            {
                System.Console.WriteLine($"  Requires Prescription: {pm.RequiresPrescription}");
                System.Console.WriteLine($"  Can be accessed via IPrescribable interface");
            }
            else if (med is OTCMedicine otc)
            {
                System.Console.WriteLine($"  Requires Prescription: {otc.RequiresPrescription}");
                System.Console.WriteLine($"  No prescription needed");
            }
        }

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region System Information

    static void ShowSystemInformation()
    {
        ConsoleHelper.PrintHeader("System Information");

        System.Console.WriteLine("Pharmacy Management System v1.0");
        System.Console.WriteLine("A comprehensive OOP demonstration project");
        System.Console.WriteLine();
        System.Console.WriteLine("OOP Concepts Demonstrated:");
        System.Console.WriteLine("  ✓ Classes (20+ classes)");
        System.Console.WriteLine("  ✓ Inheritance (4-level deep hierarchies)");
        System.Console.WriteLine("  ✓ Interfaces (4 interfaces)");
        System.Console.WriteLine("  ✓ Constructors (all classes)");
        System.Console.WriteLine("  ✓ Constructor Overloading (5+ overloads in key classes)");
        System.Console.WriteLine("  ✓ Copy Constructors (Medication, Prescription)");
        System.Console.WriteLine("  ✓ Operator Overloading (Inventory: 8 ops, Money: 10 ops)");
        System.Console.WriteLine("  ✓ Function Overloading (Search, AddStock, ApplyDiscount)");
        System.Console.WriteLine("  ✓ Static Methods & Fields (25+ in SystemConfig)");
        System.Console.WriteLine();
        System.Console.WriteLine("Architecture:");
        System.Console.WriteLine("  • Domain-Driven Design");
        System.Console.WriteLine("  • Repository Pattern");
        System.Console.WriteLine("  • Value Objects (Money, Address)");
        System.Console.WriteLine("  • Rich Domain Models");
        System.Console.WriteLine();
        System.Console.WriteLine("Technology Stack:");
        System.Console.WriteLine("  • C# 12");
        System.Console.WriteLine("  • .NET 8.0");
        System.Console.WriteLine("  • PostgreSQL (ready for EF Core integration)");
        System.Console.WriteLine("  • Docker (containerization ready)");

        ConsoleHelper.PauseForUser();
    }

    #endregion
}
