using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace JobFinder.API.Application.Handlers
{
    public class GetMyApplicationsHandler : IRequestHandler<GetMyApplicationsQuery,List<JobApplicationDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetMyApplicationsHandler> _logger;

        public GetMyApplicationsHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<GetMyApplicationsHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<JobApplicationDto>> Handle(GetMyApplicationsQuery request,CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Unauthorized request to fetch applications.");
                return new List<JobApplicationDto>();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email,cancellationToken);

            var applications = await _context.JobApplications
                               .Include(a => a.Job)
                               .Where( a => a.UserId == user.Id)
                               .Select(a => new JobApplicationDto
                               {
                                   UserName = a.User.UserName,
                                   Email = a.User.Email,
                                   JobId = a.Jobid,
                                   JobTitle = a.Job.Title,
                                   AppliedOn = a.AppliedOn,
                                   Resumepath = a.ResumePath
                               })
                               .ToListAsync(cancellationToken);
            return applications;
        }
    }
}
