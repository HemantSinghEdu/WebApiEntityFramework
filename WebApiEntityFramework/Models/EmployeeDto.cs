using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace WebApiEntityFramework.Models
{
    public class EmployeeDto : EmployeeBaseDto
    {
        public string EmployeeId { get; set; }

        public static implicit operator EmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                EmailAddress = employee.EmailAddress
            };
        }

        public static implicit operator Employee(EmployeeDto employeeDto)
        {
            return new Employee
            {
                EmployeeId = employeeDto.EmployeeId,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Age = employeeDto.Age,
                EmailAddress = employeeDto.EmailAddress
            };
        }

    }
}
