using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;

namespace JobFinder.API.Application.Handlers
{
    public class CreateJobHandler : IRequestHandler<CreateJobCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateJobHandler> _logger;

        public CreateJobHandler(ApplicationDbContext context, ILogger<CreateJobHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var Job = new Job
            {
                Title = request.Title,
                Description = request.Description,
                Company = request.Company,
                Location = request.Location,
                Skills = request.Skills,
                PostedDate = DateTime.UtcNow
            };
            _context.Jobs.Add(Job);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Job created with ID: {JobId}", Job.Id);
            return Job.Id;
        }
    }
}
