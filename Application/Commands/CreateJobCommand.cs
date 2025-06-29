using MediatR;

namespace JobFinder.API.Application.Commands
{
    public class CreateJobCommand: IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string[] Skills { get; set; }

    }
}
