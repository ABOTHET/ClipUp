using ClipUp.Database;
using ClipUp.Services;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Tools.Attributes;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using fileSystem = System.IO.File;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }
        public IConfiguration Configuration { get; set; }

        private List<string> _formats = new List<string>() { "mp4" };

        public VideosController(ApplicationContext applicationContext, IConfiguration configuration) 
        {
            Configuration = configuration;
            ApplicationContext = applicationContext;
        }

        [HttpPost]
        [Authorization]
        public async Task<IActionResult> UploadVideo(IFormFile formVideo)
        {
            Guid id = Request.GetId();
            if (formVideo.Length <= 0) return BadRequest();
            string? format = formVideo.FileName.Split('.').LastOrDefault();
            if (format == null) return BadRequest();
            string? isValid = (from formatList in _formats
                             where formatList == format
                             select formatList).FirstOrDefault();
            if (isValid == null) return BadRequest();
            string fileName = Guid.NewGuid().ToString() + $".{format}";
            string? defaultPath = Configuration["PathFolderVideos"];
            if (defaultPath == null)
            {
                defaultPath = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "PathFolderVideos");
            }
            string path = Path.Combine(defaultPath, id.ToString());
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string pathVideo = Path.Combine(path, fileName);
            using (var stream = fileSystem.Create(pathVideo))
            {
                await formVideo.CopyToAsync(stream);
            }
            Video video = new() { FileName = fileName };
            Profile? profile = await ApplicationContext.Profiles.FindAsync(id);
            profile!.Videos.Add(video);
            await ApplicationContext.SaveChangesAsync();
            return Ok(video.Id);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> FindVideo(Guid id)
        {
            string? pathVideos = Configuration["PathFolderVideos"];
            Video? video = await ApplicationContext.Videos.FindAsync(id);
            if (video == null) return NotFound();
            Guid profileId = video.Profile.Id;
            string fileName = video.FileName;
            string path = Path.Combine(pathVideos!, profileId.ToString(), fileName);
            if(!fileSystem.Exists(path)) return NotFound();
            return File(fileSystem.ReadAllBytes(path), "video/mpeg");
        }
    }
}
