using JobFinder.API.Application.Commands;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace JobFinder.API.Application.Handlers
{
    public class ApplyToJobHandler : IRequestHandler<ApplyToJobCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ApplyToJobHandler> _logger;

        public ApplyToJobHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplyToJobHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> Handle(ApplyToJobCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Processing job application for job ID: {JobId}", request.JobId);

            var email = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null)
            {
                _logger.LogWarning("Job application failed : User not authenticated.");
                return "UnAuthorized";
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Job application failed : User not find for email {Email} ", email);
                return "User not found.";
            }

            if (string.IsNullOrEmpty(user.ResumePath))
            {
                _logger.LogWarning("Job application failed: Resume not uploaded for user {Email}", email);
            }

            var jobExists = await _context.Jobs.AnyAsync(j => j.Id == request.JobId, cancellationToken);
            if (!jobExists)
            {
                _logger.LogWarning("Job with ID {JobId} not found", request.JobId);
                return $"Job with ID {request.JobId} does not exist.";
            }

            var alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.UserId == user.Id && a.Jobid == request.JobId, cancellationToken);

            if (alreadyApplied)
            {
                _logger.LogInformation("User {UserId} has already applied to Job ID {JobId}", user.Id, request.JobId);
                return "You have already applied for this job.";
            }
         
            var application = new JobApplication
            {
                UserId = user.Id,
                Jobid = request.JobId,
                AppliedOn = DateTime.UtcNow,
                ResumePath = user.ResumePath!
            };

            await _context.JobApplications.AddAsync(application, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {Email} successfully applied to Job ID {JobId}", email, request.JobId);
            return "Applied successfully";
        }
    }
}
