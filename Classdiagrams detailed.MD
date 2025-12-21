# Class Diagrams and Relationships

## Visual Class Hierarchy Diagrams

### 1. User Hierarchy (Complete Structure)

```
┌─────────────────────────────────────────────────────────────┐
│                         Entity                              │
│  ─────────────────────────────────────────────────────────  │
│  # int Id                                                   │
│  # DateTime CreatedAt                                       │
│  # DateTime UpdatedAt                                       │
│  ─────────────────────────────────────────────────────────  │
│  + GetId() : int                                           │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           │ inherits
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                User (abstract)                              │
│  ─────────────────────────────────────────────────────────  │
│  + string Username                                          │
│  + string Email                                             │
│  + string PasswordHash                                      │
│  + UserRole Role                                            │
│  + bool IsActive                                            │
│  ─────────────────────────────────────────────────────────  │
│  # User()                                  [Constructor]    │
│  # User(username, email, role)            [Constructor]    │
│  + ValidateCredentials(password) : bool                     │
│  + UpdatePassword(oldPass, newPass) : void                  │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           │ inherits
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                Person (abstract)                            │
│  ─────────────────────────────────────────────────────────  │
│  + string FirstName                                         │
│  + string LastName                                          │
│  + DateTime DateOfBirth                                     │
│  + string PhoneNumber                                       │
│  + Address Address                                          │
│  ─────────────────────────────────────────────────────────  │
│  # Person()                               [Constructor]     │
│  # Person(firstName, lastName, dob)       [Constructor]     │
│  # Person(firstName, lastName, dob,       [Overloaded       │
│           phone, address)                  Constructor]     │
│  + GetFullName() : string                                   │
│  + GetAge() : int                                           │
│  + UpdateContactInfo(phone, address) : void                 │
└──────────────────────────┬──────────────────────────────────┘
                           │
            ┌──────────────┴──────────────┐
            │                             │
            ▼                             ▼
┌──────────────────────┐    ┌──────────────────────────────────┐
│      Patient         │    │      Employee (abstract)         │
│  ──────────────────  │    │  ──────────────────────────────  │
│  + string           │    │  + string EmployeeId             │
│    MedicalHistory   │    │  + DateTime HireDate             │
│  + string Allergies │    │  + decimal Salary                │
│  + string           │    │  + string Department             │
│    InsuranceNumber  │    │  ──────────────────────────────  │
│  + List<           │    │  # Employee()      [Constructor] │
│    Prescription>   │    │  # Employee(empId,  [Constructor] │
│    Prescriptions   │    │            hireDate, salary)      │
│  + List<           │    │  + CalculateYearsOfService() : int│
│    Transaction>    │    │  + GetEmployeeInfo() : string    │
│    Transactions    │    └───────────────┬──────────────────┘
│  ──────────────────  │                  │
│  + Patient()        │                  │ inherits
│    [Constructor]    │                  ▼
│  + Patient(         │    ┌──────────────────────────────────┐
│    firstName,       │    │         Pharmacist               │
│    lastName, dob)   │    │  ──────────────────────────────  │
│    [Constructor]    │    │  + string LicenseNumber          │
│  + Patient(         │    │  + string Specialization         │
│    firstName,       │    │  + DateTime LicenseExpiryDate    │
│    lastName, dob,   │    │  + List<Prescription>            │
│    insurance)       │    │    ProcessedPrescriptions        │
│    [Overloaded]     │    │  ──────────────────────────────  │
│  + AddPrescription( │    │  + Pharmacist()    [Constructor] │
│    prescription)    │    │  + Pharmacist(     [Constructor] │
│    : void           │    │    license, spec)                │
│  + GetMedicalInfo() │    │  + Pharmacist(     [Overloaded   │
│    : string         │    │    license, spec,   Constructor] │
│  + HasAllergy(      │    │    expiry)                       │
│    medication)      │    │  + VerifyLicense() : bool        │
│    : bool           │    │  + ProcessPrescription(          │
└─────────────────────┘    │    prescription) : void          │
                           │  + GetProcessedCount() : int     │
                           └──────────────────────────────────┘

Implements: ISearchable (Patient, Pharmacist)
```

---

### 2. Medication Hierarchy (Complete Structure)

