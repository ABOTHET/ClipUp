using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    public class Picture
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;
        public virtual VideoChannel? VideoChannel { get; set; }
        public virtual Channel? Channel { get; set; }
        public virtual DataProfile? DataProfile { get; set; }

        public required byte[] Content { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
