using JobFinder.API.DTOs;
using MediatR;

namespace JobFinder.API.Application.Queries
{
    public class SearchJobsQuery : IRequest<PaginatedJobListDto>
    {
        public string? KeyWord { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}