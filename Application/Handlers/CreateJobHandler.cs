using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;

namespace JobFinder.API.Application.Handlers
{
    public class CreateJobHandler : IRequestHandler<CreateJobCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateJobHandler(ApplicationDbContext context)
        {
            _context = context;
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
            return Job.Id;
        }
    }
}
