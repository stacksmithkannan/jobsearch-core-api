using JobFinder.API.Domain.Entities;
using JobFinder.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using JobFinder.API.Application.Queries;

namespace JobFinder.API.Application.Handlers
{
    public class GetJobByIdHandler:IRequestHandler<GetJobByIdQuery,Job?>
    {

        private readonly ApplicationDbContext _context;

        public GetJobByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Job?> Handle(GetJobByIdQuery request,CancellationToken cancellationToken)
        {
            return await _context.Jobs.FindAsync(request.Id);
        }
    }
}
