using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Entities.ValueObjects;

namespace PharmacySystem.Core.Entities.Operations;

public class Supplier : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string? ContactPerson { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Address? Address { get; private set; }
    public string? Website { get; private set; }
    public decimal Rating { get; private set; }
    public bool IsActive { get; private set; }
    public virtual ICollection<Inventory> SuppliedInventory { get; private set; } = new List<Inventory>();
    private Supplier() : base()
    {
        IsActive = true;
        Rating = 3.0m; // Default rating
    }

    public Supplier(string name) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }

    public Supplier(string name, string contactPerson, string email) : this(name)
    {
        ContactPerson = contactPerson;
        Email = email;
    }

    public Supplier(string name, string contactPerson, string email, 
                   string phoneNumber, Address address) : this(name, contactPerson, email)
    {
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public void UpdateSupplierInfo(string? contactPerson, string? email, 
                                   string? phoneNumber, Address? address)
    {
        if (contactPerson != null) ContactPerson = contactPerson;
        if (email != null) Email = email;
        if (phoneNumber != null) PhoneNumber = phoneNumber;
        if (address != null) Address = address;
        
        UpdateTimestamp();
    }

    public void UpdateWebsite(string website)
    {
        Website = website;
        UpdateTimestamp();
    }

    public void UpdateRating(decimal newRating)
    {
        if (newRating < 0 || newRating > 5)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(newRating));

        Rating = newRating;
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }


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