```
┌─────────────────────────────────────────────────────────────┐
│                         Entity                              │
│  ─────────────────────────────────────────────────────────  │
│  # int Id                                                   │
│  # DateTime CreatedAt                                       │
│  # DateTime UpdatedAt                                       │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           │ inherits
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                  Item (abstract)                            │
│  ─────────────────────────────────────────────────────────  │
│  + string Name                                              │
│  + string Description                                       │
│  + Money Price                                              │
│  + string SKU                                               │
│  ─────────────────────────────────────────────────────────  │
│  # Item()                                 [Constructor]     │
│  # Item(name, price)                     [Constructor]     │
│  + GetItemInfo() : string                                   │
│  + UpdatePrice(newPrice) : void                             │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           │ inherits
                           ▼
┌─────────────────────────────────────────────────────────────┐
│              Medication (abstract)                          │
│              implements IInventoryItem, ISearchable         │
│  ─────────────────────────────────────────────────────────  │
│  + string GenericName                                       │
│  + string Manufacturer                                      │
│  + DateTime ExpiryDate                                      │
│  + MedicationCategory Category                              │
│  + string BatchNumber                                       │
│  + Inventory Inventory                    [Navigation]      │
│  ─────────────────────────────────────────────────────────  │
│  # Medication()                           [Protected]       │
│  + Medication(name, price)               [Constructor]     │
│  + Medication(name, genericName, price)  [Overloaded]      │
│  + Medication(name, genericName, price,  [Overloaded]      │
│              category)                                      │
│  + Medication(name, genericName, price,  [Full Constructor]│
│              category, manufacturer,                        │
│              expiryDate)                                    │
│  + Medication(other)                     [Copy Constructor]│
│  ─────────────────────────────────────────────────────────  │
│  + IsExpired() : bool                                       │
│  + GetDaysUntilExpiry() : int                              │
│  + GetMedicationInfo() : string                            │
│  # IInventoryItem Implementation                            │
│  + CurrentStock { get; }                                    │
│  + ReorderLevel { get; }                                    │
│  + IsLowStock() : bool                                      │
│  + RequiresReorder() : bool                                 │
│  + UpdateStock(quantity) : void                             │
│  # ISearchable Implementation                               │
│  + GetSearchableText() : string                            │
│  + MatchesSearch(searchTerm) : bool                        │
└──────────────────────────┬──────────────────────────────────┘
                           │
            ┌──────────────┴──────────────┐
            │                             │
            ▼                             ▼
┌──────────────────────────────┐ ┌───────────────────────────┐
│   PrescriptionMedicine       │ │      OTCMedicine          │
│   implements IPrescribable   │ │                           │
│  ──────────────────────────  │ │  ───────────────────────  │
│  + string DosageForm         │ │  + string                 │
│  + string Strength           │ │    UsageInstructions      │
│  + bool RequiresPrescription │ │  + string                 │
│  + bool ControlledSubstance  │ │    ActiveIngredients      │
│  + int MaxRefills            │ │  + string Warnings        │
│  ──────────────────────────  │ │  + int RecommendedAge     │
│  + PrescriptionMedicine()    │ │  ───────────────────────  │
│    [Constructor]             │ │  + OTCMedicine()          │
│  + PrescriptionMedicine(     │ │    [Constructor]          │
│    name, generic, price)     │ │  + OTCMedicine(           │
│    [Constructor]             │ │    name, generic, price)  │
│  + PrescriptionMedicine(     │ │    [Constructor]          │
│    name, generic, price,     │ │  + OTCMedicine(           │
│    dosageForm, strength)     │ │    name, generic, price,  │
│    [Overloaded Constructor]  │ │    instructions)          │
│  ──────────────────────────  │ │    [Overloaded]           │
│  # IPrescribable             │ │  ───────────────────────  │
│    Implementation            │ │  + CanBeSoldWithout       │
│  + CanDispense(quantity)     │ │    Prescription() : bool  │
│    : bool                    │ │  + GetUsageInfo()         │
│  + GetPrescriptionWarnings() │ │    : string               │
│    : string                  │ │  + IsSafeForAge(age)      │
│  + ValidatePrescription(     │ │    : bool                 │
│    prescription) : bool      │ │                           │
└──────────────────────────────┘ └───────────────────────────┘
```

