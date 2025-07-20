using JobFinder.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobFinder.API.Application.Handlers
{
    public class DownloadResumeQueryHandler : IRequestHandler<DownloadResumeQuery, FileContentResult>
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DownloadResumeQueryHandler> _logger;

        public DownloadResumeQueryHandler(IWebHostEnvironment env, ILogger<DownloadResumeQueryHandler> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<FileContentResult> Handle(DownloadResumeQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to download resume: {FileName}", request.FileName);

            if (string.IsNullOrEmpty(request.FileName))
            {
                _logger.LogWarning("Download failed : No file name provided.");
                throw new ArgumentException("File name is required");
            }

            var path = Path.Combine(_env.WebRootPath, "resumes", request.FileName);

            if (!File.Exists(path))
            {
                _logger.LogWarning("Download failed : File {Filename} not found", request.FileName);
                throw new FileNotFoundException("Resume not found.");
            }

            var fileBytes = await File.ReadAllBytesAsync(path, cancellationToken);

            _logger.LogInformation("Resume {FileName} downloaded successfully.", request.FileName);

            return new FileContentResult(fileBytes, "application/pdf")
            {
                FileDownloadName = request.FileName,
            };
        }
    }
}
