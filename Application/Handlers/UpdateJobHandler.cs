using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class UpdateJobHandler:IRequestHandler<UpdateJobCommand,bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UpdateJobHandler> _logger;

        public UpdateJobHandler(ApplicationDbContext context, ILogger<UpdateJobHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update request received for job ID: {JobId}", request.Id);

            var job = await _context.Jobs.FindAsync(request.Id);
            if (job == null)
            {
                _logger.LogWarning("Job not found with ID: {JobId}", request.Id);
                return false;
            }

            job.Title = request.Title;
            job.Description = request.Description;
            job.Location = request.Location;
            job.Company = request.Company;
            job.Skills = request.Skills;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Job updated successfully for ID: {JobId}", request.Id);
            return true;
        }

    }
}
