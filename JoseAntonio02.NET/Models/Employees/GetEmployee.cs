using JoseAntonio02.NET.Models.Employees;

namespace Net02.Api.Models.Employees;

public class GetEmployee : EmployeeBase
{
    public string Id { get; set; }

    public DateTime HiredDate { get; set; }

    public IList<EmployeeHistory> History { get; set; }

    public GetEmployee()
    {
        History = new List<EmployeeHistory>();
    }
}