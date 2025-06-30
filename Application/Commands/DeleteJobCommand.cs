using MediatR;
namespace JobFinder.API.Application.Commands
{
  public record DeleteJobCommand (int id):IRequest<bool>;
}
