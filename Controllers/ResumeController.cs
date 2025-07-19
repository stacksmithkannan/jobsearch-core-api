using JobFinder.API.Application.Commands;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(IMediator mediator, ILogger<ResumeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadDto dto)
        {
            _logger.LogInformation("Upload endpoint hit for resume.");

            var result = await _mediator.Send(new UploadResumeCommand(dto.File));

            if (result.StartsWith("/resumes"))
            {
                _logger.LogInformation("Resume uploaded successfully. Path: {Path}", result);
                return Ok(new { Message = "Resume uploaded successfully", Path = result });
            }
            else
            {
                _logger.LogWarning("Resume upload failed. Reason: {Reason}", result);
                return BadRequest(result);
            }
        }
    }
}
