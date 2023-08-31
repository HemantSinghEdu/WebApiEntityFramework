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
        [ProducesResponseType(typeof(EmployeeDto), 200)]
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
        [ProducesResponseType(typeof(EmployeeDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetEmployeeById(string employeeId)
        {
            var employee = _dbcontext.Employees.FirstOrDefault(a => a.EmployeeId.Equals(employeeId));
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
        [ProducesResponseType( 400)]
        [ProducesResponseType(500)]
        public IActionResult AddEmployee(EmployeeAddDto employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            employee.EmployeeId = Guid.NewGuid().ToString();
            _dbcontext.Employees.Add(employee);
            _dbcontext.SaveChanges();

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
        }


        /// <summary>
        /// Update an employee's details based on employee id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType( 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Updateemployee(string id, EmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest("The employee id in url does not match the employee id in body of request.");
            }

            var employeeToUpdate = _dbcontext.Employees.FirstOrDefault(a => a.EmployeeId.Equals(id));

            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.EmailAddress = employee.EmailAddress;
            employeeToUpdate.Age = employee.Age;
            
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