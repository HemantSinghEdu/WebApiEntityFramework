using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace WebApiEntityFramework.Models
{
    public class Employee
    {
        public string? EmployeeId { get; set; }

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

    }
}
