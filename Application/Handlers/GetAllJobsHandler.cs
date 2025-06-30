using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class GetAllJobsHandler: IRequestHandler<GetAllJobsQuery,IEnumerable<Job>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllJobsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Job>> Handle(GetAllJobsQuery request,CancellationToken cancellationToken)
        {
            return await _context.Jobs.ToListAsync();
        }
    }
}
