using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace WebApiEntityFramework.Models
{
    public class EmployeeAddDto : EmployeeBaseDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public string? EmployeeId { get; set; }

        public static implicit operator EmployeeAddDto(Employee employee)
        {
            return new EmployeeAddDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                EmailAddress = employee.EmailAddress
            };
        }

        public static implicit operator Employee(EmployeeAddDto employeeDto)
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
