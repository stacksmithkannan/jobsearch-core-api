namespace JobFinder.API.DTOs
{
    public class PaginatedJobListDto
    {
        public IEnumerable<JobListingDto> Jobs { get; set; } = new List<JobListingDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
