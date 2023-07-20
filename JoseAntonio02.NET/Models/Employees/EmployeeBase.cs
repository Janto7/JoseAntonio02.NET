using System.Net;

namespace JoseAntonio02.NET.Models.Employees
{
    public abstract class EmployeeBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Department { get; set; }

        public Address Address { get; set; }
    }
}