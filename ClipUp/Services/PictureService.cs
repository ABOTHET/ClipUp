using ClipUp.Database;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;

using ImageMedia = System.Net.Mime.MediaTypeNames;

namespace ClipUp.Services
{
    public class PictureService : IPictureService
    {
        public ApplicationContext ApplicationContext { get; set; }

        public PictureService(ApplicationContext applicationContext) 
        {
            ApplicationContext = applicationContext;
        }

        public async Task<bool> SaveAsWebpAsync(string path)
        {
            try
            {
                using (Image image = Image.Load(path))
                {
                    await image.SaveAsWebpAsync(path,
                        new WebpEncoder() { Method = WebpEncodingMethod.Level0 });
                }
            } catch (UnknownImageFormatException)
            {
                return false;
            }
            return true;
        }

        public async Task<Guid> CreatePictureAsync(Guid id, string path)
        {
            Profile? profile = await ApplicationContext.Profiles.FindAsync(id);
            Picture picture = new()
            {
                Content = File.ReadAllBytes(path),
            };
            profile!.Pictures.Add(picture);
            await ApplicationContext.SaveChangesAsync();
            return picture.Id;
        }

    }
}
