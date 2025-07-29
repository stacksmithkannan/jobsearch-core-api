namespace JobFinder.API.DTOs
{
    public class JobApplicationDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Resumepath {  get; set; }
        public DateTime AppliedOn { get; set; }
    }
}
