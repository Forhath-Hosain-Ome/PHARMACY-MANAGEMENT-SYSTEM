namespace PharmacySystem.Core.Entities.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    public Money(decimal amount) : this(amount, "USD")
    {
    }
    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpper();
    }

    public static Money operator +(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        if (left.Amount < right.Amount)
            throw new InvalidOperationException("Cannot subtract to negative amount");
        
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException("Multiplier cannot be negative", nameof(multiplier));
        
        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide by zero");
        
        if (divisor < 0)
            throw new ArgumentException("Divisor cannot be negative", nameof(divisor));
        
        return new Money(money.Amount / divisor, money.Currency);
    }

    public static bool operator >(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount < right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount <= right.Amount;
    }

    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }

    private static void ValidateSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Cannot operate on different currencies: {left.Currency} and {right.Currency}");
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public override string ToString()
    {
        return $"{Currency} {Amount:N2}";
    }

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
