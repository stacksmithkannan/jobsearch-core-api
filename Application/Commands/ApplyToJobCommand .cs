using MediatR;

namespace JobFinder.API.Application.Commands
{
    public class ApplyToJobCommand : IRequest<string>
    {
        public int JobId {  get; set; } 
        public ApplyToJobCommand(int jobId)
        {
            JobId = jobId;
        }
    }
}
