using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class UpdateApplicationStatusHandler : IRequestHandler<UpdateApplicationStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UpdateApplicationStatusHandler> _logger;

        public UpdateApplicationStatusHandler(ApplicationDbContext context, ILogger<UpdateApplicationStatusHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var application = await _context.JobApplications
                    .FirstOrDefaultAsync(a => a.Id == request.JobApplicationId, cancellationToken);

                if (application == null)
                {
                    _logger.LogWarning("Application with ID {Id} not found.", request.JobApplicationId);
                    return false;
                }

                if (!Enum.TryParse<ApplicationStatus>(request.Status, true, out var parsedStatus))
                {
                    _logger.LogWarning("Invalid status '{Status}' provided.", request.Status);
                    return false;
                }

                if (application.Status == parsedStatus)
                {
                    _logger.LogInformation("Status is already '{Status}' for application {Id}. No update needed.",
                        parsedStatus, request.JobApplicationId);
                    return true;
                }

                application.Status = parsedStatus;
                var rows = await _context.SaveChangesAsync(cancellationToken);

                if (rows > 0)
                {
                    _logger.LogInformation("Application {Id} status updated to {Status}.", request.JobApplicationId, parsedStatus);
                    return true;
                }
                else
                {
                    _logger.LogWarning("No rows were updated in DB. Possible EF tracking issue.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating application status.");
                return false;
            }
        }
    }
}
