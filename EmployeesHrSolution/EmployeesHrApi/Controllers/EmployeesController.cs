using EmployeesHrApi.Data;
using EmployeesHrApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesHrApi.Controllers
{
    public class EmployeesController : ControllerBase
    {

        private readonly EmployeeDataContext _context;

        public EmployeesController(EmployeeDataContext context)
        {
            _context = context;
        }

        [HttpGet("/employees")]
        public async Task<ActionResult> GetEmployeesAsync()
        {
            var employees = await _context.Employees.Select(emp => new EmployeesSummaryResponseModel
            {
              Id = emp.Id.ToString(),
              FirstName = emp.FirstName,
              LastName = emp.LastName,
              Department = emp.Department,
              Email = emp.Email,
            }).ToListAsync();


            return Ok(employees);
        }
    }
}
