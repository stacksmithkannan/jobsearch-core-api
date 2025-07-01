using MediatR;

namespace JobFinder.API.Application.Commands
{
        public record RegisterUserCommand(string UserName,string Email,string Password):IRequest<string>;
}
