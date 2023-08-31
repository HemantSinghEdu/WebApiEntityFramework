using Microsoft.AspNetCore.Mvc;
using WebApiEntityFramework.Models;

namespace WebApiEntityFramework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> _employees = new List<Employee>();

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns a list of all employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_employees);
        }


        /// <summary>
        /// Returns details of a single employee based on input employeeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetEmployee(string employeeId)
        {
            var employee = _employees.FirstOrDefault(a => a.EmployeeId.Equals(employeeId));
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        /// <summary>
        /// Adds a new employee to employee table
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            employee.EmployeeId = Guid.NewGuid().ToString();
            _employees.Add(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }


        /// <summary>
        /// Update an employee's details based on employee id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Updateemployee(string id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest("The employee id in url does not match the employee id in body of request.");
            }

            var employeeToUpdate = _employees.FirstOrDefault(a => a.EmployeeId.Equals(id));

            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.EmailAddress = employee.EmailAddress;
            employeeToUpdate.Age = employee.Age;

            return NoContent();
        }

        /// <summary>
        /// Delete an employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Deleteemployee(string id)
        {
            var employeeToDelete = _employees.FirstOrDefault(a => a.EmployeeId.Equals(id));

            if (employeeToDelete == null)
            {
                return NotFound();
            }

            _employees.Remove(employeeToDelete);

            return NoContent();
        }

        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errMsgs);
        }
    }
}