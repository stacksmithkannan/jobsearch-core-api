using JobFinder.API.Domain.Entities;
using MediatR;

namespace JobFinder.API.Application.Queries
{
  public record GetJobByIdQuery(int Id) : IRequest<Job?>;
}
