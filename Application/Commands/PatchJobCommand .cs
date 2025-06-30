using MediatR;

namespace JobFinder.API.Application.Commands
{
    public class PatchJobCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Company { get; set; }
        public List<string>? Skills { get; set; }
    }
}
