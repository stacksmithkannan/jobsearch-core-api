using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.API.Application.Queries
{
    public class DownloadResumeQuery : IRequest<FileContentResult>
    {
        public string FileName {  get; set; }

        public DownloadResumeQuery(string fileName)
        {
            FileName = fileName;
        }
    }
}
