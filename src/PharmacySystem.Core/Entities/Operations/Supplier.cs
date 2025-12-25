using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.ValueObjects;

namespace PharmacySystem.Core.Entities.Operations;

/// <summary>
/// Represents a supplier of medications
/// </summary>
public class Supplier : Entity
{
    /// <summary>
    /// Supplier name
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Contact person name
    /// </summary>
    public string? ContactPerson { get; private set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Physical address
    /// </summary>
    public Address? Address { get; private set; }

    /// <summary>
    /// Website URL
    /// </summary>
    public string? Website { get; private set; }

    /// <summary>
    /// Supplier rating (0-5)
    /// </summary>
    public decimal Rating { get; private set; }

    /// <summary>
    /// Whether supplier is currently active
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Navigation property for supplied inventory
    /// </summary>
    public virtual ICollection<Inventory> SuppliedInventory { get; private set; } = new List<Inventory>();

    /// <summary>
    /// Private constructor for Entity Framework Core
    /// </summary>
    private Supplier() : base()
    {
        IsActive = true;
        Rating = 3.0m; // Default rating
    }

    /// <summary>
    /// Constructor with name only (Constructor Overloading - 1)
    /// </summary>
    public Supplier(string name) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }

    /// <summary>
    /// Constructor with contact information (Constructor Overloading - 2)
    /// </summary>
    public Supplier(string name, string contactPerson, string email) : this(name)
    {
        ContactPerson = contactPerson;
        Email = email;
    }

    /// <summary>
    /// Constructor with full contact details (Constructor Overloading - 3)
    /// </summary>
    public Supplier(string name, string contactPerson, string email, 
                   string phoneNumber, Address address) : this(name, contactPerson, email)
    {
        PhoneNumber = phoneNumber;
        Address = address;
    }

    /// <summary>
    /// Updates supplier information
    /// </summary>
    public void UpdateSupplierInfo(string? contactPerson, string? email, 
                                   string? phoneNumber, Address? address)
    {
        if (contactPerson != null) ContactPerson = contactPerson;
        if (email != null) Email = email;
        if (phoneNumber != null) PhoneNumber = phoneNumber;
        if (address != null) Address = address;
        
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates website
    /// </summary>
    public void UpdateWebsite(string website)
    {
        Website = website;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates rating
    /// </summary>
    public void UpdateRating(decimal newRating)
    {
        if (newRating < 0 || newRating > 5)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(newRating));

        Rating = newRating;
        UpdateTimestamp();
    }

    /// <summary>
    /// Activates the supplier
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    /// <summary>
    /// Deactivates the supplier
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets supplier information
    /// </summary>
    public string GetSupplierInfo()
    {
        return $"Supplier: {Name}\n" +
               $"Contact: {ContactPerson ?? "N/A"}\n" +
               $"Email: {Email ?? "N/A"}\n" +
               $"Phone: {PhoneNumber ?? "N/A"}\n" +
               $"Address: {Address?.GetFormattedAddress() ?? "N/A"}\n" +
               $"Website: {Website ?? "N/A"}\n" +
               $"Rating: {Rating:F1}/5.0\n" +
               $"Status: {(IsActive ? "Active" : "Inactive")}\n" +
               $"Supplied Items: {SuppliedInventory.Count}";
    }
}
