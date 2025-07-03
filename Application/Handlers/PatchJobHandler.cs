using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class PatchJobHandler : IRequestHandler<PatchJobCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatchJobHandler> _logger;

        public PatchJobHandler(ApplicationDbContext context, ILogger<PatchJobHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(PatchJobCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to patch job with ID: {JobId}", request.Id);

            var job = await _context.Jobs.FindAsync(request.Id);
            if (job == null)
            {
                _logger.LogWarning("Job not found with ID: {JobId}", request.Id);
                return false;
            }

            bool isUpdated = false;

            if (request.Title is not null)
            {
                job.Title = request.Title;
                isUpdated = true;
            }
            if (request.Description is not null)
            {
                job.Description = request.Description;
                isUpdated = true;
            }
            if (request.Location is not null)
            {
                job.Location = request.Location;
                isUpdated = true;
            }
            if (request.Company is not null)
            {
                job.Company = request.Company;
                isUpdated = true;
            }
            if (request.Skills is not null)
            {
                job.Skills = request.Skills;
                isUpdated = true;
            }

            if (isUpdated)
            {
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully updated job with ID: {JobId}", request.Id);
            }
            else
            {
                _logger.LogInformation("No changes detected for job with ID: {JobId}", request.Id);
            }

            return true;
        }
    }

}
