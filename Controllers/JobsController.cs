using JobFinder.API.Application.Commands;
using JobFinder.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
        {
            var JobId = await _mediator.Send(command);
            return Ok(JobId);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _mediator.Send(new GetAllJobsQuery());
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _mediator.Send(new GetJobByIdQuery(id));
            if(job == null)
                return NotFound($"Job with ID {id} not found.");
            return Ok(job);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL and body do not match");

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound($"Job with ID {id} not found.");

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> PatchJob(int id, [FromBody]  PatchJobCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL and body do not match");

            var result =await _mediator.Send(command);
            if (!result)

                return NotFound($"Job with ID {id} not found.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var result = await _mediator.Send(new DeleteJobCommand(id));
            if (!result)
                return NotFound($"Job with ID {id} not found.");

            return NoContent();
        }
    }
}