---

### 3. Operational Classes Structure

```
┌─────────────────────────────────────────────────────────────┐
│                      Inventory                              │
│  ─────────────────────────────────────────────────────────  │
│  + int MedicationId                       [Foreign Key]     │
│  + Medication Medication                  [Navigation]      │
│  + int CurrentQuantity                                      │
│  + int ReorderLevel                                         │
│  + int ReorderQuantity                                      │
│  + string Location                                          │
│  + DateTime? LastRestockDate                                │
│  + int? SupplierId                        [Foreign Key]     │
│  + Supplier Supplier                      [Navigation]      │
│  + string Notes                                             │
│  ─────────────────────────────────────────────────────────  │
│  + Inventory(medicationId, quantity)      [Constructor]     │
│  + Inventory(medicationId, quantity,      [Overloaded]      │
│             reorderLevel)                                   │
│  + Inventory(medicationId, quantity,      [Full Constructor]│
│             reorderLevel, reorderQty,                       │
│             location)                                       │
│  ─────────────────────────────────────────────────────────  │
│  + AddStock(quantity) : void                                │
│  + RemoveStock(quantity) : bool                             │
│  + CheckIfLowStock() : bool                                 │
│  + CheckIfNeedsReorder() : bool                             │
│  + UpdateLocation(location) : void                          │
│  ─────────────────────────────────────────────────────────  │
│  # OPERATOR OVERLOADING                                     │
│  + operator +(Inventory, int) : Inventory  [Add stock]      │
│  + operator -(Inventory, int) : Inventory  [Remove stock]   │
│  + operator <(Inventory, int) : bool       [Compare qty]    │
│  + operator >(Inventory, int) : bool       [Compare qty]    │
│  + operator ==(Inventory, Inventory) : bool                 │
│  + operator !=(Inventory, Inventory) : bool                 │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                      Prescription                           │
│                implements ISearchable                        │
│  ─────────────────────────────────────────────────────────  │
│  + string PrescriptionNumber              [Unique]          │
│  + int PatientId                          [Foreign Key]     │
│  + Patient Patient                        [Navigation]      │
│  + int PharmacistId                       [Foreign Key]     │
│  + Pharmacist Pharmacist                  [Navigation]      │
│  + DateTime PrescriptionDate                                │
│  + PrescriptionStatus Status                                │
│  + string DoctorName                                        │
│  + string DoctorLicenseNumber                               │
│  + List<PrescriptionItem> Medications     [Navigation]      │
│  + string Notes                                             │
│  ─────────────────────────────────────────────────────────  │
│  + Prescription(patientId, pharmacistId)  [Constructor]     │
│  + Prescription(patientId, pharmacistId,  [Overloaded]      │
│                doctorName)                                  │
│  + Prescription(other)                    [Copy Constructor]│
│  ─────────────────────────────────────────────────────────  │
│  + AddMedication(medicationId, quantity,                    │
│                 dosage, frequency) : void                   │
│  + RemoveMedication(medicationId) : void                    │
│  + Dispense() : bool                                        │
│  + Cancel() : void                                          │
│  + GetTotalCost() : Money                                   │
│  + Validate() : bool                                        │
│  # ISearchable Implementation                               │
│  + GetSearchableText() : string                            │
│  + MatchesSearch(searchTerm) : bool                        │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                     Transaction                             │
│  ─────────────────────────────────────────────────────────  │
│  + string TransactionNumber               [Unique]          │
│  + int? PatientId                         [Foreign Key]     │
│  + Patient Patient                        [Navigation]      │
│  + int PharmacistId                       [Foreign Key]     │
│  + Pharmacist Pharmacist                  [Navigation]      │
│  + DateTime TransactionDate                                 │
│  + Money SubTotal                                           │
│  + Money TaxAmount                                          │
│  + Money DiscountAmount                                     │
│  + Money TotalAmount                                        │
│  + string PaymentMethod                                     │
│  + TransactionStatus Status                                 │
│  + int? PrescriptionId                    [Foreign Key]     │
│  + Prescription Prescription              [Navigation]      │
│  + List<TransactionItem> Items            [Navigation]      │
│  + string Notes                                             │
│  ─────────────────────────────────────────────────────────  │
│  + Transaction(pharmacistId)              [Constructor]     │
│  + Transaction(pharmacistId, patientId)   [Overloaded]      │
│  ─────────────────────────────────────────────────────────  │
│  + AddItem(medicationId, quantity) : void                   │
│  + RemoveItem(medicationId) : void                          │
│  + CalculateTotals() : void                                 │
│  + ApplyDiscount(amount) : void                             │
│  + ApplyDiscount(percentage) : void       [Overloaded]      │
│  + Complete() : bool                                        │
│  + Refund() : bool                                          │
│  + GetReceipt() : string                                    │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                       Supplier                              │
│  ─────────────────────────────────────────────────────────  │
│  + string Name                                              │
│  + string ContactPerson                                     │
│  + string Email                                             │
│  + string PhoneNumber                                       │
│  + Address Address                                          │
│  + string Website                                           │
│  + decimal Rating                                           │
│  + bool IsActive                                            │
│  + List<Inventory> SuppliedInventory      [Navigation]      │
│  ─────────────────────────────────────────────────────────  │
│  + Supplier(name)                         [Constructor]     │
│  + Supplier(name, contact, email)         [Overloaded]      │
│  ─────────────────────────────────────────────────────────  │
│  + UpdateRating(newRating) : void                           │
│  + Activate() : void                                        │
│  + Deactivate() : void                                      │
│  + GetSupplierInfo() : string                              │
└─────────────────────────────────────────────────────────────┘
```

