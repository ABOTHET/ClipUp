using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    public class Subscription
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;
        public virtual Channel Channel { get; set; } = null!;
    }
}
