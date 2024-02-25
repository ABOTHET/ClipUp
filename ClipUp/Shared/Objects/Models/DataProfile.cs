using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Login), IsUnique = true)]
    public class DataProfile
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;
        public virtual Picture? Avatar { get; set; }

        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required DateTime DateOfBirth { get; set; }
    }
}
