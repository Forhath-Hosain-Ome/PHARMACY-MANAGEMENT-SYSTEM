# Pharmacy Management System - Console Application

## 🎯 Overview

This is a fully functional console application that demonstrates all required OOP concepts in an interactive menu-driven interface.

## ✨ Features

### Complete Menu System
- **Medication Management**: Add, search, view, update, and delete medications
- **Inventory Management**: Track stock, add/remove inventory, view reports
- **Prescription Management**: Handle prescriptions (framework ready)
- **Transaction Management**: Process sales (framework ready)
- **Patient Management**: View registered patients
- **Pharmacist Management**: View active pharmacists
- **Supplier Management**: View suppliers
- **Reports & Statistics**: Comprehensive reporting system
- **OOP Demonstrations**: Interactive demos of all OOP concepts

### Key Highlights

1. **Function Overloading Demonstrations**:
   - `MedicationService.Search()` - 5 overloaded versions
   - `InventoryService.AddStock()` - 5 overloaded versions
   - Various `ConsoleHelper` methods

2. **Operator Overloading in Action**:
   - Inventory stock operations: `inventory + 50`, `inventory - 20`
   - Comparisons: `inventory < 100`, `inventory > 50`
   - Interactive demonstration available in menu

3. **Sample Data**:
   - Pre-loaded with patients, pharmacists, medications, and suppliers
   - Ready to explore immediately

## 🚀 Running the Application

### Prerequisites
- .NET 8 SDK installed
- Terminal/Command Prompt

### Method 1: Using dotnet CLI

```bash
# Navigate to the console project directory
cd src/PharmacySystem.Console

# Run the application
dotnet run
```

### Method 2: Build and Run

```bash
# Build the project
dotnet build

# Run the executable
cd bin/Debug/net8.0
./PharmacySystem.Console    # Linux/Mac
PharmacySystem.Console.exe  # Windows
```

### Method 3: From Solution Root

```bash
# From the PharmacySystem root directory
dotnet run --project src/PharmacySystem.Console
```

## 📋 Menu Navigation

### Main Menu Options:

1. **Medication Management**
   - Add new prescription and OTC medicines
   - Search using 5 different overloaded methods
   - View, update, and delete medications
   - View statistics

2. **Inventory Management**
   - View all inventory
   - Add/remove stock using overloaded methods
   - Update reorder settings
   - View low stock and reorder reports
   - **Operator Overloading Demo** (highly recommended!)

3. **OOP Demonstrations** (Menu Option 9)
   - Inheritance hierarchy visualization
   - Interface implementation examples
   - Constructor overloading explanation
   - Copy constructor demonstration
   - Operator overloading interactive demo
   - Function overloading examples
   - Static methods & fields showcase
   - Polymorphism demonstration

## 🎓 OOP Concepts in Action

### Where to See Each Concept:

#### 1. Function Overloading
- **Location**: Medication Management → Search Medications
- **Demo**: Try searching by name, category, price range, or combinations
- Shows 5 different `Search()` method signatures working seamlessly

#### 2. Operator Overloading
- **Location**: Inventory Management → Demonstrate Operator Overloading
- **Demo**: Watch as `inventory + 50` adds stock, `inventory - 20` removes stock
- See comparison operators in action: `inventory < 100`

#### 3. Constructor Overloading
- **Location**: OOP Demonstrations → Constructor Overloading Demo
- **Shows**: How Medication has 5 constructor versions, Inventory has 5 versions

#### 4. Copy Constructor
- **Location**: OOP Demonstrations → Copy Constructor Demo
- **Shows**: How medications and prescriptions can be copied for refills

#### 5. Inheritance
- **Location**: OOP Demonstrations → Inheritance Hierarchy Demo
- **Shows**: 4-level deep inheritance chains

#### 6. Interfaces
- **Location**: OOP Demonstrations → Interface Implementation Demo
- **Shows**: How IInventoryItem, IPrescribable, ISearchable work

#### 7. Static Methods & Fields
- **Location**: OOP Demonstrations → Static Methods & Fields Demo
- **Shows**: SystemConfig with 10+ static fields and 20+ static methods