---

### 4. Value Objects

```
┌─────────────────────────────────────────────────────────────┐
│                         Money                               │
│                   (Value Object)                            │
│  ─────────────────────────────────────────────────────────  │
│  + decimal Amount                         [readonly]        │
│  + string Currency                        [readonly]        │
│  ─────────────────────────────────────────────────────────  │
│  + Money(amount)                          [Constructor]     │
│  + Money(amount, currency)                [Overloaded]      │
│  ─────────────────────────────────────────────────────────  │
│  + ToString() : string                                      │
│  + Equals(other) : bool                                     │
│  + GetHashCode() : int                                      │
│  ─────────────────────────────────────────────────────────  │
│  # OPERATOR OVERLOADING                                     │
│  + operator +(Money, Money) : Money       [Addition]        │
│  + operator -(Money, Money) : Money       [Subtraction]     │
│  + operator *(Money, decimal) : Money     [Multiplication]  │
│  + operator /(Money, decimal) : Money     [Division]        │
│  + operator >(Money, Money) : bool        [Comparison]      │
│  + operator <(Money, Money) : bool        [Comparison]      │
│  + operator >=(Money, Money) : bool       [Comparison]      │
│  + operator <=(Money, Money) : bool       [Comparison]      │
│  + operator ==(Money, Money) : bool       [Equality]        │
│  + operator !=(Money, Money) : bool       [Inequality]      │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                        Address                              │
│                   (Value Object)                            │
│  ─────────────────────────────────────────────────────────  │
│  + string Street                          [readonly]        │
│  + string City                            [readonly]        │
│  + string State                           [readonly]        │
│  + string ZipCode                         [readonly]        │
│  + string Country                         [readonly]        │
│  ─────────────────────────────────────────────────────────  │
│  + Address(street, city, state,           [Constructor]     │
│           zipCode, country)                                 │
│  ─────────────────────────────────────────────────────────  │
│  + ToString() : string                                      │
│  + Equals(other) : bool                                     │
│  + GetHashCode() : int                                      │
│  + GetFormattedAddress() : string                          │
└─────────────────────────────────────────────────────────────┘
```

---

### 5. Interface Definitions

