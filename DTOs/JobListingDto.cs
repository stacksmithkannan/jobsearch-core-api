namespace JobFinder.API.DTOs
{
    public class JobListingDto
    {
        public int JobId { get; set; }
        public string? JobTitle { get; set; }
        public string? Location { get; set; }
        public DateTime PostedOn { get; set; }
        public string? Description { get; set; }
        public List<string>? Skills { get; set; }
    }
}
