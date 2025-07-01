using MediatR;

namespace JobFinder.API.Application.Queries
{
   public record LoginUserQuery(string Email,string Password):IRequest<string>;
}
