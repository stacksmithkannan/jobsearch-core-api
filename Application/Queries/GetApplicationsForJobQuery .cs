using JobFinder.API.DTOs;
using MediatR;

namespace JobFinder.API.Application.Queries
{
    public class GetApplicationsForJobQuery : IRequest<List<JobApplicationDto>>
    {
        public int JobId { get; set; }

        public GetApplicationsForJobQuery(int jobId)
        {
            JobId = jobId;
        }
    }
}
