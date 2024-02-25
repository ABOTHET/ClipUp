using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    public class TechnicalDataProfile
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;

        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public required IPAddress FirstIp { get; set; }
        public bool IsBanned { get; set; } = false;
        public bool EmailConfirmed { get; set; } = false;
    }
}
