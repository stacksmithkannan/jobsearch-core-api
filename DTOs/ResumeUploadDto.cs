using System.ComponentModel.DataAnnotations;

namespace JobFinder.API.DTOs
{
    public class ResumeUploadDto
    {
        [Required]
        public IFormFile? File { get; set; }
    }

}
