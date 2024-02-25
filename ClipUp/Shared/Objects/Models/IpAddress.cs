using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    public class IpAddress
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;

        public required IPAddress IPAddress { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastLoginDate { get; set; }
        public IpAddress() 
        {
            DateOfCreation = DateTime.Now;
            LastLoginDate = DateOfCreation;
        }
    }
}
