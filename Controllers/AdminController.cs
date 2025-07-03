using JobFinder.API.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class AdminController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AdminController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPut("promote/{userId}")]
        public async Task<IActionResult> PromoteUser(int userId)
        {
            _logger.LogInformation("Received request to promote user with ID: {UserId}", userId);

            var result = await _mediator.Send(new PromoteUserCommand(userId));

            _logger.LogInformation("Promotion result for user ID {UserId}: {Result}", userId, result);

            return Ok(result);
        }

    }
}
