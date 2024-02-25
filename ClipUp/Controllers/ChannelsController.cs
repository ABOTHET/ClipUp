using ClipUp.Database;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Tools.Attributes;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }

        public ChannelsController(ApplicationContext applicationContext) 
        {
            ApplicationContext = applicationContext;
        }

        [HttpPost]
        [Authorization]
        public async Task<IActionResult> CreateChannelAsync(CreateChannelDTO createChannelDTO)
        {
            Guid id = Request.GetId();
            Profile? profile = await ApplicationContext.Profiles.FindAsync(id);
            Channel channel = new()
            {
                ChannelName = createChannelDTO.ChannelName,
                Description = createChannelDTO.Description,
            };
            profile!.Channel = channel;
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }

    }
}
