using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class RegisterUserHandler:IRequestHandler<RegisterUserCommand, string>
    {
        private readonly ApplicationDbContext _context;
        public RegisterUserHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(RegisterUserCommand request,CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                return "Email already exists";

            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken))
                return "Username already taken";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = hashedPassword,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return "Registration successfull";

        }
    }
}
