# 🏥 Pharmacy & Medication Management System

A comprehensive, modular pharmacy management system built with C# and .NET 8, demonstrating advanced Object-Oriented Programming principles. This system provides medication inventory tracking, prescription management, and transaction processing with PostgreSQL database backend.

## 📋 Table of Contents
- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [OOP Concepts Demonstrated](#-oop-concepts-demonstrated)
- [Prerequisites](#-prerequisites)
- [Installation & Setup](#-installation--setup)
- [Project Structure](#-project-structure)
- [Database Setup](#-database-setup)
- [Running the Application](#-running-the-application)
- [Usage Examples](#-usage-examples)
- [Future Enhancements](#-future-enhancements)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

### Phase 1 (Current Implementation)
- **Medication Management**: Add, update, search, and delete medications (Prescription & OTC)
- **Inventory Tracking**: Real-time stock monitoring with low-stock alerts
- **Prescription Handling**: Create and dispense prescriptions with pharmacist verification
- **Transaction Processing**: Complete billing system with tax calculations and discounts
- **Supplier Management**: Track suppliers and manage restocking
- **Search Functionality**: Multi-criteria search across medications, patients, and prescriptions
- **Reporting**: Daily sales, low stock, and popular medication reports

### Key Capabilities
- ✅ Multi-level inheritance hierarchies
- ✅ Interface-based polymorphism
- ✅ Operator overloading for natural syntax
- ✅ Repository pattern for data access
- ✅ Docker containerization for easy deployment
- ✅ PostgreSQL database with Entity Framework Core

---

## 🛠️ Technology Stack

- **Language**: C# 12 (.NET 8.0)
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core 8
- **Architecture**: Repository Pattern, Domain-Driven Design
- **Containerization**: Docker & Docker Compose
- **Development**: Visual Studio Code / Visual Studio 2022

---

## 🎓 OOP Concepts Demonstrated

This project comprehensively demonstrates all required Object-Oriented Programming concepts:

### 1. Classes (10+ classes)
- Base classes: `Entity`, `User`, `Person`, `Item`
- Domain entities: `Medication`, `Patient`, `Pharmacist`, `Prescription`, `Inventory`, `Transaction`, `Supplier`
- Value objects: `Money`, `Address`

### 2. Inheritance (Multi-level hierarchies)
```
User → Person → Patient
User → Person → Employee → Pharmacist
Item → Medication → PrescriptionMedicine
Item → Medication → OTCMedicine
```

### 3. Interfaces (4+ interfaces)
- `IInventoryItem` - Inventory tracking behavior
- `IPrescribable` - Prescribable medication behavior
- `ISearchable` - Search functionality
- `IRepository<T>` - Data access abstraction

### 4. Constructors
- Every class has parameterized constructors
- Protected constructors for abstract base classes
- Default constructors for Entity Framework Core

### 5. Constructor Overloading
```csharp
// Example: Medication class
Medication(string name, decimal price)
Medication(string name, string genericName, decimal price)
Medication(string name, string genericName, decimal price, MedicationCategory category)
Medication(string name, string genericName, decimal price, MedicationCategory category, 
           string manufacturer, DateTime expiryDate, int supplierId)
```

### 6. Copy Constructors
```csharp
// Example: Creates a deep copy of a prescription
public Prescription(Prescription other) { ... }
public Medication(Medication other) { ... }
```

### 7. Operator Overloading
```csharp
// Inventory stock operations
inventory + 50;  // Add 50 units
inventory - 20;  // Remove 20 units
inventory < 100; // Check if below threshold

// Money calculations
money1 + money2;
money1 * 1.15m;  // Apply tax
money1 > money2;
```

### 8. Function Overloading
```csharp
// Search with different parameters
Search(string name)
Search(MedicationCategory category)
Search(string name, MedicationCategory category)
Search(decimal minPrice, decimal maxPrice)

// Add stock with various options
AddStock(int medicationId, int quantity)
AddStock(int medicationId, int quantity, string notes)
AddStock(Medication medication, int quantity, int supplierId)
```

### 9. Static Methods & Fields
```csharp
public static class SystemConfig
{
    public static readonly decimal TaxRate = 0.15m;
    public static readonly int DefaultReorderLevel = 50;
    
    public static decimal CalculateTax(decimal amount) { ... }
    public static bool IsLowStock(int quantity) { ... }
    public static string GeneratePrescriptionNumber() { ... }
}
```

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8 SDK** or later
  - Download: https://dotnet.microsoft.com/download
  - Verify: `dotnet --version`

- **Docker Desktop** (for containerized database)
  - Download: https://www.docker.com/products/docker-desktop
  - Verify: `docker --version` and `docker-compose --version`

- **Visual Studio Code** (recommended) or Visual Studio 2022
  - VS Code: https://code.visualstudio.com/
  - Required extensions for VS Code:
    - C# Dev Kit
    - .NET Install Tool

- **Git** (for version control)
  - Download: https://git-scm.com/downloads
  - Verify: `git --version`

---

## 🚀 Installation & Setup

### Step 1: Clone the Repository
```bash
git clone https://github.com/yourusername/pharmacy-system.git
cd pharmacy-system
```

### Step 2: Setup Environment Variables
```bash
# Copy the example environment file
cp .env.example .env

# Edit .env with your preferred settings (optional)
# Default values work out of the box
```

### Step 3: Start PostgreSQL Database
```bash
# Start the PostgreSQL container
docker-compose up -d

# Verify the container is running
docker-compose ps

# View logs (optional)
docker-compose logs -f postgres
```

The database will be available at:
- **Host**: localhost
- **Port**: 5432
- **Database**: pharmacy_db
- **Username**: pharmacy_user
- **Password**: pharmacy_pass_2024

**Optional**: Access pgAdmin (Database GUI) at http://localhost:5050
- **Email**: admin@pharmacy.com
- **Password**: admin123

### Step 4: Restore NuGet Packages
```bash
# Navigate to the solution directory
dotnet restore
```

### Step 5: Build the Solution
```bash
# Build all projects
dotnet build

# Or build specific project
dotnet build src/PharmacySystem.Console/PharmacySystem.Console.csproj
```

### Step 6: Setup Database Schema
```bash
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console

# Apply migrations to database
dotnet ef database update \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console
```

---

## 📁 Project Structure

```
PharmacySystem/
├── src/
│   ├── PharmacySystem.Core/          # Domain models, interfaces, business logic
│   │   ├── Entities/
│   │   │   ├── Base/                 # Abstract base classes
│   │   │   ├── People/               # User hierarchy (Patient, Pharmacist, etc.)
│   │   │   ├── Items/                # Medication hierarchy
│   │   │   ├── Operations/           # Prescription, Transaction, Inventory
│   │   │   └── ValueObjects/         # Money, Address
│   │   ├── Interfaces/               # IInventoryItem, IPrescribable, etc.
│   │   ├── Enums/                    # MedicationCategory, Status enums
│   │   └── Configuration/            # Static configuration
│   │
│   ├── PharmacySystem.Data/          # Data access layer
│   │   ├── Context/                  # EF Core DbContext
│   │   ├── Repositories/             # Repository implementations
│   │   ├── Configurations/           # Entity configurations
│   │   └── Migrations/               # Database migrations
│   │
│   └── PharmacySystem.Console/       # Console application
│       ├── Program.cs                # Entry point
│       ├── Services/                 # Business services
│       └── Utilities/                # Helper utilities
│
├── tests/                            # Unit tests (optional)
├── docker/
│   ├── docker-compose.yml            # Docker configuration
│   ├── .env.example                  # Environment variables template
│   └── init.sql                      # Database initialization
├── .gitignore
├── .dockerignore
├── PharmacySystem.sln                # Solution file
└── README.md
```

---

## 🗄️ Database Setup

### Connection String Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=pharmacy_db;Username=pharmacy_user;Password=pharmacy_pass_2024;"
  }
}
```

### Database Schema

The system uses **Table Per Hierarchy (TPH)** inheritance:

**Main Tables:**
- `Users` - Stores Patients, Employees, and Pharmacists
- `Medications` - Stores PrescriptionMedicine and OTCMedicine
- `Inventory` - One-to-one with Medications
- `Prescriptions` - Prescription headers
- `PrescriptionItems` - Junction table for prescriptions and medications
- `Transactions` - Sale transactions
- `TransactionItems` - Junction table for transactions and medications
- `Suppliers` - Supplier information

### Managing Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console

# Update database
dotnet ef database update \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console

# Rollback to specific migration
dotnet ef database update PreviousMigrationName \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console

# Remove last migration (if not applied)
dotnet ef migrations remove \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console
```

---

## ▶️ Running the Application

### Using .NET CLI
```bash
# Run from solution root
dotnet run --project src/PharmacySystem.Console

# Or navigate to the console project
cd src/PharmacySystem.Console
dotnet run
```

### Using Visual Studio Code
1. Open the project folder in VS Code
2. Press `F5` or select "Run > Start Debugging"
3. Or use `Ctrl+F5` for running without debugging

### Using Visual Studio 2022
1. Open `PharmacySystem.sln`
2. Set `PharmacySystem.Console` as startup project
3. Press `F5` to run

---

## 💡 Usage Examples

### Main Menu Navigation
When you run the application, you'll see:

```
╔════════════════════════════════════════╗
║  PHARMACY MANAGEMENT SYSTEM v1.0       ║
╚════════════════════════════════════════╝

Main Menu:
1. Medication Management
2. Inventory Management
3. Prescription Management
4. Transaction Management
5. Reports
6. Settings
0. Exit

Select an option:
```

### Example: Adding a Medication
```
Select option: 1 (Medication Management)
→ Add New Medication

Enter medication details:
- Name: Amoxicillin
- Generic Name: Amoxicillin Trihydrate
- Price: 15.99
- Category: Antibiotic
- Manufacturer: PharmaCorp
- Expiry Date: 2026-12-31
- Type: Prescription Medicine

✓ Medication added successfully! (ID: 1)
```

### Example: Using Operator Overloading
```csharp
// In code: Add stock using + operator
inventory = inventory + 100;  // Adds 100 units

// In code: Remove stock using - operator
inventory = inventory - 50;   // Removes 50 units

// In code: Check if below threshold
if (inventory < 20)
{
    Console.WriteLine("⚠ Low stock alert!");
}
```

### Example: Searching Medications (Function Overloading)
```
Search Options:
1. By Name
2. By Category
3. By Name and Category
4. By Price Range

Select: 3
Enter name: Amox
Select category: Antibiotic

Results found: 2 medications
```

### Example: Creating a Prescription
```
Create New Prescription:
- Patient ID: 101
- Pharmacist ID: 5
- Doctor Name: Dr. Smith
- Doctor License: MD12345

Add Medications:
1. Medication ID: 1 (Amoxicillin)
   Quantity: 30
   Dosage: 500mg
   Frequency: 3 times daily
   Duration: 10 days

✓ Prescription created: RX-2024-001
```

---

## 🔮 Future Enhancements

### Phase 2: Enhanced Operations
- [ ] Patient portal for prescription refills
- [ ] Automated inventory alerts
- [ ] Supplier integration with purchase orders
- [ ] Advanced reporting and analytics
- [ ] Multi-location support

### Phase 3: Rule Engine
- [ ] Drug interaction checker
- [ ] Dosage validation engine
- [ ] Patient allergy cross-reference
- [ ] Prescription authenticity verification
- [ ] Insurance verification system

### Phase 4: Healthcare Integration
- [ ] Lab management module
- [ ] Appointment scheduling system
- [ ] Electronic Health Records (EHR)
- [ ] Telemedicine integration

### Phase 5: Advanced Features
- [ ] Mobile applications (iOS/Android)
- [ ] AI-powered demand forecasting
- [ ] Blockchain for prescription verification
- [ ] IoT integration for smart inventory

---

## 🧪 Testing

### Running Unit Tests (Optional)
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity detailed

# Run specific test project
dotnet test tests/PharmacySystem.Tests/PharmacySystem.Tests.csproj
```

---

## 🛑 Troubleshooting

### Database Connection Issues
```bash
# Check if PostgreSQL container is running
docker-compose ps

# Restart PostgreSQL
docker-compose restart postgres

# View PostgreSQL logs
docker-compose logs postgres

# Check connection from host machine
psql -h localhost -p 5432 -U pharmacy_user -d pharmacy_db
```

### Migration Issues
```bash
# Drop and recreate database (⚠ CAUTION: Data loss!)
dotnet ef database drop --force \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console

# Recreate database
dotnet ef database update \
  --project src/PharmacySystem.Data \
  --startup-project src/PharmacySystem.Console
```

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build

# Restore packages
dotnet restore --force
```

---

## 📚 Documentation

### Additional Resources
- [C# OOP Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Docker Documentation](https://docs.docker.com/)

### Code Comments
All classes and methods are documented with XML comments:
```csharp
/// <summary>
/// Represents a medication in the pharmacy inventory
/// </summary>
public class Medication : Item, IInventoryItem, ISearchable
{
    /// <summary>
    /// Adds stock to the inventory using operator overloading
    /// </summary>
    public static Inventory operator +(Inventory inv, int quantity)
    {
        // Implementation
    }
}
```

---

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Style
- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML comments for public members
- Keep methods focused and concise
- Write unit tests for new features

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **Your Name** - *Initial work* - [YourGitHub](https://github.com/yourusername)

---

## 🙏 Acknowledgments

- Inspired by real-world pharmacy management systems
- Built as an academic project demonstrating OOP principles
- Thanks to the .NET and Entity Framework Core communities

---

## 📞 Support

For questions or issues:
- 📧 Email: your.email@example.com
- 🐛 Issues: [GitHub Issues](https://github.com/yourusername/pharmacy-system/issues)
- 💬 Discussions: [GitHub Discussions](https://github.com/yourusername/pharmacy-system/discussions)

---

## 🎯 Assignment Submission Notes

### OOP Concepts Coverage
This project demonstrates **all required OOP concepts**:
- ✅ Classes (10+ implemented)
- ✅ Inheritance (multi-level hierarchies)
- ✅ Interfaces (4+ implemented)
- ✅ Constructors (in all classes)
- ✅ Constructor Overloading (Medication, Inventory, Prescription)
- ✅ Copy Constructors (Medication, Prescription)
- ✅ Operator Overloading (Inventory, Money)
- ✅ Function Overloading (Search, AddStock methods)
- ✅ Static Methods & Fields (SystemConfig, ConsoleHelper)

### Key Files to Review
1. `src/PharmacySystem.Core/Entities/` - All domain models
2. `src/PharmacySystem.Core/Interfaces/` - Interface definitions
3. `src/PharmacySystem.Data/Repositories/` - Repository pattern
4. `src/PharmacySystem.Console/Program.cs` - Main application flow

### Running the Demo
```bash
docker-compose up -d
dotnet ef database update --project src/PharmacySystem.Data --startup-project src/PharmacySystem.Console
dotnet run --project src/PharmacySystem.Console
```

---

**Built with ❤️ using C# and .NET**

*Last Updated: December 2024*