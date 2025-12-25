namespace PharmacySystem.Core.Entities.ValueObjects;

/// <summary>
/// Value object representing monetary amounts
/// Demonstrates operator overloading for mathematical operations
/// </summary>
public class Money : IEquatable<Money>
{
    /// <summary>
    /// The monetary amount
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// The currency code (default: USD)
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Constructor with amount only (defaults to USD)
    /// </summary>
    public Money(decimal amount) : this(amount, "USD")
    {
    }

    /// <summary>
    /// Constructor with amount and currency (Constructor Overloading)
    /// </summary>
    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpper();
    }

    /// <summary>
    /// Addition operator overloading
    /// </summary>
    public static Money operator +(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    /// <summary>
    /// Subtraction operator overloading
    /// </summary>
    public static Money operator -(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        if (left.Amount < right.Amount)
            throw new InvalidOperationException("Cannot subtract to negative amount");
        
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    /// <summary>
    /// Multiplication operator overloading (for tax, discount calculations)
    /// </summary>
    public static Money operator *(Money money, decimal multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException("Multiplier cannot be negative", nameof(multiplier));
        
        return new Money(money.Amount * multiplier, money.Currency);
    }

    /// <summary>
    /// Division operator overloading
    /// </summary>
    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide by zero");
        
        if (divisor < 0)
            throw new ArgumentException("Divisor cannot be negative", nameof(divisor));
        
        return new Money(money.Amount / divisor, money.Currency);
    }

    /// <summary>
    /// Greater than operator overloading
    /// </summary>
    public static bool operator >(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount > right.Amount;
    }

    /// <summary>
    /// Less than operator overloading
    /// </summary>
    public static bool operator <(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount < right.Amount;
    }

    /// <summary>
    /// Greater than or equal operator overloading
    /// </summary>
    public static bool operator >=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount >= right.Amount;
    }

    /// <summary>
    /// Less than or equal operator overloading
    /// </summary>
    public static bool operator <=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount <= right.Amount;
    }

    /// <summary>
    /// Equality operator overloading
    /// </summary>
    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator overloading
    /// </summary>
    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Validates that two Money objects have the same currency
    /// </summary>
    private static void ValidateSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Cannot operate on different currencies: {left.Currency} and {right.Currency}");
    }

    /// <summary>
    /// Determines whether the specified Money is equal to the current Money
    /// </summary>
    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current Money
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this Money
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    /// <summary>
    /// Returns a string representation of the Money
    /// </summary>
    public override string ToString()
    {
        return $"{Currency} {Amount:N2}";
    }

    /// <summary>
    /// Formats the money for display
    /// </summary>
    public string ToDisplayString()
    {
        return Currency switch
        {
            "USD" => $"${Amount:N2}",
            "EUR" => $"€{Amount:N2}",
            "GBP" => $"£{Amount:N2}",
            _ => ToString()
        };
    }
}
