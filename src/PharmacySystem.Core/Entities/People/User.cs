using PharmacySystem.Core.Entities.Base;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.People;

/// <summary>
/// Abstract base class for all users in the system
/// Inherits from Entity and provides common user properties
/// </summary>
public abstract class User : Entity
{
    /// <summary>
    /// Unique username for login
    /// </summary>
    public string Username { get; protected set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; protected set; } = string.Empty;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; protected set; } = string.Empty;

    /// <summary>
    /// User role in the system
    /// </summary>
    public UserRole Role { get; protected set; }

    /// <summary>
    /// Whether the user account is active
    /// </summary>
    public bool IsActive { get; protected set; }

    /// <summary>
    /// Protected constructor for Entity Framework Core
    /// </summary>
    protected User()
    {
        IsActive = true;
    }

    /// <summary>
    /// Protected constructor with basic user information (Constructor Overloading)
    /// </summary>
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

    /// <summary>
    /// Protected constructor with password (Constructor Overloading)
    /// </summary>
    protected User(string username, string email, string passwordHash, UserRole role) 
        : this(username, email, role)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Validates user credentials
    /// </summary>
    public virtual bool ValidateCredentials(string password)
    {
        // In a real system, this would use proper password hashing (BCrypt, etc.)
        // For demo purposes, we're doing a simple comparison
        return PasswordHash == HashPassword(password);
    }

    /// <summary>
    /// Updates the user's password
    /// </summary>
    public virtual void UpdatePassword(string oldPassword, string newPassword)
    {
        if (!ValidateCredentials(oldPassword))
            throw new InvalidOperationException("Invalid old password");

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password cannot be empty", nameof(newPassword));

        PasswordHash = HashPassword(newPassword);
        UpdateTimestamp();
    }

    /// <summary>
    /// Activates the user account
    /// </summary>
    public virtual void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    /// <summary>
    /// Deactivates the user account
    /// </summary>
    public virtual void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates user email
    /// </summary>
    public virtual void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be empty", nameof(newEmail));

        Email = newEmail;
        UpdateTimestamp();
    }

    /// <summary>
    /// Simple password hashing (for demo purposes only)
    /// In production, use BCrypt or similar
    /// </summary>
    protected static string HashPassword(string password)
    {
        // This is NOT secure - use BCrypt or similar in production
        return Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(password + "SALT")
        );
    }

    /// <summary>
    /// Gets basic user information
    /// </summary>
    public virtual string GetUserInfo()
    {
        return $"{Username} ({Email}) - {Role}";
    }
}
