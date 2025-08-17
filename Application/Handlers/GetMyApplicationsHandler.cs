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
    public class GetMyApplicationsHandler : IRequestHandler<GetMyApplicationsQuery, PaginatedUserApplicationDto>
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

        public async Task<PaginatedUserApplicationDto> Handle(GetMyApplicationsQuery request,CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Unauthorized request to fetch applications.");
                return new PaginatedUserApplicationDto
                {

                    Applications = Enumerable.Empty<JobApplicationDto>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0
                };

            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email,cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", email);
                return new PaginatedUserApplicationDto();
            }

            var query = _context.JobApplications
                       .Include(a => a.Job)
                       .Include(a => a.User)
                       .Where(a => a.UserId == user.Id)
                       .AsQueryable();

            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 10;

            var totalCount = await _context.JobApplications
                                .Where(a => a.UserId == user.Id)
                                .CountAsync(cancellationToken);

            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Include(a => a.User)
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.AppliedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new JobApplicationDto
                {
                    UserName = a.User.UserName,
                    Email = a.User.Email,
                    JobId = a.Jobid,
                    JobTitle = a.Job.Title,
                    Location = a.Job.Location,
                    PostedOn = a.Job.PostedDate,
                    Description = a.Job.Description,
                    Skills = a.Job.Skills,
                    Status = a.Status.ToString(),
                    AppliedOn = a.AppliedOn,
                    Resumepath = a.ResumePath
                })
                .ToListAsync(cancellationToken);

            return new PaginatedUserApplicationDto
            {
                Applications = applications,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
