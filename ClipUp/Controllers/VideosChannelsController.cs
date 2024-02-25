using ClipUp.Database;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Objects.ViewModels;
using ClipUp.Shared.Tools.Attributes;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosChannelsController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }

        public VideosChannelsController(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        [HttpPost]
        [Authorization]
        public async Task<IActionResult> CreateVideoChannel(CreateVideoChannelDTO createVideoChannelDTO)
        {
            Guid profileId = Request.GetId();
            Profile? profile = await ApplicationContext.Profiles.FindAsync(profileId);
            Channel? channel = profile!.Channel;
            if (channel == null) { return BadRequest("У вас нет канала"); }
            Picture? picture = profile.Pictures
                .Where(picture => picture.Id == createVideoChannelDTO.CoverId)
                .FirstOrDefault();
            if (picture == null) { return BadRequest("Данной картинки не существует"); }
            Video? video = profile.Videos
                .Where(video => video.Id == createVideoChannelDTO.VideoId)
                .FirstOrDefault();
            if (video == null) { return BadRequest("Данного видео не существует"); }
            VideoChannel videoChannel = new()
            {
                VideoName = createVideoChannelDTO.VideoName,
                Description = createVideoChannelDTO.Description,
                Video = video,
                Cover = picture
            };
            channel.VideosChannel.Add(videoChannel);
            await ApplicationContext.SaveChangesAsync();
            return Ok(videoChannel.Id);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> FindVideoChannel(Guid id)
        {
            VideoChannel? videoChannel = await ApplicationContext.VideosChannels.FindAsync(id);
            if (videoChannel == null) { return NotFound(); }
            VideoChannelViewModel videoChannelViewModel = new(videoChannel);
            return Ok(videoChannelViewModel);
        }

    }
}
