namespace JobFinder.API.DTOs
{
    public class JobApplicationDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Resumepath {  get; set; }
        public DateTime AppliedOn { get; set; }
        public int JobId { get; set; }
        public string? JobTitle { get; set; }
        public string? Location { get; set; }
        public DateTime PostedOn { get; set; }
        public string? Description { get; set; }
        public List<string>? Skills { get; set; }
        public string? Status { get; set; } = "Pending";
    }
}

