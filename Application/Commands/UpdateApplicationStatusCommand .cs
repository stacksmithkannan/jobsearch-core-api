using JobFinder.API.Domain.Enums;
using MediatR;

namespace JobFinder.API.Application.Commands
{
    public class UpdateApplicationStatusCommand : IRequest<bool>
    {
        public int JobApplicationId { get; set; }
        public string? Status { get; set; }
    }
}