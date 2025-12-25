namespace PharmacySystem.Core.Enums;

/// <summary>
/// Categories of medications available in the pharmacy
/// </summary>
public enum MedicationCategory
{
    Antibiotic,
    Painkiller,
    Vitamin,
    Supplement,
    Cardiovascular,
    Respiratory,
    Gastrointestinal,
    Dermatological,
    Neurological,
    Endocrine,
    Other
}

/// <summary>
/// Status of a prescription
/// </summary>
public enum PrescriptionStatus
{
    Pending,
    Dispensed,
    PartiallyDispensed,
    Cancelled,
    Expired
}

/// <summary>
/// Status of a transaction
/// </summary>
public enum TransactionStatus
{
    Pending,
    Completed,
    Refunded,
    PartiallyRefunded,
    Cancelled
}

/// <summary>
/// Roles available in the system
/// </summary>
public enum UserRole
{
    Admin,
    Pharmacist,
    PharmacyTechnician,
    Patient,
    Manager
}

/// <summary>
/// Payment methods accepted
/// </summary>
public enum PaymentMethod
{
    Cash,
    CreditCard,
    DebitCard,
    Insurance,
    MobilePayment,
    BankTransfer
}
