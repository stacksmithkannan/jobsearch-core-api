using JobFinder.API.Domain.Entities;
using JobFinder.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using JobFinder.API.Application.Queries;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class GetJobByIdHandler:IRequestHandler<GetJobByIdQuery,Job?>
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetJobByIdHandler> _logger;

        public GetJobByIdHandler(ApplicationDbContext context, ILogger<GetJobByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Job?> Handle(GetJobByIdQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching job with ID: {Id}", request.Id);

            var job = await _context.Jobs.FindAsync(request.Id);
            if (job == null)
            {
                _logger.LogWarning("Job with ID {Id} not found", request.Id);
            }
            else
            {
                _logger.LogInformation("Job with ID {Id} found", request.Id);
            }

            return job;
        }
    }
}
