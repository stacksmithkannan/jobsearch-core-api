using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class RegisterUserHandler:IRequestHandler<RegisterUserCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisterUserHandler> _logger;
        public RegisterUserHandler(ApplicationDbContext context, ILogger<RegisterUserHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> Handle(RegisterUserCommand request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering user: {Email}", request.Email);

            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                {
                    _logger.LogWarning("Email already exists: {Email}", request.Email);
                    return "Email already exists";
                }

                if (await _context.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken))
                {
                    _logger.LogWarning("Username already taken: {Username}", request.UserName);
                    return "Username already taken";
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = hashedPassword,
                    Role = "User"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Registration successful for: {Email}", request.Email);
                return "Registration successful";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for: {Email}", request.Email);
                return "An error occurred while registering the user.";
            }

        }
    }
}
