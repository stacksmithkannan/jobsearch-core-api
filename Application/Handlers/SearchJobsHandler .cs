using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using JobFinder.API.Domain.Entities;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class SearchJobsHandler : IRequestHandler<SearchJobsQuery, PaginatedJobListDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchJobsHandler> _logger;

        public SearchJobsHandler(ApplicationDbContext context, ILogger<SearchJobsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedJobListDto> Handle(SearchJobsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Searching jobs with keyword: {KeyWord}", request.KeyWord);

                var keyword = request.KeyWord?.ToLower();


                var initialQuery = _context.Jobs
                    .AsNoTracking()
                    .Where(j =>
                        string.IsNullOrEmpty(keyword) ||
                        j.Title.ToLower().Contains(keyword) ||
                        j.Description.ToLower().Contains(keyword) ||
                        j.Location.ToLower().Contains(keyword))
                    .OrderByDescending(j => j.PostedDate);

                var jobList = await initialQuery.ToListAsync(cancellationToken);

                
                if (!string.IsNullOrEmpty(keyword))
                {
                    jobList = jobList
                        .Where(j => j.Skills != null &&
                                    j.Skills.Any(s =>
                                        !string.IsNullOrWhiteSpace(s) &&
                                        s.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                var totalCount = jobList.Count;

                var paginatedJobs = jobList
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(j => new JobListingDto
                    {
                        JobId = j.Id,
                        JobTitle = j.Title,
                        Description = j.Description,
                        Location = j.Location,
                        PostedOn = j.PostedDate,
                        Skills = j.Skills
                    })
                    .ToList();

                return new PaginatedJobListDto
                {
                    Jobs = paginatedJobs,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching jobs with keyword: {Keyword}", request.KeyWord);
                return new PaginatedJobListDto
                {
                    Jobs = Enumerable.Empty<JobListingDto>(),
                    TotalCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
        }
    }

}
