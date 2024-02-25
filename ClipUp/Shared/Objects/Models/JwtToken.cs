using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    public class JwtToken
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;

        public string? RefreshToken { get; set; }
    }
}
