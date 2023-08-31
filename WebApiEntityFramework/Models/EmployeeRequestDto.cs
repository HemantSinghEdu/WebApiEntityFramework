using System.ComponentModel.DataAnnotations;

namespace WebApiEntityFramework.Models
{
    public class EmployeeRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public static implicit operator EmployeeRequestDto(Employee employee)
        {
            return new EmployeeRequestDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                EmailAddress = employee.EmailAddress
            };
        }

        public static implicit operator Employee(EmployeeRequestDto employeeDto)
        {
            return new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Age = employeeDto.Age,
                EmailAddress = employeeDto.EmailAddress
            };
        }
    }
}
