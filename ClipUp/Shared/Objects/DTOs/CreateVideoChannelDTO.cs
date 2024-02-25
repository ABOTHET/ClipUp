using ClipUp.Shared.Objects.Models;
using System.ComponentModel.DataAnnotations;

namespace ClipUp.Shared.Objects.DTOs
{
    public class CreateVideoChannelDTO
    {
        [Required]
        public required Guid VideoId { get; set; }
        [Required]
        public required Guid CoverId { get; set; }
        [Required]
        public required string VideoName { get; set; }
        public string? Description { get; set; }
    }
}