```
┌─────────────────────────────────────────────────────────────┐
│                  <<interface>>                              │
│                  IInventoryItem                             │
│  ─────────────────────────────────────────────────────────  │
│  + int Id { get; }                                          │
│  + string Name { get; }                                     │
│  + int CurrentStock { get; }                                │
│  + int ReorderLevel { get; }                                │
│  ─────────────────────────────────────────────────────────  │
│  + IsLowStock() : bool                                      │
│  + RequiresReorder() : bool                                 │
│  + UpdateStock(quantity : int) : void                       │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                  <<interface>>                              │
│                  IPrescribable                              │
│  ─────────────────────────────────────────────────────────  │
│  + string Name { get; }                                     │
│  + string GenericName { get; }                              │
│  + string DosageForm { get; }                               │
│  + string Strength { get; }                                 │
│  + bool RequiresPrescription { get; }                       │
│  ─────────────────────────────────────────────────────────  │
│  + CanDispense(quantity : int) : bool                       │
│  + GetPrescriptionWarnings() : string                       │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                  <<interface>>                              │
│                   ISearchable                               │
│  ─────────────────────────────────────────────────────────  │
│  + int Id { get; }                                          │
│  ─────────────────────────────────────────────────────────  │
│  + GetSearchableText() : string                            │
│  + MatchesSearch(searchTerm : string) : bool               │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                  <<interface>>                              │
│                IRepository<T> where T : Entity              │
│  ─────────────────────────────────────────────────────────  │
│  + GetByIdAsync(id : int) : Task<T>                        │
│  + GetAllAsync() : Task<IEnumerable<T>>                    │
│  + AddAsync(entity : T) : Task<T>                          │
│  + UpdateAsync(entity : T) : Task                          │
│  + DeleteAsync(id : int) : Task                            │
│  + ExistsAsync(id : int) : Task<bool>                      │
└─────────────────────────────────────────────────────────────┘
```

---

### 6. Static Configuration Class

```
┌─────────────────────────────────────────────────────────────┐
│              <<static class>>                               │
│                 SystemConfig                                │
│  ─────────────────────────────────────────────────────────  │
│  # STATIC FIELDS                                            │
│  + static readonly decimal TaxRate = 0.15m                  │
│  + static readonly int DefaultReorderLevel = 50            │
│  + static readonly int DefaultReorderQuantity = 100        │
│  + static readonly int LowStockThreshold = 20              │
│  + static readonly decimal DiscountThreshold = 1000m       │
│  + static readonly decimal BulkDiscountRate = 0.10m        │
│  + static readonly int MaxPrescriptionRefills = 5          │
│  ─────────────────────────────────────────────────────────  │
│  # STATIC METHODS                                           │
│  + static CalculateTax(amount : decimal) : decimal          │
│  + static ApplyDiscount(amount : decimal,                   │
│                        discountRate : decimal) : decimal    │
│  + static IsLowStock(quantity : int) : bool                 │
│  + static RequiresReorder(current : int,                    │
│                          reorderLevel : int) : bool         │
│  + static GeneratePrescriptionNumber() : string             │
│  + static GenerateTransactionNumber() : string              │
│  + static ValidateEmail(email : string) : bool              │
│  + static ValidatePhoneNumber(phone : string) : bool        │
│  + static FormatCurrency(amount : decimal) : string         │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│              <<static class>>                               │
│                ConsoleHelper                                │
│  ─────────────────────────────────────────────────────────  │
│  # STATIC METHODS                                           │
│  + static PrintHeader(title : string) : void                │
│  + static PrintSuccess(message : string) : void             │
│  + static PrintError(message : string) : void               │
│  + static PrintWarning(message : string) : void             │
│  + static PrintInfo(message : string) : void                │
│  + static GetIntInput(prompt : string) : int                │
│  + static GetDecimalInput(prompt : string) : decimal        │
│  + static GetStringInput(prompt : string) : string          │
│  + static GetDateInput(prompt : string) : DateTime          │
│  + static ConfirmAction(message : string) : bool            │
│  + static ClearScreen() : void                              │
│  + static PauseForUser() : void                             │
└─────────────────────────────────────────────────────────────┘
```

---

## Entity Relationships Diagram