#### 8. Polymorphism
- **Location**: OOP Demonstrations → Polymorphism Demo
- **Shows**: Different medication types in the same collection

## 📊 Sample Data Included

The application comes pre-loaded with:
- **3 Patients**: John Doe, Jane Smith, Bob Wilson
- **2 Pharmacists**: Sarah Johnson, Mike Brown
- **8 Medications**:
  - 4 Prescription Medicines (including controlled substances)
  - 4 OTC Medicines
- **3 Suppliers**: MediSupply Corp, PharmaDirect, GlobalMeds Inc
- **Inventory Records**: For all medications with varied stock levels

## 🎮 Recommended Exploration Path

1. **Start Here**: Main Menu → Option 10 (System Information)
   - Get an overview of the system

2. **View Sample Data**: 
   - Option 1 → View All Medications
   - Option 2 → View All Inventory
   - Option 5 → Patient Management

3. **Try Function Overloading**:
   - Option 1 → Search Medications
   - Try all 5 search methods

4. **See Operator Overloading**:
   - Option 2 → Demonstrate Operator Overloading
   - Watch the magic of `+` and `-` operators!

5. **Explore OOP Concepts**:
   - Option 9 → OOP Demonstrations
   - Go through all 8 demonstrations

6. **Add Your Own Data**:
   - Option 1 → Add New Prescription Medicine
   - Option 2 → Add Stock

## 💡 Code Highlights

### ConsoleHelper (Static Class)
```csharp
// All methods are static - no instance needed
ConsoleHelper.PrintSuccess("Operation completed!");
int choice = ConsoleHelper.GetIntInput("Enter choice", 1, 10);
ConsoleHelper.DisplayTable(headers, rows);
```

### MedicationService (Function Overloading)
```csharp
// 5 different ways to search!
Search(string name)
Search(MedicationCategory category)
Search(string name, MedicationCategory category)
Search(decimal minPrice, decimal maxPrice)
Search(string name, decimal minPrice, decimal maxPrice)
```

### InventoryService (Operator Overloading Usage)
```csharp
// Adding stock using operator
inventory = inventory + 50;  // Calls operator+

// Removing stock
inventory = inventory - 20;  // Calls operator-

// Comparisons
if (inventory < 100) { ... }  // Calls operator<
```

## 🐛 Troubleshooting

### "dotnet: command not found"
- Install .NET 8 SDK from https://dotnet.microsoft.com/download

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

### Can't Find Project
```bash
# Make sure you're in the right directory
cd PharmacySystem/src/PharmacySystem.Console
dotnet run
```

## 📝 Notes

### In-Memory Storage
- This console app uses in-memory lists (not a database)
- Data is reset each time you restart the application
- Perfect for demonstration and testing OOP concepts

### Modular Design
- Core domain logic in `PharmacySystem.Core`
- Console UI and services in `PharmacySystem.Console`
- Easy to add Data layer with EF Core later

### Future Enhancements
The console application is ready to be extended with:
- Full Prescription workflow
- Complete Transaction processing
- Patient registration
- Pharmacist management
- Database persistence (EF Core + PostgreSQL)

## 🎯 Assignment Checklist

When demonstrating for your instructor, show:

- [x] **Classes**: Browse through the code, show 20+ classes
- [x] **Inheritance**: Menu 9 → Option 1 (4-level hierarchies)
- [x] **Interfaces**: Menu 9 → Option 2 (4 interfaces)
- [x] **Constructors**: Every class has them
- [x] **Constructor Overloading**: Menu 9 → Option 3
- [x] **Copy Constructors**: Menu 9 → Option 4
- [x] **Operator Overloading**: Menu 2 → Option 8 (Interactive!)
- [x] **Function Overloading**: Menu 1 → Option 3 (5 Search methods)
- [x] **Static Methods & Fields**: Menu 9 → Option 7 (SystemConfig)

## 🎉 Enjoy!

This console application is a complete demonstration of professional C# OOP principles. Explore the menus, try the demonstrations, and see how everything works together!

For questions or issues, refer to the main project documentation or the code comments.

**Happy coding!** 🚀
