using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using WebApiEntityFramework.DatabaseContext;
using WebApiEntityFramework.Models;

namespace WebApiEntityFramework.Controllers
{
    /*****************
     * TODO - 
     * 1. put ef operations behind an interface
     * 2. Add a few employees at startup
     * 3. Add logs
     * 2. Ensure that no error escapes web api, so add a global error handler
     * 3. Provide a generic message for each error scenario
     * 4. Add unit tests
     * 5. Ensure that unique key constraints are enforced
     */
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly InMemoryDbContext _dbcontext;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger, InMemoryDbContext dbContext)
        {
            _logger = logger;
            _dbcontext = dbContext;
        }

        /// <summary>
        /// Returns a list of all employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(EmployeeResponseDto), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetEmployees()
        {
            return Ok(_dbcontext.Employees.ToList());
        }


        /// <summary>
        /// Returns details of a single employee based on input employeeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetEmployeeById(string id)
        {
            var employee = _dbcontext.Employees.FirstOrDefault(a => a.EmployeeId.Equals(id));
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult AddEmployee(EmployeeRequestDto employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            Employee employee = employeeRequest;
            employee.EmployeeId = Guid.NewGuid().ToString();
            _dbcontext.Employees.Add(employee);
            _dbcontext.SaveChanges();

            EmployeeResponseDto employeeResponse = employee;

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeResponse.EmployeeId }, employeeResponse);
        }


        /// <summary>
        /// Update an employee's details based on employee id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Updateemployee(string id, EmployeeRequestDto employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            var employeeToUpdate = _dbcontext.Employees.FirstOrDefault(a => a.EmployeeId.Equals(id));

            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            employeeToUpdate.FirstName = employeeRequest.FirstName;
            employeeToUpdate.LastName = employeeRequest.LastName;
            employeeToUpdate.Age = employeeRequest.Age;
            employeeToUpdate.EmailAddress = employeeRequest.EmailAddress;

            _dbcontext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete an employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Deleteemployee(string id)
        {
            var employeeToDelete = _dbcontext.Employees.FirstOrDefault(a => a.EmployeeId.Equals(id));

            if (employeeToDelete == null)
            {
                return NotFound();
            }

            _dbcontext.Employees.Remove(employeeToDelete);
            _dbcontext.SaveChanges();

            return NoContent();
        }

        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errMsgs);
        }
    }
}