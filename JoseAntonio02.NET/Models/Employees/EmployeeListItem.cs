namespace Net02.Api.Models.Employees;

public class EmployeeListItem
{
    public string FirstName { get; }

    public string LastName { get; }

    public EmployeeListItem(GetEmployee employee)
    {
        FirstName = employee.FirstName;
        LastName = employee.LastName;
    }

    public override bool Equals(object? obj)
    {
        return obj is EmployeeListItem other &&
               FirstName == other.FirstName &&
               LastName == other.LastName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }
}