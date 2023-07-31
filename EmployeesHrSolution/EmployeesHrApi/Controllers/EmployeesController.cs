using Microsoft.AspNetCore.Mvc;

namespace EmployeesHrApi.Controllers
{
    public class EmployeesController : ControllerBase
    {
        [HttpGet("/employees")]
        public async Task<ActionResult> GetEmployeesAsync()
        {
            return Ok();
        }
    }
}
