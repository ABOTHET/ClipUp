using ClipUp.Shared.Objects.Models;

namespace ClipUp.Shared.Objects.ViewModels
{
    public class VideoChannelViewModel
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public Guid CoverId { get; set; }
        public Guid VideoId { get; set; }
        public uint Comments { get; set; }
        public uint Likes { get; set; }
        public uint Dislikes { get; set; }

        public DateTime DateOfCreation { get; set; }
        public string VideoName { get; set; }
        public string? Description { get; set; }

        public VideoChannelViewModel(VideoChannel videoChannel)
        {
            Id = videoChannel.Id;
            ChannelId = videoChannel.Channel.Id;
            CoverId = videoChannel.Cover.Id;
            VideoId = videoChannel.Video.Id;
            Comments = (uint) videoChannel.Comments.Count;
            Likes = (uint) videoChannel.Likes.Count;
            Dislikes = (uint) videoChannel.Dislikes.Count;
            DateOfCreation = videoChannel.DateOfCreation;
            VideoName = videoChannel.VideoName;
            Description = videoChannel.Description;
        }
    }
}
