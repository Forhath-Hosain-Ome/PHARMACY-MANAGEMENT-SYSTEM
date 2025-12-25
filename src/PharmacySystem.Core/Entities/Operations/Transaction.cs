using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.People;
using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.Operations;

/// <summary>
/// Represents a sales transaction in the pharmacy
/// Demonstrates function overloading for discount methods
/// </summary>
public class Transaction : Entity
{
    /// <summary>
    /// Unique transaction number
    /// </summary>
    public string TransactionNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Foreign key to Patient (optional - walk-in customers)
    /// </summary>
    public int? PatientId { get; private set; }

    /// <summary>
    /// Navigation property to Patient
    /// </summary>
    public virtual Patient? Patient { get; private set; }

    /// <summary>
    /// Foreign key to Pharmacist handling the transaction
    /// </summary>
    public int PharmacistId { get; private set; }

    /// <summary>
    /// Navigation property to Pharmacist
    /// </summary>
    public virtual Pharmacist? Pharmacist { get; private set; }

    /// <summary>
    /// Transaction date and time
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// Subtotal before tax and discounts
    /// </summary>
    public Money SubTotal { get; private set; } = new Money(0);

    /// <summary>
    /// Tax amount
    /// </summary>
    public Money TaxAmount { get; private set; } = new Money(0);

    /// <summary>
    /// Discount amount
    /// </summary>
    public Money DiscountAmount { get; private set; } = new Money(0);

    /// <summary>
    /// Total amount after tax and discounts
    /// </summary>
    public Money TotalAmount { get; private set; } = new Money(0);

