using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using MediatR;

namespace JobFinder.API.Application.Handlers
{
    public class DeleteJobHandler :IRequestHandler<DeleteJobCommand, bool>
    {
        public readonly ApplicationDbContext _context;
        public DeleteJobHandler(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<bool> Handle(DeleteJobCommand request,CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.FindAsync(request.id);
            if (job == null)  return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
