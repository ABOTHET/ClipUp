using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(FileName), IsUnique = true)]
    public class Video
    {
        public Guid Id { get; set; }
        public virtual VideoChannel? VideoChannel { get; set; }
        public virtual Profile Profile { get; set; } = null!;

        public required string FileName { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
