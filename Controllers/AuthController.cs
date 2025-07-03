using JobFinder.API.Application.Commands;
using JobFinder.API.Application.Queries;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registration request received for: {Email}", request.Email);

            try
            {
                var result = await _mediator.Send(new RegisterUserCommand(request.Username, request.Email, request.Password));
                _logger.LogInformation("Registration successful for: {Email}", request.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for: {Email}", request.Email);
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login request received for: {Email}", request.Email);

            try
            {
                var result = await _mediator.Send(new LoginUserQuery(request.Email, request.Password));

                if (result == "Invalid email or password")
                {
                    _logger.LogWarning("Login failed for: {Email}", request.Email);
                    return Unauthorized("Invalid email or password");
                }

                _logger.LogInformation("Login successful for: {Email}", request.Email);
                return Ok(new { token = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for: {Email}", request.Email);
                return StatusCode(500, "An error occurred during login.");
            }
        }
    }
}
