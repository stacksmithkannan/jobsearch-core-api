using JobFinder.API.DTOs;
using MediatR;
using System.Collections.Generic;

namespace JobFinder.API.Application.Queries
{
    public class GetMyApplicationsQuery : IRequest<List<JobApplicationDto>> { }
  
}
