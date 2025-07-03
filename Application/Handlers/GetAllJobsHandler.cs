using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class GetAllJobsHandler: IRequestHandler<GetAllJobsQuery,IEnumerable<Job>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetAllJobsHandler> _logger;

        public GetAllJobsHandler(ApplicationDbContext context, ILogger<GetAllJobsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Job>> Handle(GetAllJobsQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllJobsQuery...");

            var jobs = await _context.Jobs.ToListAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} jobs from database.", jobs.Count);
            return jobs;
        }
    }
}
