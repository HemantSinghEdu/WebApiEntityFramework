using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using WebApiEntityFramework.Contracts.Repositories;
using WebApiEntityFramework.DatabaseContext;
using WebApiEntityFramework.Implementations.Repositories;
using WebApiEntityFramework.Models;

namespace WebApiEntityFramework.Controllers
{
    /*****************
     * TODO - 
     * 4. Add unit tests
     * 5. Ensure that unique key constraints are enforced
     */
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Returns a list of all employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(EmployeeResponseDto), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return Ok(employees);
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
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                _logger.LogInformation("Unable to find Employee with requested employee id", id);
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
        public async Task<IActionResult> AddEmployee(EmployeeRequestDto employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            Employee employee = employeeRequest;
            employee.EmployeeId = Guid.NewGuid().ToString();
            await _employeeRepository.CreateAsync(employee);

            EmployeeResponseDto employeeResponse = employee;
            _logger.LogInformation("New Employee added with employee id", employee.EmployeeId);
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
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeRequestDto employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            var employeeToUpdate = await _employeeRepository.GetById(id);

            if (employeeToUpdate == null)
            {
                _logger.LogInformation("UpdateEmployee: Unable to find Employee with employee id", id);
                return NotFound();
            }

            employeeToUpdate.FirstName = employeeRequest.FirstName;
            employeeToUpdate.LastName = employeeRequest.LastName;
            employeeToUpdate.Age = employeeRequest.Age;
            employeeToUpdate.EmailAddress = employeeRequest.EmailAddress;

            await _employeeRepository.UpdateAsync(employeeToUpdate);

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
        public async Task<IActionResult> Deleteemployee(string id)
        {
            var employeeToDelete = await _employeeRepository.GetById(id);

            if (employeeToDelete == null)
            {
                _logger.LogInformation("DeleteEmployee: Unable to find Employee with employee id", id);
                return NotFound();
            }

            _employeeRepository.DeleteAsync(employeeToDelete);

            return NoContent();
        }

        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            _logger.LogInformation("Bad request received", errMsgs);
            return BadRequest(errMsgs);
        }
    }
}