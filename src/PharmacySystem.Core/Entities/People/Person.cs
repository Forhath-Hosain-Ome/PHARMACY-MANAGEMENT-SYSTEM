using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.People;

/// <summary>
/// Abstract class representing a person in the system
/// Inherits from User and adds personal information
/// </summary>
public abstract class Person : User
{
    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; protected set; } = string.Empty;

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; protected set; } = string.Empty;

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; protected set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; protected set; }

    /// <summary>
    /// Physical address
    /// </summary>
    public Address? Address { get; protected set; }

    /// <summary>
    /// Protected constructor for Entity Framework Core
    /// </summary>
    protected Person() : base()
    {
    }

    /// <summary>
    /// Protected constructor with basic information (Constructor Overloading - 1)
    /// </summary>
    protected Person(string username, string email, UserRole role, 
                    string firstName, string lastName, DateTime dateOfBirth)
        : base(username, email, role)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        if (dateOfBirth > DateTime.Now)
            throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    /// <summary>
    /// Protected constructor with contact information (Constructor Overloading - 2)
    /// </summary>
    protected Person(string username, string email, UserRole role,
                    string firstName, string lastName, DateTime dateOfBirth,
                    string phoneNumber)
        : this(username, email, role, firstName, lastName, dateOfBirth)
    {
        PhoneNumber = phoneNumber;
    }

    /// <summary>
    /// Protected constructor with full information (Constructor Overloading - 3)
    /// </summary>
    protected Person(string username, string email, UserRole role,
                    string firstName, string lastName, DateTime dateOfBirth,
                    string phoneNumber, Address address)
        : this(username, email, role, firstName, lastName, dateOfBirth, phoneNumber)
    {
        Address = address;
    }

    /// <summary>
    /// Gets the full name of the person
    /// </summary>
    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    /// <summary>
    /// Gets the person's age
    /// </summary>
    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        
        if (DateOfBirth.Date > today.AddYears(-age))
            age--;
        
        return age;
    }

    /// <summary>
    /// Updates contact information
    /// </summary>
    public virtual void UpdateContactInfo(string? phoneNumber, Address? address)
    {
        PhoneNumber = phoneNumber;
        Address = address;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates personal information
    /// </summary>
    public virtual void UpdatePersonalInfo(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets detailed personal information
    /// </summary>
    public override string GetUserInfo()
    {
        return $"{GetFullName()} ({Username}) - {Email} - Age: {GetAge()}";
    }
}
