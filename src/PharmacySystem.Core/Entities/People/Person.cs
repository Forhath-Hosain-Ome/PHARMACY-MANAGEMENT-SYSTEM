using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.People;

public abstract class Person : User
{
    public string FirstName { get; protected set; } = string.Empty;
    public string LastName { get; protected set; } = string.Empty;
    public DateTime DateOfBirth { get; protected set; }
    public string? PhoneNumber { get; protected set; }
    public Address? Address { get; protected set; }
    protected Person() : base()
    {
    }
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
    protected Person(string username, string email, UserRole role,
                    string firstName, string lastName, DateTime dateOfBirth,
                    string phoneNumber)
        : this(username, email, role, firstName, lastName, dateOfBirth)
    {
        PhoneNumber = phoneNumber;
    }
    protected Person(string username, string email, UserRole role,
                    string firstName, string lastName, DateTime dateOfBirth,
                    string phoneNumber, Address address)
        : this(username, email, role, firstName, lastName, dateOfBirth, phoneNumber)
    {
        Address = address;
    }
    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        
        if (DateOfBirth.Date > today.AddYears(-age))
            age--;
        
        return age;
    }
    public virtual void UpdateContactInfo(string? phoneNumber, Address? address)
    {
        PhoneNumber = phoneNumber;
        Address = address;
        UpdateTimestamp();
    }
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
    public override string GetUserInfo()
    {
        return $"{GetFullName()} ({Username}) - {Email} - Age: {GetAge()}";
    }
}
