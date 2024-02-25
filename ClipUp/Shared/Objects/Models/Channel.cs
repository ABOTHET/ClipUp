using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(ChannelName), IsUnique = true)]
    public class Channel
    {
        public Guid Id { get; set; }
        public virtual Profile Profile { get; set; } = null!;
        public virtual ICollection<VideoChannel> VideosChannel { get; set; } = new List<VideoChannel>();
        public virtual ICollection<Subscription> Subscribers { get; set; } = new List<Subscription>();
        public virtual Picture? Cover { get; set; }

        public required string ChannelName { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
