using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class UpdateJobHandler:IRequestHandler<UpdateJobCommand,bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateJobHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.FindAsync(request.Id);
            if (job == null) return false;

            job.Title = request.Title;
            job.Description = request.Description;
            job.Location = request.Location;
            job.Company = request.Company;
            job.Skills = request.Skills;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
