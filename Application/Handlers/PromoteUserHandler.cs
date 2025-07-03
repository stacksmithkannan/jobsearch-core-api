using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class PromoteUserHandler : IRequestHandler<PromoteUserCommand, string>
    {
        public readonly ApplicationDbContext _context;
        public readonly ILogger<PromoteUserHandler> _logger;

        public PromoteUserHandler(ApplicationDbContext context, ILogger<PromoteUserHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> Handle(PromoteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Promotion request received for User ID: {UserId}", request.userId);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found for promotion with ID: {UserId}", request.userId);
                return "User not found";
            }

            user.Role = "Admin";
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User ID: {UserId} successfully promoted to Admin", request.userId);
            return "User promoted to Admin";
        }
    }
}
