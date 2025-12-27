namespace PharmacySystem.Core.Enums;

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
public enum PrescriptionStatus
{
    Pending,
    Dispensed,
    PartiallyDispensed,
    Cancelled,
    Expired
}
public enum TransactionStatus
{
    Pending,
    Completed,
    Refunded,
    PartiallyRefunded,
    Cancelled
}
public enum UserRole
{
    Admin,
    Pharmacist,
    PharmacyTechnician,
    Patient,
    Manager
}
public enum PaymentMethod
{
    Cash,
    CreditCard,
    DebitCard,
    Insurance,
    MobilePayment,
    BankTransfer
}
