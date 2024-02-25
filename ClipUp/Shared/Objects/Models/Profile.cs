using Microsoft.EntityFrameworkCore;

namespace ClipUp.Shared.Objects.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Email), IsUnique = true)]
    public class Profile
    {
        public Guid Id { get; set; }
        public required virtual DataProfile? DataProfile { get; set; }
        public required virtual TechnicalDataProfile? TechnicalDataProfile { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
        public virtual ICollection<IpAddress> IpAddresses { get; set; } = new List<IpAddress>();
        public required virtual JwtToken? JwtToken { get; set; }
        public virtual List<Role> Roles { get; set; } = [];
        public virtual Channel? Channel { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Dislike> Dislikes { get; set; } = new List<Dislike>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
