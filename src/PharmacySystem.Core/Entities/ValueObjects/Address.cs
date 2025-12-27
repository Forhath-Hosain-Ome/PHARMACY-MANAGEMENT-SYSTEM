namespace PharmacySystem.Core.Entities.ValueObjects;

public class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }
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
    public string GetFormattedAddress()
    {
        return $"{Street}, {City}, {State} {ZipCode}, {Country}";
    }
    public string GetFormattedAddressMultiLine()
    {
        return $"{Street}\n{City}, {State} {ZipCode}\n{Country}";
    }
    public bool Equals(Address? other)
    {
        if (other is null) return false;
        
        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               ZipCode == other.ZipCode &&
               Country == other.Country;
    }
    public override bool Equals(object? obj)
    {
        return obj is Address other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode, Country);
    }
    public override string ToString()
    {
        return GetFormattedAddress();
    }
    public static bool operator ==(Address? left, Address? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }
    public static bool operator !=(Address? left, Address? right)
    {
        return !(left == right);
    }
}
