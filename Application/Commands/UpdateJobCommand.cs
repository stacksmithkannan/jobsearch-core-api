using MediatR;

namespace JobFinder.API.Application.Commands
{
   public record UpdateJobCommand(
       int Id,
       string Title,
       string Description,
       string Location,
       string Company,
       List<string> Skills
       ):IRequest<bool>;
}
