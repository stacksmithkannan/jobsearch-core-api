using JobFinder.API.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class AdminController: ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("promote/{userId}")]
        public async Task<IActionResult> Promoteuser(int userId)
        {
            var result = await _mediator.Send(new PromoteUserCommand(userId));
            return Ok(result);
        }

    }
}
