using ClipUp.Shared.Objects.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(RoleName), IsUnique = true)]
    public class Role
    {
        public Guid Id { get; set; }
        public virtual List<Profile> Profiles { get; set; } = [];

        public required RolesEnum RoleName { get; set; }
    }
}
