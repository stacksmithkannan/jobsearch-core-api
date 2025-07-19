using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace JobFinder.API.Application.Handlers
{
    public class UploadResumeHandler:IRequestHandler<UploadResumeCommand,string>
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UploadResumeHandler> _logger;

        public UploadResumeHandler(ApplicationDbContext context, IWebHostEnvironment env, 
            IHttpContextAccessor httpContextAccessor, ILogger<UploadResumeHandler> logger)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> Handle(UploadResumeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling resume upload request.");

            if (request.File == null || request.File.Length == 0)
            {
                _logger.LogWarning("Resume upload failed: No file provided.");
                return "No file uploaded.";
            }

            var ext = Path.GetExtension(request.File.FileName).ToLower();
            if(ext != ".pdf" && ext != ".doc" && ext != ".docx")
            {
                _logger.LogWarning("Resume upload failed: Invalid file extension: { Extension}", ext);
                return "Invalid file type.";
            }

            if(request.File.Length > 5 * 1024 * 1024)
            {
                _logger.LogWarning("Resume upload failed: File too large({Size} bytes)",request.File.Length);
                return "File too large";
            }

            var email = _httpContextAccessor.HttpContext?.User?.Claims.
                        FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null) 
            {
                _logger.LogWarning("Resume upload failed: User not authenticated.");
                return "Unauthorized";
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Resume upload failed: User not found for email {Email}", email);
                return "Unauthorized";
            }

            try
            {
                var folder = Path.Combine(_env.WebRootPath ?? "wwwroot", "resumes");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                if (Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(folder, fileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream, cancellationToken);
                }

                user.ResumePath = $"/resumes/{fileName}";
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Resume uploaded successfully for user: {Email}", email);
                return user.ResumePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resume upload failed for user: {Email}", email);
                return "An error occurred while uploading the resume.";
            }

        }
    }
}
