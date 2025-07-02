using MediatR;

namespace JobFinder.API.Application.Commands
{
    public record PromoteUserCommand(int userId):IRequest<string>;
}
