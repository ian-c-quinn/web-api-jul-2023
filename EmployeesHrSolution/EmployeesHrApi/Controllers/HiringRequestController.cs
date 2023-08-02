using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeesHrApi.Data;
using EmployeesHrApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesHrApi.Controllers
{
    public class HiringRequestController : ControllerBase
    {
        private readonly EmployeeDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;


        public HiringRequestController(EmployeeDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = config;
        }

        [HttpPost("/denied-hiring-requests")]
        public async Task<ActionResult> DenyHiringRequestAsync([FromBody] HiringRequestResponseModel request)
        {
            var id = int.Parse(request.Id);
            if (request.Status != HiringRequestStatus.WaitingForJobAssignment)
            {
                return BadRequest("Can only deny pending assignments");
            }
            var savedHiringRequest = await _context.HiringRequests.Where(h => h.Id == id)
                .SingleOrDefaultAsync();



            if (savedHiringRequest == null)
            {
                return BadRequest();
            }
            else
            {
                savedHiringRequest.Status = HiringRequestStatus.Denied;
                await _context.SaveChangesAsync();
                return NoContent(); // or the mapped hiring request.
            }
        }

        [HttpPost("/hiring-requests")]
        public async Task<ActionResult> AddHiringRequestAsync([FromBody] HiringRequestCreateRequest request)
        {
            // 1. Validate it a little? - if it isn't valid, send them a 400 (Bad Request)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400
            }
            // 2. Save it to the database.
            var newHiringRequest = _mapper.Map<HiringRequests>(request);
            _context.HiringRequests.Add(newHiringRequest);
            await _context.SaveChangesAsync();
            // 3. Return a 201 Created Status Code 
            //   - Add Header "Location" - with the Url of the new resource.
            //   - Return them a copy of the new resource
            var response = _mapper.Map<HiringRequestResponseModel>(newHiringRequest);
            return CreatedAtRoute("hiring-request#gethiringrequestbyidasync", new { id = response.Id }, response);
        }

        [HttpGet("/hiring-requests/{id:int}", Name="hiring-request#gethiringrequestbyidasync")]
        public async Task<ActionResult> GetHiringRequestByIdAsync(int id)
        {
            var hiringRequest = await _context.HiringRequests
                .Where(e => e.Id == id)
                .ProjectTo<HiringRequestResponseModel>(_mapperConfig)
                .SingleOrDefaultAsync();

            if (hiringRequest is not null)
            {
                return Ok(hiringRequest);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
