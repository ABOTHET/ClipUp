using ClipUp.Shared.Objects.Models;
using Microsoft.EntityFrameworkCore;
using ClipUp.Shared.Objects.Enums;

namespace ClipUp.Database
{
    public class ApplicationContext : DbContext
    {

        private static bool _isInitialized = false;

        #region Таблицы
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DataProfile> DataProfiles { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }
        public DbSet<IpAddress> IpAddresses { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<TechnicalDataProfile> TechnicalDataProfiles { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoChannel> VideosChannels { get; set; }
        #endregion

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            bool isDevelopment = Environment
                .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment && !_isInitialized)
            {
                Database.EnsureDeleted();
            }
            Database.EnsureCreated();
            if (!_isInitialized)
            {
                InitializingDefaultValues();
            }
            _isInitialized = true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region modelBuilder
            modelBuilder.Entity<Profile>()
                .HasOne(profile => profile.Channel)
                .WithOne(channel => channel.Profile)
                .HasForeignKey<Channel>();
            modelBuilder.Entity<Profile>()
               .HasOne(profile => profile.DataProfile)
               .WithOne(dataProfile => dataProfile.Profile)
               .HasForeignKey<DataProfile>();
            modelBuilder.Entity<Profile>()
               .HasOne(profile => profile.TechnicalDataProfile)
               .WithOne(technicalDataProfile => technicalDataProfile.Profile)
               .HasForeignKey<TechnicalDataProfile>();
            modelBuilder.Entity<Profile>()
               .HasOne(profile => profile.JwtToken)
               .WithOne(jwtToken => jwtToken.Profile)
               .HasForeignKey<JwtToken>();
            modelBuilder.Entity<Video>()
                .HasOne(video => video.VideoChannel)
                .WithOne(videoChannel => videoChannel.Video)
                .HasForeignKey<VideoChannel>("VideoId");
            modelBuilder.Entity<Picture>()
                .HasOne(picture => picture.VideoChannel)
                .WithOne(videoChannel => videoChannel.Cover)
                .HasForeignKey<VideoChannel>("CoverId");
            modelBuilder.Entity<Picture>()
                .HasOne(picture => picture.Channel)
                .WithOne(channel => channel.Cover)
                .HasForeignKey<Channel>("CoverId");
            modelBuilder.Entity<Picture>()
                .HasOne(picture => picture.DataProfile)
                .WithOne(dataProfile => dataProfile.Avatar)
                .HasForeignKey<DataProfile>("AvatarId");
            #endregion
        }

        private void InitializingDefaultValues()
        {
            Role role = new Role() { RoleName = RolesEnum.User };
            Roles.Add(role);

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
