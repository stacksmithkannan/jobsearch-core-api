namespace JobFinder.API.DTOs
{
    public class PaginatedUserApplicationDto
    {
        public IEnumerable<JobApplicationDto>? Applications { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
