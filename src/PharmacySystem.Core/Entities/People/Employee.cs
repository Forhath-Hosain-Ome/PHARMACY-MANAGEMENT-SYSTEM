using PharmacySystem.Core.Entities.ValueObjects;
using PharmacySystem.Core.Enums;

namespace PharmacySystem.Core.Entities.People;

public abstract class Employee : Person
{
    public string EmployeeId { get; protected set; } = string.Empty;
    public DateTime HireDate { get; protected set; }
    public decimal Salary { get; protected set; }
    public string? Department { get; protected set; }
    public bool IsEmployed { get; protected set; }
    protected Employee() : base()
    {
        IsEmployed = true;
    }

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

    public int CalculateYearsOfService()
    {
        var years = DateTime.Now.Year - HireDate.Year;
        
        if (HireDate.Date > DateTime.Now.AddYears(-years))
            years--;
        
        return years;
    }
    public virtual void UpdateSalary(decimal newSalary)
    {
        if (newSalary < 0)
            throw new ArgumentException("Salary cannot be negative", nameof(newSalary));

        Salary = newSalary;
        UpdateTimestamp();
    }

    public virtual void UpdateDepartment(string department)
    {
        Department = department;
        UpdateTimestamp();
    }

    public virtual void Terminate()
    {
        IsEmployed = false;
        Deactivate();
        UpdateTimestamp();
    }

    public virtual void Reinstate()
    {
        IsEmployed = true;
        Activate();
        UpdateTimestamp();
    }

    public virtual string GetEmployeeInfo()
    {
        return $"{GetFullName()} - Employee ID: {EmployeeId}\n" +
               $"Department: {Department ?? "Not assigned"}\n" +
               $"Hire Date: {HireDate:yyyy-MM-dd}\n" +
               $"Years of Service: {CalculateYearsOfService()}\n" +
               $"Status: {(IsEmployed ? "Active" : "Terminated")}";
    }

    public override string GetUserInfo()
    {
        return $"{GetFullName()} ({EmployeeId}) - {Role} - {Department ?? "Unassigned"}";
    }
}
