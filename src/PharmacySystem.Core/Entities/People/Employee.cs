using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.People;

/// <summary>
/// Abstract class representing an employee in the pharmacy
/// Inherits from Person
/// </summary>
public abstract class Employee : Person
{
    /// <summary>
    /// Unique employee identifier
    /// </summary>
    public string EmployeeId { get; protected set; } = string.Empty;

    /// <summary>
    /// Date when employee was hired
    /// </summary>
    public DateTime HireDate { get; protected set; }

    /// <summary>
    /// Employee's salary
    /// </summary>
    public decimal Salary { get; protected set; }

    /// <summary>
    /// Department where employee works
    /// </summary>
    public string? Department { get; protected set; }

    /// <summary>
    /// Whether employee is currently employed
    /// </summary>
    public bool IsEmployed { get; protected set; }

    /// <summary>
    /// Protected constructor for Entity Framework Core
    /// </summary>
    protected Employee() : base()
    {
        IsEmployed = true;
    }

    /// <summary>
    /// Protected constructor with basic employee information (Constructor Overloading - 1)
    /// </summary>
    protected Employee(string username, string email, UserRole role,
                      string firstName, string lastName, DateTime dateOfBirth,
                      string employeeId, DateTime hireDate, decimal salary)
        : base(username, email, role, firstName, lastName, dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative", nameof(salary));

        EmployeeId = employeeId;
        HireDate = hireDate;
        Salary = salary;
        IsEmployed = true;
    }

    /// <summary>
    /// Protected constructor with department (Constructor Overloading - 2)
    /// </summary>
    protected Employee(string username, string email, UserRole role,
                      string firstName, string lastName, DateTime dateOfBirth,
                      string phoneNumber, string employeeId, DateTime hireDate, 
                      decimal salary, string department)
        : base(username, email, role, firstName, lastName, dateOfBirth, phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative", nameof(salary));

        EmployeeId = employeeId;
        HireDate = hireDate;
        Salary = salary;
        Department = department;
        IsEmployed = true;
    }

    /// <summary>
    /// Calculates years of service
    /// </summary>
    public int CalculateYearsOfService()
    {
        var years = DateTime.Now.Year - HireDate.Year;
        
        if (HireDate.Date > DateTime.Now.AddYears(-years))
            years--;
        
        return years;
    }

    /// <summary>
    /// Updates salary
    /// </summary>
    public virtual void UpdateSalary(decimal newSalary)
    {
        if (newSalary < 0)
            throw new ArgumentException("Salary cannot be negative", nameof(newSalary));

        Salary = newSalary;
        UpdateTimestamp();
    }

    /// <summary>
    /// Updates department
    /// </summary>
    public virtual void UpdateDepartment(string department)
    {
        Department = department;
        UpdateTimestamp();
    }

    /// <summary>
    /// Terminates employment
    /// </summary>
    public virtual void Terminate()
    {
        IsEmployed = false;
        Deactivate();
        UpdateTimestamp();
    }

    /// <summary>
    /// Reinstates employment
    /// </summary>
    public virtual void Reinstate()
    {
        IsEmployed = true;
        Activate();
        UpdateTimestamp();
    }

    /// <summary>
    /// Gets employee information
    /// </summary>
    public virtual string GetEmployeeInfo()
    {
        return $"{GetFullName()} - Employee ID: {EmployeeId}\n" +
               $"Department: {Department ?? "Not assigned"}\n" +
               $"Hire Date: {HireDate:yyyy-MM-dd}\n" +
               $"Years of Service: {CalculateYearsOfService()}\n" +
               $"Status: {(IsEmployed ? "Active" : "Terminated")}";
    }

    /// <summary>
    /// Gets basic employee info
    /// </summary>
    public override string GetUserInfo()
    {
        return $"{GetFullName()} ({EmployeeId}) - {Role} - {Department ?? "Unassigned"}";
    }
}
