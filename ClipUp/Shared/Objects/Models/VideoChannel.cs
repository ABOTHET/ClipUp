using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(VideoName), IsUnique = true)]
    public class VideoChannel
    {
        public Guid Id { get; set; }
        public virtual Channel Channel { get; set; } = null!;
        public virtual Picture Cover { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Dislike> Dislikes { get; set; } = new List<Dislike>();

        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public required string VideoName { get; set; }
        public string? Description { get; set; }
    }
}
