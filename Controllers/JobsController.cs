using JobFinder.API.Application.Commands;
using JobFinder.API.Application.Handlers;
using JobFinder.API.Application.Queries;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JobsController> _logger;


        public JobsController(IMediator mediator, ILogger<JobsController> logger) 
        { 
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
        {
            var jobId = await _mediator.Send(command);
            _logger.LogInformation("New job created with ID: {JobId}", jobId);
            return Ok(jobId);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllJobs()
        {
            _logger.LogInformation("GET request received: Fetching all jobs");

            var jobs = await _mediator.Send(new GetAllJobsQuery());

            _logger.LogInformation("Returning {Count} jobs", jobs.Count());

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(int id)
        {
            _logger.LogInformation("Received request to get job with ID: {JobId}", id);

            var job = await _mediator.Send(new GetJobByIdQuery(id));
            if (job == null)
            {
                _logger.LogWarning("Job with ID {JobId} not found", id);
                return NotFound($"Job with ID {id} not found.");
            }

            _logger.LogInformation("Job with ID {JobId} successfully retrieved", id);
            return Ok(job);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobCommand command)
        {
            _logger.LogInformation("Received update request for job ID: {JobId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("ID mismatch: URL ID ({UrlId}) does not match body ID ({BodyId})", id, command.Id);
                return BadRequest("ID in URL and body do not match");
            }

            var result = await _mediator.Send(command);
            if (!result)
            {
                _logger.LogWarning("Job not found for update: ID = {JobId}", id);
                return NotFound($"Job with ID {id} not found.");
            }

            _logger.LogInformation("Job updated successfully: ID = {JobId}", id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchJob(int id, [FromBody] PatchJobCommand command)
        {
            _logger.LogInformation("PATCH request received for Job ID: {JobId}", id);

            if (id != command.Id)
            {
                _logger.LogWarning("Job ID mismatch. URL ID: {UrlId}, Body ID: {BodyId}", id, command.Id);
                return BadRequest("ID in URL and body do not match");
            }

            var result = await _mediator.Send(command);
            if (!result)
            {
                _logger.LogWarning("Job not found or no changes made for ID: {JobId}", id);
                return NotFound($"Job with ID {id} not found.");
            }

            _logger.LogInformation("Job with ID: {JobId} successfully patched", id);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            _logger.LogInformation("Admin requested to delete job with ID: {JobId}", id);

            var result = await _mediator.Send(new DeleteJobCommand(id));
            if (!result)
            {
                _logger.LogWarning("Delete failed. Job with ID {JobId} not found.", id);
                return NotFound($"Job with ID {id} not found.");
            }

            _logger.LogInformation("Job with ID {JobId} successfully deleted.", id);

            return NoContent();
        }

        [HttpPost("{jobId}/apply")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> ApplyToJob(int jobId)
        {
            _logger.LogInformation("user is attempting to apply to job ID {JobId}", jobId);

            var result = await _mediator.Send(new ApplyToJobCommand(jobId));

            if(result == "Applied successfully")
                return Ok(new {message = result});

            _logger.LogWarning("Application failed: {Reason}", result);
            return BadRequest(result);
        }

        [HttpGet("{jobId}/applications")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetApplicationsForJob(int jobId)
        {
            _logger.LogInformation("Admin requested applications for job ID: {JobId}", jobId);
            var applications = await _mediator.Send( new GetApplicationsForJobQuery(jobId));
            return Ok(applications);
        }

        [HttpGet("my-applications")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> GetMyApplications([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetMyApplicationsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(result);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchJobs([FromQuery] string? keyWord, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new SearchJobsQuery
            {
                KeyWord = keyWord,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var results = await _mediator.Send( query );
            return Ok(results);
        }

        [HttpPatch("{jobApplicationId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateApplicationStatus(
                                            [FromRoute] int jobApplicationId,
                                            [FromBody] UpdateApplicationStatusDto dto)
        {
            var command = new UpdateApplicationStatusCommand
            {
                JobApplicationId = jobApplicationId,
                Status = dto.Status
            };

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound($"Application with ID {jobApplicationId} not found.");

            return Ok($"Status updated to {dto.Status}.");
        }


    }
}
