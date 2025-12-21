# Pharmacy & Medication Management System - Project Blueprint

## 📋 Table of Contents
1. [Project Overview](#project-overview)
2. [Project Structure](#project-structure)
3. [OOP Concepts Mapping](#oop-concepts-mapping)
4. [Class Diagrams & Relationships](#class-diagrams--relationships)
5. [Interface Definitions](#interface-definitions)
6. [Database Schema](#database-schema)
7. [Implementation Guidelines](#implementation-guidelines)
8. [Expansion Roadmap](#expansion-roadmap)

---

## 🎯 Project Overview

**Pharmacy & Medication Management System (Phase 1)**

A modular, expandable healthcare system starting with pharmacy and medication management. Built with OOP principles, PostgreSQL database, and Docker containerization.

### Core Features (Phase 1):
- Medication/Drug management (Prescription & OTC)
- Inventory tracking and stock management
- Supplier management
- Prescription handling
- Transaction/Billing system
- Search functionality across entities

### Technology Stack:
- **Language**: C# (.NET 8.0+)
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core
- **Containerization**: Docker & Docker Compose
- **Architecture**: Repository Pattern with modular structure

---

## 📁 Project Structure

```
PharmacySystem/
├── src/
│   ├── PharmacySystem.Core/              # Domain models, interfaces, enums
│   │   ├── Entities/
│   │   │   ├── Base/
│   │   │   │   ├── Entity.cs             # Base entity with Id, CreatedAt, etc.
│   │   │   │   ├── User.cs               # Abstract base for all users
│   │   │   │   └── Item.cs               # Abstract base for inventory items
│   │   │   ├── People/
│   │   │   │   ├── Person.cs             # Inherits from User
│   │   │   │   ├── Patient.cs            # Inherits from Person
│   │   │   │   ├── Employee.cs           # Inherits from Person
│   │   │   │   └── Pharmacist.cs         # Inherits from Employee
│   │   │   ├── Items/
│   │   │   │   ├── Medication.cs         # Inherits from Item
│   │   │   │   ├── PrescriptionMedicine.cs   # Inherits from Medication
│   │   │   │   └── OTCMedicine.cs        # Inherits from Medication
│   │   │   ├── Operations/
│   │   │   │   ├── Prescription.cs       # Prescription details
│   │   │   │   ├── Inventory.cs          # Stock management (Operator Overloading)
│   │   │   │   ├── Transaction.cs        # Sales/billing
│   │   │   │   └── Supplier.cs           # Supplier information
│   │   │   └── ValueObjects/
│   │   │       ├── Money.cs              # Money type (Operator Overloading)
│   │   │       └── Address.cs            # Address value object
│   │   ├── Interfaces/
│   │   │   ├── IInventoryItem.cs         # Inventory tracking
│   │   │   ├── IPrescribable.cs          # Prescribable items
│   │   │   ├── ISearchable.cs            # Search functionality
│   │   │   └── IRepository.cs            # Generic repository
│   │   ├── Enums/
│   │   │   ├── MedicationCategory.cs     # Drug categories
│   │   │   ├── PrescriptionStatus.cs
│   │   │   ├── TransactionStatus.cs
│   │   │   └── UserRole.cs
│   │   ├── Configuration/
│   │   │   └── SystemConfig.cs           # Static configuration (Static Fields)
│   │   └── PharmacySystem.Core.csproj
│   │
│   ├── PharmacySystem.Data/              # Database & Repository implementation
│   │   ├── Context/
│   │   │   └── PharmacyDbContext.cs      # EF Core DbContext
│   │   ├── Repositories/
│   │   │   ├── GenericRepository.cs      # Base repository implementation
│   │   │   ├── MedicationRepository.cs
│   │   │   ├── PrescriptionRepository.cs
│   │   │   ├── InventoryRepository.cs
│   │   │   ├── TransactionRepository.cs
│   │   │   └── UserRepository.cs
│   │   ├── Configurations/              # EF Core entity configurations
│   │   │   ├── MedicationConfiguration.cs
│   │   │   ├── PrescriptionConfiguration.cs
│   │   │   └── ...
│   │   ├── Migrations/                  # EF Core migrations
│   │   └── PharmacySystem.Data.csproj
│   │
│   └── PharmacySystem.Console/           # Console application (Main program)
│       ├── Program.cs                    # Entry point
│       ├── Services/
│       │   ├── MedicationService.cs
│       │   ├── InventoryService.cs
│       │   ├── PrescriptionService.cs
│       │   └── TransactionService.cs
│       ├── Utilities/
│       │   ├── ConsoleHelper.cs          # Static utility methods
│       │   └── DataSeeder.cs             # Sample data
│       └── PharmacySystem.Console.csproj
│
├── tests/                                # Unit tests (Optional for now)
│   └── PharmacySystem.Tests/
│
├── docker/
│   ├── docker-compose.yml
│   ├── .env.example
│   └── init.sql                          # Initial database setup
│
├── .gitignore
├── .dockerignore
├── PharmacySystem.sln                    # Solution file
└── README.md
```

---

## 🎓 OOP Concepts Mapping

### 1. **Classes**
- ✅ Entity (base class)
- ✅ User (abstract base)
- ✅ Person, Patient, Employee, Pharmacist
- ✅ Item (abstract base)
- ✅ Medication, PrescriptionMedicine, OTCMedicine
- ✅ Prescription, Inventory, Transaction, Supplier
- ✅ Money, Address (value objects)

### 2. **Inheritance**
```
User (abstract)
└── Person (abstract)
    ├── Patient
    └── Employee (abstract)
        └── Pharmacist

Item (abstract)
└── Medication (abstract)
    ├── PrescriptionMedicine
    └── OTCMedicine
```

### 3. **Interfaces**
- ✅ **IInventoryItem**: Implemented by Medication
- ✅ **IPrescribable**: Implemented by PrescriptionMedicine
- ✅ **ISearchable**: Implemented by Medication, Patient, Prescription
- ✅ **IRepository<T>**: Generic repository interface

### 4. **Constructors**
#### Regular Constructors:
- All entity classes have parameterized constructors
- Base classes (Entity, User, Person, Item) have protected constructors

#### Example in Medication:
```csharp
// Default constructor for EF Core
protected Medication() { }

// Basic constructor
public Medication(string name, string genericName, decimal price)

// Full constructor
public Medication(string name, string genericName, decimal price, 
                  MedicationCategory category, string manufacturer, 
                  DateTime expiryDate)
```

### 5. **Constructor Overloading**
#### Medication Class:
```csharp
public Medication(string name, decimal price)
public Medication(string name, string genericName, decimal price)
public Medication(string name, string genericName, decimal price, 
                  MedicationCategory category)
public Medication(string name, string genericName, decimal price, 
                  MedicationCategory category, string manufacturer, 
                  DateTime expiryDate, int supplierId)
```

#### Inventory Class:
```csharp
public Inventory(int medicationId, int quantity)
public Inventory(int medicationId, int quantity, int reorderLevel)
public Inventory(int medicationId, int quantity, int reorderLevel, 
                 int reorderQuantity, string location)
```

### 6. **Copy Constructors**
#### Medication:
```csharp
// Copy constructor
public Medication(Medication other)
{
    Id = 0; // New entity
    Name = other.Name;
    GenericName = other.GenericName;
    Price = other.Price;
    Category = other.Category;
    Manufacturer = other.Manufacturer;
    // ... copy other properties
}
```

#### Prescription:
```csharp
// Copy constructor
public Prescription(Prescription other)
{
    PatientId = other.PatientId;
    PharmacistId = other.PharmacistId;
    Status = PrescriptionStatus.Pending; // Reset status
    // Deep copy medication list
    Medications = new List<PrescriptionItem>(
        other.Medications.Select(m => new PrescriptionItem(m))
    );
}
```

### 7. **Operator Overloading**

#### Inventory Class:
```csharp
// Add stock
public static Inventory operator +(Inventory inv, int quantity)
{
    inv.AddStock(quantity);
    return inv;
}

// Remove stock
public static Inventory operator -(Inventory inv, int quantity)
{
    inv.RemoveStock(quantity);
    return inv;
}

// Comparison (check if needs reorder)
public static bool operator <(Inventory inv, int threshold)
{
    return inv.CurrentQuantity < threshold;
}

public static bool operator >(Inventory inv, int threshold)
{
    return inv.CurrentQuantity > threshold;
}
```

#### Money Class:
```csharp
public static Money operator +(Money m1, Money m2)
public static Money operator -(Money m1, Money m2)
public static Money operator *(Money m, decimal multiplier)
public static bool operator >(Money m1, Money m2)
public static bool operator <(Money m1, Money m2)
public static bool operator ==(Money m1, Money m2)
public static bool operator !=(Money m1, Money m2)
```

### 8. **Function Overloading**

#### MedicationService:
```csharp
// Search overloads
public List<Medication> Search(string name)
public List<Medication> Search(MedicationCategory category)
public List<Medication> Search(string name, MedicationCategory category)
public List<Medication> Search(decimal minPrice, decimal maxPrice)
```

#### InventoryService:
```csharp
// Add stock overloads
public void AddStock(int medicationId, int quantity)
public void AddStock(int medicationId, int quantity, string notes)
public void AddStock(Medication medication, int quantity)
public void AddStock(Medication medication, int quantity, int supplierId, string notes)
```

### 9. **Static Methods & Static Fields**

#### SystemConfig Class:
```csharp
public static class SystemConfig
{
    // Static fields
    public static readonly decimal TaxRate = 0.15m;
    public static readonly int DefaultReorderLevel = 50;
    public static readonly int DefaultReorderQuantity = 100;
    public static readonly int LowStockThreshold = 20;
    public static readonly decimal DiscountThreshold = 1000m;
    public static readonly decimal BulkDiscountRate = 0.10m;
    
    // Static methods
    public static decimal CalculateTax(decimal amount)
    public static decimal ApplyDiscount(decimal amount, decimal discountRate)
    public static bool IsLowStock(int quantity)
    public static bool RequiresReorder(int current, int reorderLevel)
    public static string GeneratePrescriptionNumber()
    public static string GenerateTransactionNumber()
}
```

#### ConsoleHelper Class:
```csharp
public static class ConsoleHelper
{
    public static void PrintHeader(string title)
    public static void PrintSuccess(string message)
    public static void PrintError(string message)
    public static void PrintWarning(string message)
    public static int GetIntInput(string prompt)
    public static decimal GetDecimalInput(string prompt)
    public static string GetStringInput(string prompt)
}
```

---

## 📊 Class Diagrams & Relationships

### User Hierarchy
```
┌─────────────────┐
│     Entity      │ (Base class with Id, CreatedAt, UpdatedAt)
└────────┬────────┘
         │
    ┌────▼────┐
    │  User   │ (abstract: Username, Email, Role)
    └────┬────┘
         │
    ┌────▼────┐
    │ Person  │ (abstract: FirstName, LastName, DateOfBirth, Phone)
    └────┬────┘
         │
         ├──────────┐
         │          │
    ┌────▼────┐ ┌──▼────────┐
    │ Patient │ │ Employee  │ (abstract: EmployeeId, HireDate, Salary)
    └─────────┘ └──────┬────┘
                       │
                  ┌────▼────────┐
                  │ Pharmacist  │ (LicenseNumber, Specialization)
                  └─────────────┘
```

### Medication Hierarchy
```
┌─────────────────┐
│     Entity      │
└────────┬────────┘
         │
    ┌────▼────┐
    │  Item   │ (abstract: Name, Description, Price)
    └────┬────┘
         │
    ┌────▼───────┐
    │ Medication │ (abstract: GenericName, Manufacturer, ExpiryDate, Category)
    └────┬───────┘
         │
         ├────────────────────┐
         │                    │
    ┌────▼──────────────┐ ┌──▼─────────┐
    │ PrescriptionMedicine│ │ OTCMedicine │
    │ (DosageForm, Strength)│ (UsageInstructions)
    │ implements IPrescribable│ │
    └─────────────────────┘ └────────────┘
    
    Both implement: IInventoryItem, ISearchable
```

### Entity Relationships
```
┌──────────┐         ┌──────────────┐         ┌────────────┐
│ Patient  │────────▶│ Prescription │◀────────│ Pharmacist │
└──────────┘ places  └──────┬───────┘ creates └────────────┘
                            │ contains
                            │
                     ┌──────▼──────────┐
                     │ PrescriptionItem│
                     │ (M:N relation)  │
                     └──────┬──────────┘
                            │
                     ┌──────▼──────┐
                     │ Medication  │
                     └──────┬──────┘
                            │
                     ┌──────▼──────┐
                     │  Inventory  │ (1:1 with Medication)
                     └──────┬──────┘
                            │ supplied by
                     ┌──────▼──────┐
                     │  Supplier   │
                     └─────────────┘

┌──────────────┐
│ Transaction  │ (Records sales)
└──────┬───────┘
       │ contains
┌──────▼────────┐
│TransactionItem│
│ (M:N relation)│
└──────┬────────┘
       │
┌──────▼──────┐
│ Medication  │
└─────────────┘
```

---

## 🔌 Interface Definitions

### IInventoryItem
```csharp
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int CurrentStock { get; }
    int ReorderLevel { get; }
    bool IsLowStock();
    bool RequiresReorder();
    void UpdateStock(int quantity);
}
```

### IPrescribable
```csharp
public interface IPrescribable
{
    string Name { get; }
    string GenericName { get; }
    string DosageForm { get; }
    string Strength { get; }
    bool RequiresPrescription { get; }
    bool CanDispense(int quantity);
    string GetPrescriptionWarnings();
}
```

### ISearchable
```csharp
public interface ISearchable
{
    int Id { get; }
    string GetSearchableText(); // Returns concatenated searchable fields
    bool MatchesSearch(string searchTerm);
}
```

### IRepository<T>
```csharp
public interface IRepository<T> where T : Entity
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
```

---

## 🗄️ Database Schema

### Tables Overview
```
Users (Base table)
├── Patients (Inherits from Users via TPH - Table Per Hierarchy)
├── Employees (Inherits from Users via TPH)
└── Pharmacists (Inherits from Employees via TPH)

Items (Base table)
└── Medications (Inherits from Items via TPH)
    ├── PrescriptionMedicines (Discriminator column)
    └── OTCMedicines (Discriminator column)

Inventory (1:1 with Medications)
Suppliers
Prescriptions
PrescriptionItems (Junction table)
Transactions
TransactionItems (Junction table)
```

### Key Tables Schema

**Users** (TPH - includes Patients, Employees, Pharmacists)
```sql
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Discriminator VARCHAR(50) NOT NULL, -- 'Patient', 'Employee', 'Pharmacist'
    Username VARCHAR(100) UNIQUE NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(500) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    PhoneNumber VARCHAR(20),
    Address TEXT,
    -- Patient specific
    MedicalHistory TEXT,
    Allergies TEXT,
    InsuranceNumber VARCHAR(100),
    -- Employee specific
    EmployeeId VARCHAR(50),
    HireDate DATE,
    Salary DECIMAL(18,2),
    -- Pharmacist specific
    LicenseNumber VARCHAR(100),
    Specialization VARCHAR(200),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Medications** (TPH - includes PrescriptionMedicine, OTCMedicine)
```sql
CREATE TABLE Medications (
    Id SERIAL PRIMARY KEY,
    Discriminator VARCHAR(50) NOT NULL, -- 'PrescriptionMedicine', 'OTCMedicine'
    Name VARCHAR(200) NOT NULL,
    GenericName VARCHAR(200),
    Description TEXT,
    Price DECIMAL(18,2) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Manufacturer VARCHAR(200),
    ExpiryDate DATE NOT NULL,
    -- PrescriptionMedicine specific
    DosageForm VARCHAR(100),
    Strength VARCHAR(50),
    RequiresPrescription BOOLEAN DEFAULT false,
    ControlledSubstance BOOLEAN DEFAULT false,
    -- OTCMedicine specific
    UsageInstructions TEXT,
    ActiveIngredients TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Inventory**
```sql
CREATE TABLE Inventory (
    Id SERIAL PRIMARY KEY,
    MedicationId INT UNIQUE NOT NULL REFERENCES Medications(Id),
    CurrentQuantity INT NOT NULL DEFAULT 0,
    ReorderLevel INT NOT NULL DEFAULT 50,
    ReorderQuantity INT NOT NULL DEFAULT 100,
    Location VARCHAR(100),
    LastRestockDate TIMESTAMP,
    SupplierId INT REFERENCES Suppliers(Id),
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Suppliers**
```sql
CREATE TABLE Suppliers (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    ContactPerson VARCHAR(100),
    Email VARCHAR(255),
    PhoneNumber VARCHAR(20),
    Address TEXT,
    Website VARCHAR(255),
    Rating DECIMAL(3,2),
    IsActive BOOLEAN DEFAULT true,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Prescriptions**
```sql
CREATE TABLE Prescriptions (
    Id SERIAL PRIMARY KEY,
    PrescriptionNumber VARCHAR(50) UNIQUE NOT NULL,
    PatientId INT NOT NULL REFERENCES Users(Id),
    PharmacistId INT NOT NULL REFERENCES Users(Id),
    PrescriptionDate TIMESTAMP NOT NULL,
    Status VARCHAR(50) NOT NULL, -- 'Pending', 'Dispensed', 'Cancelled'
    DoctorName VARCHAR(200),
    DoctorLicenseNumber VARCHAR(100),
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**PrescriptionItems** (Junction table)
```sql
CREATE TABLE PrescriptionItems (
    Id SERIAL PRIMARY KEY,
    PrescriptionId INT NOT NULL REFERENCES Prescriptions(Id),
    MedicationId INT NOT NULL REFERENCES Medications(Id),
    Quantity INT NOT NULL,
    Dosage VARCHAR(100),
    Frequency VARCHAR(100),
    Duration VARCHAR(100),
    Instructions TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Transactions**
```sql
CREATE TABLE Transactions (
    Id SERIAL PRIMARY KEY,
    TransactionNumber VARCHAR(50) UNIQUE NOT NULL,
    PatientId INT REFERENCES Users(Id),
    PharmacistId INT NOT NULL REFERENCES Users(Id),
    TransactionDate TIMESTAMP NOT NULL,
    SubTotal DECIMAL(18,2) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) DEFAULT 0,
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    Status VARCHAR(50) NOT NULL, -- 'Completed', 'Refunded', 'Cancelled'
    PrescriptionId INT REFERENCES Prescriptions(Id),
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**TransactionItems** (Junction table)
```sql
CREATE TABLE TransactionItems (
    Id SERIAL PRIMARY KEY,
    TransactionId INT NOT NULL REFERENCES Transactions(Id),
    MedicationId INT NOT NULL REFERENCES Medications(Id),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

---

## 🛠️ Implementation Guidelines

### Phase 1: Setup (Day 1)

1. **Create Solution Structure**
```bash
dotnet new sln -n PharmacySystem
dotnet new classlib -n PharmacySystem.Core -o src/PharmacySystem.Core
dotnet new classlib -n PharmacySystem.Data -o src/PharmacySystem.Data
dotnet new console -n PharmacySystem.Console -o src/PharmacySystem.Console

dotnet sln add src/PharmacySystem.Core/PharmacySystem.Core.csproj
dotnet sln add src/PharmacySystem.Data/PharmacySystem.Data.csproj
dotnet sln add src/PharmacySystem.Console/PharmacySystem.Console.csproj
```

2. **Add Project References**
```bash
cd src/PharmacySystem.Data
dotnet add reference ../PharmacySystem.Core/PharmacySystem.Core.csproj

cd ../PharmacySystem.Console
dotnet add reference ../PharmacySystem.Core/PharmacySystem.Core.csproj
dotnet add reference ../PharmacySystem.Data/PharmacySystem.Data.csproj
```

3. **Install NuGet Packages**
```bash
# In PharmacySystem.Data
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design

# In PharmacySystem.Console
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.DependencyInjection
```

### Phase 2: Core Domain (Day 2-3)

**Order of Implementation:**

1. **Base Classes First**
   - Entity.cs (Id, CreatedAt, UpdatedAt)
   - User.cs (abstract)
   - Item.cs (abstract)

2. **Enums**
   - MedicationCategory
   - PrescriptionStatus
   - TransactionStatus
   - UserRole

3. **Interfaces**
   - IInventoryItem
   - IPrescribable
   - ISearchable
   - IRepository<T>

4. **Value Objects**
   - Money (with operator overloading)
   - Address

5. **User Hierarchy**
   - Person → Patient
   - Person → Employee → Pharmacist
   - *Implement constructor overloading in Person and Pharmacist*

6. **Medication Hierarchy**
   - Medication (abstract)
   - PrescriptionMedicine (implements IPrescribable)
   - OTCMedicine
   - *Implement constructor overloading and copy constructor in Medication*

7. **Operation Classes**
   - Supplier
   - Inventory (implement operator overloading +, -, <, >)
   - Prescription (implement copy constructor)
   - Transaction

8. **Static Configuration**
   - SystemConfig class with static fields and methods

### Phase 3: Data Layer (Day 4-5)

1. **DbContext Setup**
```csharp
public class PharmacyDbContext : DbContext
{
    // DbSets for all entities
    // OnModelCreating for configurations (TPH, relationships)
}
```

2. **Entity Configurations**
   - Configure TPH for Users and Medications
   - Configure relationships (1:1, 1:M, M:N)
   - Configure value conversions for Money

3. **Repository Pattern**
   - GenericRepository<T> implementing IRepository<T>
   - Specific repositories with additional methods

4. **Migrations**
```bash
dotnet ef migrations add InitialCreate --project src/PharmacySystem.Data --startup-project src/PharmacySystem.Console
dotnet ef database update --project src/PharmacySystem.Data --startup-project src/PharmacySystem.Console
```

### Phase 4: Console Application (Day 6-7)

1. **Service Layer**
   - MedicationService (implement function overloading for Search methods)
   - InventoryService (implement function overloading for AddStock methods)
   - PrescriptionService
   - TransactionService

2. **Console Menu Structure**
```
Main Menu:
1. Medication Management
   - Add Medication
   - Search Medication (demonstrate function overloading)
   - Update Medication
   - Delete Medication
   - List All Medications
   
2. Inventory Management
   - View Current Stock
   - Add Stock (demonstrate operator overloading: inventory + quantity)
   - Remove Stock (demonstrate operator overloading: inventory - quantity)
   - Check Low Stock Items
   - Generate Reorder Report
   
3. Prescription Management
   - Create Prescription
   - Dispense Prescription
   - Search Prescriptions
   - Copy Prescription (demonstrate copy constructor)
   
4. Transaction Management
   - Create Transaction
   - View Transaction History
   - Process Refund
   
5. Reports
   - Daily Sales Report
   - Low Stock Report
   - Popular Medications
   - Expiring Medications
```

3. **Static Utilities**
   - ConsoleHelper (static methods for UI)
   - DataSeeder (static method to seed initial data)

### Phase 5: Docker Setup (Day 7)

1. **docker-compose.yml**
2. **Environment variables**
3. **Database initialization script**
4. **Connection string configuration**

---

## 🚀 Expansion Roadmap

### Phase 2: Enhanced Pharmacy Operations (Future)
- **Patient Portal**: Online prescription requests, refill reminders
- **Inventory Alerts**: Automated low-stock notifications, expiry alerts
- **Supplier Integration**: Purchase orders, automated reordering
- **Reporting Engine**: Advanced analytics, sales forecasting
- **Multi-location Support**: Chain pharmacy management

### Phase 3: Rule Engine & Validation (Future)
- **Drug Interaction Checker**: Check for contraindications
- **Dosage Validator**: Age/weight-based dosage validation
- **Allergy Checker**: Cross-reference patient allergies
- **Prescription Validator**: Verify prescription authenticity
- **Insurance Verification**: Real-time insurance checks

### Phase 4: Healthcare Integration (Future)
- **Lab Management Module**:
  - Test ordering
  - Result tracking
  - Report generation
  
- **Appointment Scheduling Module**:
  - Doctor appointments
  - Pharmacy consultations
  - Calendar integration

- **Electronic Health Records (EHR)**:
  - Complete patient history
  - Medical records integration
  - HIPAA compliance features

- **Telemedicine Module**:
  - Virtual consultations
  - Online prescription generation
  - Remote monitoring

### Phase 5: Advanced Features (Future)
- **Mobile App**: Patient and pharmacist mobile applications
- **AI/ML Integration**: 
  - Demand forecasting
  - Personalized medicine recommendations
  - Fraud detection
- **Blockchain**: Prescription verification, supply chain tracking
- **IoT Integration**: Smart inventory management, refrigeration monitoring

---

## 📝 Key Implementation Tips

### OOP Best Practices

1. **Encapsulation**
   - Keep fields private
   - Use properties with appropriate access modifiers
   - Validate in setters

2. **Abstraction**
   - Use abstract classes for base functionality
   - Use interfaces for contracts
   - Hide implementation details

3. **Inheritance**
   - Keep hierarchies shallow (2-3 levels max)
   - Use "is-a" relationship test
   - Don't overuse inheritance where composition is better

4. **Polymorphism**
   - Override virtual methods appropriately
   - Use interface polymorphism for flexibility
   - Leverage operator overloading naturally

### Repository Pattern Tips

1. **Generic Repository**
   - Common CRUD operations
   - Async/await for database operations
   - Include error handling

2. **Specific Repositories**
   - Add domain-specific queries
   - Complex search operations
   - Business logic queries

3. **Unit of Work Pattern** (Optional for Phase 1)
   - Can be added later for transaction management
   - Groups repository operations

### EF Core Tips

1. **TPH (Table Per Hierarchy)**
   - Best for User and Medication hierarchies
   - Single table with discriminator column
   - Better performance, some null columns

2. **Relationships**
   - 1:1 → Inventory-Medication
   - 1:M → Supplier-Inventory, Patient-Prescriptions
   - M:N → Prescription-Medications, Transaction-Medications

3. **Configuration**
   - Use Fluent API over Data Annotations for complex scenarios
   - Configure indexes for frequently searched fields
   - Set up cascade delete rules carefully

---

## 🎯 Demonstrating OOP Concepts - Quick Reference

When your instructor reviews your code, they'll look for:

✅ **Classes**: 10+ classes with clear responsibilities
✅ **Inheritance**: Multi-level hierarchies (User→Person→Patient→Pharmacist)
✅ **Interfaces**: 4 interfaces implemented across multiple classes
✅ **Constructors**: Multiple constructors in each entity
✅ **Constructor Overloading**: 3-4 overloads in Medication, Inventory
✅ **Copy Constructors**: In Medication and Prescription classes
✅ **Operator Overloading**: Inventory (+, -, <, >) and Money (+, -, *, ==, !=)
✅ **Function Overloading**: Search and AddStock methods with different parameters
✅ **Static Members**: SystemConfig class with static fields and utility methods

---

## 📚 Additional Resources

### Learning Materials
- C# OOP Official Docs: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/
- EF Core Documentation: https://learn.microsoft.com/en-us/ef/core/
- Repository Pattern: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design

### Tools
- Visual Studio Code + C# Extension
- .NET 8 SDK
- PostgreSQL + pgAdmin (optional GUI)
- Docker Desktop
- Git for version control

---

## 🎓 Assignment Submission Checklist

Before submitting your assignment, verify:

- [ ] All 9 OOP concepts are implemented
- [ ] Code is well-commented with XML documentation
- [ ] Solution builds without errors
- [ ] Database migrations are included
- [ ] Docker compose successfully starts PostgreSQL
- [ ] Console application demonstrates all features
- [ ] README.md has setup instructions
- [ ] Code follows C# naming conventions
- [ ] Each class has a clear single responsibility
- [ ] Git repository is clean (no bin/obj folders)

---

**Good luck with your implementation! 🚀**

*Remember: This is Phase 1 - keep it expandable but don't over-engineer. Focus on demonstrating solid OOP principles first.*