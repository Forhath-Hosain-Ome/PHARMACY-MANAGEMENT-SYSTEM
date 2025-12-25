namespace PharmacySystem.Core.Entities.ValueObjects;

/// <summary>
/// Value object representing a physical address
/// Immutable object following value object pattern
/// </summary>
public class Address : IEquatable<Address>
{
    /// <summary>
    /// Street address
    /// </summary>
    public string Street { get; }

    /// <summary>
    /// City name
    /// </summary>
    public string City { get; }

    /// <summary>
    /// State or province
    /// </summary>
    public string State { get; }

    /// <summary>
    /// Postal/ZIP code
    /// </summary>
    public string ZipCode { get; }

    /// <summary>
    /// Country name
    /// </summary>
    public string Country { get; }

    /// <summary>
    /// Constructor for Address (all parameters required)
    /// </summary>
    public Address(string street, string city, string state, string zipCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));
        
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State cannot be empty", nameof(state));
        
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException("ZipCode cannot be empty", nameof(zipCode));
        
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    /// <summary>
    /// Gets a formatted single-line address
    /// </summary>
    public string GetFormattedAddress()
    {
        return $"{Street}, {City}, {State} {ZipCode}, {Country}";
    }

    /// <summary>
    /// Gets a formatted multi-line address
    /// </summary>
    public string GetFormattedAddressMultiLine()
    {
        return $"{Street}\n{City}, {State} {ZipCode}\n{Country}";
    }

    /// <summary>
    /// Determines whether the specified Address is equal to the current Address
    /// </summary>
    public bool Equals(Address? other)
    {
        if (other is null) return false;
        
        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               ZipCode == other.ZipCode &&
               Country == other.Country;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current Address
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Address other && Equals(other);
    }

    /// <summary>
    /// Returns the hash code for this Address
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode, Country);
    }

    /// <summary>
    /// Returns a string representation of the Address
    /// </summary>
    public override string ToString()
    {
        return GetFormattedAddress();
    }

    /// <summary>
    /// Equality operator
    /// </summary>
    public static bool operator ==(Address? left, Address? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator
    /// </summary>
    public static bool operator !=(Address? left, Address? right)
    {
        return !(left == right);
    }
}
