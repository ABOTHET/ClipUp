using ClipUp.Database;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Tools.Attributes;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileSystem = System.IO.File;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }
        public IPictureService PictureService { get; set; }

        public PicturesController(ApplicationContext applicationContext, IPictureService pictureService)
        {
            ApplicationContext = applicationContext;
            PictureService = pictureService;
        }

        [HttpPost]
        [Authorization]
        public async Task<IActionResult> UploadPictureAsync(IFormFile picture)
        {
            if (picture.Length <= 0) { return BadRequest(); }
            Guid id = Request.GetId();
            string path = Path.GetTempFileName();
            using (var stream = FileSystem.Create(path))
            {
                await picture.CopyToAsync(stream);
            }
            bool pictureIsValid = await PictureService.SaveAsWebpAsync(path);
            if (!pictureIsValid) { return BadRequest(); }
            Guid pictureId = await PictureService.CreatePictureAsync(id, path);
            return Ok(pictureId);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPictureAsync(Guid id)
        {
            Picture? picture = await ApplicationContext.Pictures.FindAsync(id);
            if (picture == null) { return NotFound(); }
            return File(picture.Content, "image/webp");
        }

        [HttpDelete("{id:guid}")]
        [Authorization]
        public async Task<IActionResult> DeletePictureAsync(Guid id)
        {
            Guid profileId = Request.GetId();
            Picture? picture = await ApplicationContext.Pictures
                .Where(picture => picture.Id == id && picture.Profile.Id == profileId)
                .FirstOrDefaultAsync();
            if (picture == null) { return NotFound(); };
            picture.DataProfile!.Avatar = null;
            ApplicationContext.Pictures.Remove(picture);
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Profile/{id:guid}")]
        [Authorization]
        public async Task<IActionResult> SetAvatarAsync(Guid id)
        {
            Guid profileId = Request.GetId()!;
            Picture? picture = await ApplicationContext.Pictures
                .Where(picture => picture.Id == id && picture.Profile.Id == profileId)
                .FirstOrDefaultAsync();
            if (picture == null) { return NotFound(); }
            picture.Profile.DataProfile!.Avatar = picture;
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Channel/{id:guid}")]
        [Authorization]
        public async Task<IActionResult> SetCoverAsync(Guid id)
        {
            Guid profileId = Request.GetId();
            Profile? profile = await ApplicationContext.Profiles.FindAsync(profileId);
            Picture? picture = profile!.Pictures
                .Where(picture => picture.Id == id && profile.Id == profileId)
                .FirstOrDefault();
            if (picture == null) { return NotFound(); }
            Channel? channel = profile.Channel;
            if (channel == null) { return BadRequest(); }
            channel.Cover = picture;
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }
    }
}
