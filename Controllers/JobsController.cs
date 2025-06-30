using JobFinder.API.Application.Commands;
using JobFinder.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
        {
            var JobId = await _mediator.Send(command);
            return Ok(JobId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _mediator.Send(new GetAllJobsQuery());
            return Ok(jobs);
        }

    }
}
