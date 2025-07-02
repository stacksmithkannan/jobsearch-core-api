using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class PromoteUserHandler : IRequestHandler<PromoteUserCommand, string>
    {
        public readonly ApplicationDbContext _context;

        public PromoteUserHandler(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<string> Handle(PromoteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userId,cancellationToken);
            if (user == null)
                return "User not found";

            user.Role = "Admin";
            await _context.SaveChangesAsync(cancellationToken);

            return "User promoted to Admin";
        }
    }
}
