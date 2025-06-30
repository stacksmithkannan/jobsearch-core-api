using JobFinder.API.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace JobFinder.API.Application.Queries
{
    public record GetAllJobsQuery : IRequest<IEnumerable<Job>>;
}