```
┌──────────┐                ┌──────────────┐                ┌────────────┐
│ Patient  │───────────────▶│ Prescription │◀───────────────│ Pharmacist │
│          │ 1           * │              │ *           1  │            │
└──────────┘   places      └──────┬───────┘   creates      └────────────┘
                                  │
                                  │ contains
                                  │ * (Many-to-Many)
                                  │
                           ┌──────▼──────────┐
                           │PrescriptionItem │
                           │  (Junction)     │
                           └──────┬──────────┘
                                  │
                                  │ *
                                  │
                                  │ 1
                           ┌──────▼──────────┐
                           │   Medication    │
                           └──────┬──────────┘
                                  │
                                  │ 1:1
                                  │
                           ┌──────▼──────────┐
                           │   Inventory     │
                           └──────┬──────────┘
                                  │
                                  │ * supplied by
                                  │
                                  │ 1
                           ┌──────▼──────────┐
                           │    Supplier     │
                           └─────────────────┘

┌──────────────┐
│ Transaction  │
└──────┬───────┘
       │ contains
       │ * (Many-to-Many)
       │
┌──────▼────────┐
│TransactionItem│
│  (Junction)   │
└──────┬────────┘
       │
       │ *
       │
       │ 1
┌──────▼──────────┐
│   Medication    │
└─────────────────┘
```

---

## Implementation Relationships

### Medication Implements Multiple Interfaces:
```
Medication
   │
   ├─ implements IInventoryItem
   │     └─ Provides: CurrentStock, IsLowStock(), UpdateStock()
   │
   └─ implements ISearchable
         └─ Provides: GetSearchableText(), MatchesSearch()

PrescriptionMedicine (extends Medication)
   │
   └─ implements IPrescribable
         └─ Provides: CanDispense(), GetPrescriptionWarnings()
```

### User Hierarchy Implements ISearchable:
```
Patient implements ISearchable
   └─ Searches by: Name, Phone, Insurance Number

Pharmacist implements ISearchable
   └─ Searches by: Name, License Number, Specialization

Prescription implements ISearchable
   └─ Searches by: Prescription Number, Patient Name, Date
```

---

## Key Design Patterns Used

### 1. Repository Pattern
```
IRepository<T>
   ├─ GenericRepository<T>
   │     └─ Base CRUD operations
   │
   ├─ MedicationRepository : GenericRepository<Medication>
   │     └─ + SearchByCategory(), GetExpiringSoon()
   │
   ├─ PrescriptionRepository : GenericRepository<Prescription>
   │     └─ + GetByPatient(), GetPending()
   │
   └─ InventoryRepository : GenericRepository<Inventory>
         └─ + GetLowStock(), GetNeedingReorder()
```

### 2. Value Object Pattern
```
Money (immutable)
   └─ Ensures money calculations are type-safe

Address (immutable)
   └─ Encapsulates address validation and formatting
```

### 3. Table Per Hierarchy (TPH) Inheritance
```
Users table contains:
   └─ Discriminator column → 'Patient', 'Employee', 'Pharmacist'

Medications table contains:
   └─ Discriminator column → 'PrescriptionMedicine', 'OTCMedicine'
```

---

## Constructor Overloading Examples

### Medication Class:
```csharp
// Constructor 1: Minimal
public Medication(string name, decimal price)

// Constructor 2: With generic name
public Medication(string name, string genericName, decimal price)

// Constructor 3: With category
public Medication(string name, string genericName, decimal price, 
                 MedicationCategory category)

// Constructor 4: Full details
public Medication(string name, string genericName, decimal price,
                 MedicationCategory category, string manufacturer,
                 DateTime expiryDate)

// Copy Constructor
public Medication(Medication other)
```

### Inventory Class:
```csharp
// Constructor 1: Basic
public Inventory(int medicationId, int quantity)

// Constructor 2: With reorder level
public Inventory(int medicationId, int quantity, int reorderLevel)

// Constructor 3: Full
public Inventory(int medicationId, int quantity, int reorderLevel,
                int reorderQuantity, string location)
```

---

## Function Overloading Examples

### MedicationService:
```csharp
// Search overloads
public List<Medication> Search(string name)
public List<Medication> Search(MedicationCategory category)
public List<Medication> Search(string name, MedicationCategory category)
public List<Medication> Search(decimal minPrice, decimal maxPrice)
public List<Medication> Search(string name, decimal minPrice, decimal maxPrice)
```

### InventoryService:
```csharp
// AddStock overloads
public void AddStock(int medicationId, int quantity)
public void AddStock(int medicationId, int quantity, string notes)
public void AddStock(Medication medication, int quantity)
public void AddStock(Medication medication, int quantity, int supplierId)
public void AddStock(Medication medication, int quantity, 
                    int supplierId, string notes)
```

---

This comprehensive class diagram document covers all aspects of the system architecture and demonstrates all required OOP concepts!