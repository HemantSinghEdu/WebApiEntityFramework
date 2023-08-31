using Swashbuckle.AspNetCore.Annotations;

namespace WebApiEntityFramework.Models
{
    public class Employee
    {
        [SwaggerSchema(ReadOnly = true)]
        public string? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public int Age { get; set; }
        public string EmailAddress { get; set; }

    }
}
