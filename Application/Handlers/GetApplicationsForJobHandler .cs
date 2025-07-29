using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class GetApplicationsForJobHandler : IRequestHandler<GetApplicationsForJobQuery, List<JobApplicationDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetApplicationsForJobHandler> _logger;

        public GetApplicationsForJobHandler(ApplicationDbContext context, ILogger<GetApplicationsForJobHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<JobApplicationDto>> Handle(GetApplicationsForJobQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching applications for job ID: {JobId}", request.JobId);

            var applications = await _context.JobApplications
                .Include(a => a.User)
                .Where(a => a.Jobid == request.JobId)
                .Select(a => new  JobApplicationDto
                {
                    UserName = a.User.UserName,
                    Email = a.User.Email,
                    Resumepath = a.ResumePath,
                    AppliedOn = a.AppliedOn,
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("{Count} applications found for job ID: {JobId}", applications.Count, request.JobId);
            return applications;
        }
    }
}
