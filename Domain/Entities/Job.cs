using System;
namespace JobFinder.API.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string[] Skills { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    }
}
