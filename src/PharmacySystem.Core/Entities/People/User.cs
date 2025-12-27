using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.People;

public abstract class User : Entity
{
    public string Username { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
    public string PasswordHash { get; protected set; } = string.Empty;
    public UserRole Role { get; protected set; }
    public bool IsActive { get; protected set; }
    protected User()
    {
        IsActive = true;
    }
    protected User(string username, string email, UserRole role) : this()
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Username = username;
        Email = email;
        Role = role;
    }
    protected User(string username, string email, string passwordHash, UserRole role) 
        : this(username, email, role)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        PasswordHash = passwordHash;
    }
    public virtual bool ValidateCredentials(string password)
    {
        // In a real system, this would use proper password hashing (BCrypt, etc.)
        // For demo purposes, we're doing a simple comparison
        return PasswordHash == HashPassword(password);
    }
    public virtual void UpdatePassword(string oldPassword, string newPassword)
    {
        if (!ValidateCredentials(oldPassword))
            throw new InvalidOperationException("Invalid old password");

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password cannot be empty", nameof(newPassword));

        PasswordHash = HashPassword(newPassword);
        UpdateTimestamp();
    }
    public virtual void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }
    public virtual void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }
    public virtual void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be empty", nameof(newEmail));

        Email = newEmail;
        UpdateTimestamp();
    }
    protected static string HashPassword(string password)
    {
        // This is NOT secure - use BCrypt or similar in production
        return Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(password + "SALT")
        );
    }
    public virtual string GetUserInfo()
    {
        return $"{Username} ({Email}) - {Role}";
    }
}
