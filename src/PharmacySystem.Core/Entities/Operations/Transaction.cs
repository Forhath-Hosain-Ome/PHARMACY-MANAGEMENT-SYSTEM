using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.People;
using PharmacySystem.Core.Entities.Items;
using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.Operations;

public class Transaction : Entity
{
    public string TransactionNumber { get; private set; } = string.Empty;
    public int? PatientId { get; private set; }
    public virtual Patient? Patient { get; private set; }
    public int PharmacistId { get; private set; }
    public virtual Pharmacist? Pharmacist { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public Money SubTotal { get; private set; } = new Money(0);
    public Money TaxAmount { get; private set; } = new Money(0);
    public Money DiscountAmount { get; private set; } = new Money(0);
    public Money TotalAmount { get; private set; } = new Money(0);
    public PaymentMethod PaymentMethod { get; private set; }
    public TransactionStatus Status { get; private set; }
    public int? PrescriptionId { get; private set; }
    public virtual Prescription? Prescription { get; private set; }
    public virtual ICollection<TransactionItem> Items { get; private set; } = new List<TransactionItem>();
    public string? Notes { get; private set; }
    private Transaction() : base()
    {
    }

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

    public Transaction(int pharmacistId, int patientId) : this(pharmacistId)
    {
        if (patientId <= 0)
            throw new ArgumentException("Patient ID must be positive", nameof(patientId));

        PatientId = patientId;
    }

    public Transaction(int pharmacistId, int patientId, int prescriptionId)
        : this(pharmacistId, patientId)
    {
        if (prescriptionId <= 0)
            throw new ArgumentException("Prescription ID must be positive", nameof(prescriptionId));

        PrescriptionId = prescriptionId;
    }

    private static string GenerateTransactionNumber()
    {
        return $"TXN-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

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

    public void ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 0 and 100", nameof(percentage));

        var discountAmount = SubTotal * (percentage / 100);
        DiscountAmount = discountAmount;
        CalculateTotals();
    }

    public void ApplyDiscount(Money amount, string reason)
    {
        ApplyDiscount(amount);
        Notes = $"Discount applied: {reason}";
    }

    public void ApplyDiscount(decimal percentage, string reason)
    {
        ApplyDiscount(percentage);
        Notes = $"Discount applied ({percentage}%): {reason}";
    }

    public void RemoveDiscount()
    {
        DiscountAmount = new Money(0);
        CalculateTotals();
    }

    public void SetPaymentMethod(PaymentMethod method)
    {
        PaymentMethod = method;
        UpdateTimestamp();
    }

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

    public bool Refund()
    {
        if (Status != TransactionStatus.Completed)
            return false;

        Status = TransactionStatus.Refunded;
        UpdateTimestamp();
        return true;
    }

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

    public void Cancel(string reason)
    {
        if (Status == TransactionStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed transaction. Use refund instead.");

        Status = TransactionStatus.Cancelled;
        Notes = $"Cancelled: {reason}";
        UpdateTimestamp();
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdateTimestamp();
    }

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

public class TransactionItem : Entity
{
    public int TransactionId { get; private set; }
    public int MedicationId { get; private set; }
    public virtual Medication? Medication { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalPrice { get; private set; }
    private TransactionItem() : base()
    {
        UnitPrice = new Money(0);
        TotalPrice = new Money(0);
    }

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

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(newQuantity));

        Quantity = newQuantity;
        TotalPrice = UnitPrice * newQuantity;
        UpdateTimestamp();
    }
}
