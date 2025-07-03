using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class DeleteJobHandler :IRequestHandler<DeleteJobCommand, bool>
    {
        public readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteJobHandler> _logger;

        public DeleteJobHandler(ApplicationDbContext context, ILogger<DeleteJobHandler> logger) 
        { 
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteJobCommand request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete Job with ID: {JobId}", request.id);

            var job = await _context.Jobs.FindAsync(request.id);
            if (job == null)
            {
                _logger.LogWarning("Job with ID: {JobId} not found.", request.id);
                return false;
            }


            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted Job with ID: {JobId}", request.id);
            return true;
        }
    }
}
