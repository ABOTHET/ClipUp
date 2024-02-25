using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    public class Dislike
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;
        public virtual VideoChannel VideoChannel { get; set; } = null!;
    }
}
