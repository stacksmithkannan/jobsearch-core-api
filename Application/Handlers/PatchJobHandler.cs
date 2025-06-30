using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;

namespace JobFinder.API.Application.Handlers
{
    public class PatchJobHandler :IRequestHandler<PatchJobCommand,bool>
    {

        public readonly ApplicationDbContext _context;
        public PatchJobHandler(ApplicationDbContext context) 
        { 
            _context = context; 
        }
        public async Task<bool> Handle(PatchJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.FindAsync(request.Id);
            if(job == null)  return false;

            if(request.Title is not null) job.Title = request.Title;
            if (request.Description is not null) job.Description = request.Description;
            if (request.Location is not null) job.Location = request.Location;
            if (request.Company is not null) job.Company = request.Company;
            if (request.Skills is not null) job.Skills = request.Skills;

            await _context.SaveChangesAsync();
            return true;

        }
    }
}
