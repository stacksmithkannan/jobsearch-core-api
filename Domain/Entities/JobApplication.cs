using JobFinder.API.Domain.Enums;

namespace JobFinder.API.Domain.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int Jobid { get; set; }
        public int UserId { get; set; }
        public DateTime AppliedOn { get; set; }
        public string ResumePath { get; set; } = string.Empty;

        public User? User { get; set; }
        public Job? Job { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    }
}
