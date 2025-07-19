using MediatR;

namespace JobFinder.API.Application.Commands
{
    public class UploadResumeCommand:IRequest<string>
    {
        public IFormFile? File { get; set; }

        public UploadResumeCommand(IFormFile? file)
        {
            File = file;
        }

    }
}