    /// <summary>
    /// Payment method used
    /// </summary>
    public PaymentMethod PaymentMethod { get; private set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    public TransactionStatus Status { get; private set; }

    /// <summary>
    /// Related prescription ID (if applicable)
    /// </summary>
    public int? PrescriptionId { get; private set; }

    /// <summary>
    /// Navigation property to Prescription
    /// </summary>
    public virtual Prescription? Prescription { get; private set; }

    /// <summary>
    /// Collection of items in this transaction
    /// </summary>
    public virtual ICollection<TransactionItem> Items { get; private set; } = new List<TransactionItem>();

    /// <summary>
    /// Additional notes
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private Transaction() : base()
    {
    }

    /// <summary>
    /// Constructor with pharmacist only (Constructor Overloading - 1)
    /// </summary>
    public Transaction(int pharmacistId)
    {
        if (pharmacistId <= 0)
            throw new ArgumentException("Pharmacist ID must be positive", nameof(pharmacistId));

        PharmacistId = pharmacistId;
        TransactionDate = DateTime.Now;
        Status = TransactionStatus.Pending;
        TransactionNumber = GenerateTransactionNumber();
        PaymentMethod = PaymentMethod.Cash; // Default
    }

    /// <summary>
    /// Constructor with patient (Constructor Overloading - 2)
    /// </summary>
    public Transaction(int pharmacistId, int patientId) : this(pharmacistId)
    {
        if (patientId <= 0)
            throw new ArgumentException("Patient ID must be positive", nameof(patientId));

        PatientId = patientId;
    }

    /// <summary>
    /// Constructor with prescription (Constructor Overloading - 3)
    /// </summary>
    public Transaction(int pharmacistId, int patientId, int prescriptionId)
        : this(pharmacistId, patientId)
    {
        if (prescriptionId <= 0)
            throw new ArgumentException("Prescription ID must be positive", nameof(prescriptionId));

        PrescriptionId = prescriptionId;
    }

    /// <summary>
    /// Generates a unique transaction number
    /// </summary>
    private static string GenerateTransactionNumber()
    {
        return $"TXN-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    /// <summary>
    /// Adds an item to the transaction
    /// </summary>
    public void AddItem(int medicationId, int quantity, Money unitPrice)
    {
        if (medicationId <= 0)
            throw new ArgumentException("Medication ID must be positive", nameof(medicationId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Cannot modify a non-pending transaction");

        var item = new TransactionItem(Id, medicationId, quantity, unitPrice);
        Items.Add(item);
        CalculateTotals();
        UpdateTimestamp();
    }

    /// <summary>
    /// Removes an item from the transaction
    /// </summary>
    public bool RemoveItem(int medicationId)
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Cannot modify a non-pending transaction");

        var item = Items.FirstOrDefault(i => i.MedicationId == medicationId);
        if (item == null)
            return false;

        Items.Remove(item);
        CalculateTotals();
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Calculates all totals (subtotal, tax, discount, total)
    /// </summary>
    public void CalculateTotals()
    {
        if (!Items.Any())
        {
            SubTotal = new Money(0);
            TaxAmount = new Money(0);
            TotalAmount = new Money(0);
            return;
        }

        // Calculate subtotal
        decimal subtotal = Items.Sum(i => i.TotalPrice.Amount);
        SubTotal = new Money(subtotal);

        // Calculate total after discount
        var discountedAmount = SubTotal - DiscountAmount;

        // Calculate tax on discounted amount
        TaxAmount = discountedAmount * Configuration.SystemConfig.TaxRate;

        // Calculate final total
        TotalAmount = discountedAmount + TaxAmount;

        UpdateTimestamp();
    }

    /// <summary>
    /// Applies a fixed discount amount (Function Overloading - 1)
    /// </summary>
    public void ApplyDiscount(Money amount)
    {
        if (amount == null)
            throw new ArgumentNullException(nameof(amount));

        if (amount.Amount < 0)
            throw new ArgumentException("Discount amount cannot be negative");

        if (amount > SubTotal)
            throw new ArgumentException("Discount amount cannot exceed subtotal");

        DiscountAmount = amount;
        CalculateTotals();
    }

    /// <summary>
    /// Applies a percentage discount (Function Overloading - 2)
    /// </summary>
    public void ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 0 and 100", nameof(percentage));

        var discountAmount = SubTotal * (percentage / 100);
        DiscountAmount = discountAmount;
        CalculateTotals();
    }

    /// <summary>
    /// Applies a discount with a reason (Function Overloading - 3)
    /// </summary>
    public void ApplyDiscount(Money amount, string reason)
    {
        ApplyDiscount(amount);
        Notes = $"Discount applied: {reason}";
    }

    /// <summary>
    /// Applies a percentage discount with a reason (Function Overloading - 4)
    /// </summary>
    public void ApplyDiscount(decimal percentage, string reason)
    {
        ApplyDiscount(percentage);
        Notes = $"Discount applied ({percentage}%): {reason}";
    }

    /// <summary>
    /// Removes any applied discount
    /// </summary>
    public void RemoveDiscount()
    {
        DiscountAmount = new Money(0);
        CalculateTotals();
    }

    /// <summary>
    /// Sets the payment method
    /// </summary>
    public void SetPaymentMethod(PaymentMethod method)
    {
        PaymentMethod = method;
        UpdateTimestamp();
    }

    /// <summary>
    /// Completes the transaction
    /// </summary>
    public bool Complete()
    {
        if (Status != TransactionStatus.Pending)
            return false;

        if (!Items.Any())
            throw new InvalidOperationException("Cannot complete transaction with no items");

        Status = TransactionStatus.Completed;
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Processes a refund for the entire transaction
    /// </summary>
    public bool Refund()
    {
        if (Status != TransactionStatus.Completed)
            return false;

        Status = TransactionStatus.Refunded;
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Processes a partial refund
    /// </summary>
    public bool PartialRefund(Money amount)
    {
        if (Status != TransactionStatus.Completed && Status != TransactionStatus.PartiallyRefunded)
            return false;

        if (amount > TotalAmount)
            throw new ArgumentException("Refund amount cannot exceed total amount");

        Status = TransactionStatus.PartiallyRefunded;
        // In a real system, you'd track the refunded amount separately
        UpdateTimestamp();
        return true;
    }

    /// <summary>
    /// Cancels the transaction
    /// </summary>
    public void Cancel(string reason)
    {
        if (Status == TransactionStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed transaction. Use refund instead.");

        Status = TransactionStatus.Cancelled;
        Notes = $"Cancelled: {reason}";
        UpdateTimestamp();
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
    /// Gets a receipt for the transaction
    /// </summary>
    public string GetReceipt()
    {
        var receipt = $"===== RECEIPT =====\n";
        receipt += $"Transaction #: {TransactionNumber}\n";
        receipt += $"Date: {TransactionDate:yyyy-MM-dd HH:mm:ss}\n";
        receipt += $"Pharmacist ID: {PharmacistId}\n";
        
        if (PatientId.HasValue)
            receipt += $"Patient ID: {PatientId}\n";

        receipt += $"\n--- ITEMS ---\n";
        foreach (var item in Items)
        {
            receipt += $"Med ID {item.MedicationId}: {item.Quantity} x {item.UnitPrice.ToDisplayString()} = {item.TotalPrice.ToDisplayString()}\n";
        }

        receipt += $"\n--- TOTALS ---\n";
        receipt += $"Subtotal: {SubTotal.ToDisplayString()}\n";
        
        if (DiscountAmount.Amount > 0)
            receipt += $"Discount: -{DiscountAmount.ToDisplayString()}\n";
        
        receipt += $"Tax: {TaxAmount.ToDisplayString()}\n";
        receipt += $"TOTAL: {TotalAmount.ToDisplayString()}\n";
        receipt += $"Payment Method: {PaymentMethod}\n";
        receipt += $"Status: {Status}\n";

        if (!string.IsNullOrEmpty(Notes))
            receipt += $"\nNotes: {Notes}\n";

        receipt += $"\nThank you for your business!\n";
        receipt += $"==================\n";

        return receipt;
    }

    /// <summary>
    /// Gets transaction summary
    /// </summary>
    public string GetTransactionSummary()
    {
        return $"Transaction: {TransactionNumber}\n" +
               $"Date: {TransactionDate:yyyy-MM-dd HH:mm:ss}\n" +
               $"Pharmacist ID: {PharmacistId}\n" +
               $"Patient ID: {PatientId?.ToString() ?? "Walk-in"}\n" +
               $"Items: {Items.Count}\n" +
               $"Total: {TotalAmount.ToDisplayString()}\n" +
               $"Payment: {PaymentMethod}\n" +
               $"Status: {Status}";
    }
}

/// <summary>
/// Represents an individual medication item in a transaction
/// </summary>
public class TransactionItem : Entity
{
    /// <summary>
    /// Foreign key to Transaction
    /// </summary>
    public int TransactionId { get; private set; }

    /// <summary>
    /// Foreign key to Medication
    /// </summary>
    public int MedicationId { get; private set; }

    /// <summary>
    /// Navigation property to Medication
    /// </summary>
    public virtual Medication? Medication { get; private set; }

    /// <summary>
    /// Quantity purchased
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price at time of purchase
    /// </summary>
    public Money UnitPrice { get; private set; }

    /// <summary>
    /// Total price (Quantity * UnitPrice)
    /// </summary>
    public Money TotalPrice { get; private set; }

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private TransactionItem() : base()
    {
        UnitPrice = new Money(0);
        TotalPrice = new Money(0);
    }

    /// <summary>
    /// Constructor for transaction item
    /// </summary>
    public TransactionItem(int transactionId, int medicationId, int quantity, Money unitPrice)
    {
        if (transactionId <= 0)
            throw new ArgumentException("Transaction ID must be positive", nameof(transactionId));

        if (medicationId <= 0)
            throw new ArgumentException("Medication ID must be positive", nameof(medicationId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice == null)
            throw new ArgumentNullException(nameof(unitPrice));

        TransactionId = transactionId;
        MedicationId = medicationId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = unitPrice * quantity;
    }

    /// <summary>
    /// Updates quantity
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(newQuantity));

        Quantity = newQuantity;
        TotalPrice = UnitPrice * newQuantity;
        UpdateTimestamp();
    }
}
