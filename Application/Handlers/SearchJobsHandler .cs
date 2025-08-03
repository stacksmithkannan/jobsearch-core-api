using JobFinder.API.Application.Queries;
using JobFinder.API.Data;
using JobFinder.API.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.API.Application.Handlers
{
    public class SearchJobsHandler : IRequestHandler<SearchJobsQuery,IEnumerable<JobListingDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchJobsHandler> _logger;

        public SearchJobsHandler(ApplicationDbContext context, ILogger<SearchJobsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<JobListingDto>> Handle(SearchJobsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Searching jobs with keyword: {KeyWord}", request.KeyWord);

                var baseQuery = _context.Jobs
                    .AsNoTracking()
                    .Where(j =>
                    string.IsNullOrEmpty(request.KeyWord) ||
                     j.Title.Contains(request.KeyWord) ||
                        j.Description.Contains(request.KeyWord) ||
                        j.Location.Contains(request.KeyWord))
                    .OrderByDescending(j => j.PostedDate);

                var jobList = await baseQuery.ToListAsync(cancellationToken);

                if(!string.IsNullOrEmpty(request?.KeyWord))
                {
                    jobList = jobList.Where(j => j.Skills != null &&
                                                 j.Skills.Any(s => s.Contains(request.KeyWord,StringComparison.OrdinalIgnoreCase))).ToList();
                }

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


                _logger.LogInformation("Returning {Count} jobs after filtering and pagination", paginatedJobs.Count);
                return paginatedJobs;


            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while searching jobs with keyword: {Keyword}", request.KeyWord);
                return Enumerable.Empty<JobListingDto>();
            }
        }
    }
}
