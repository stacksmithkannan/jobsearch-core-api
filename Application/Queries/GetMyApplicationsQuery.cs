using JobFinder.API.DTOs;
using MediatR;
using System.Collections.Generic;

namespace JobFinder.API.Application.Queries
{
    public class GetMyApplicationsQuery : IRequest<PaginatedUserApplicationDto>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}
