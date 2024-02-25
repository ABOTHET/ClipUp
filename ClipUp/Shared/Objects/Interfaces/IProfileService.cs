using ClipUp.Shared.Objects.Classes;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Models;

namespace ClipUp.Shared.Objects.Interfaces
{
    public interface IProfileService
    {
        public Task<Profile> CreateProfileAsync(CreateProfileDTO createProfileDTO, CreateProfileOptions options = null!);
        public Task<IEnumerable<Profile>> FindProfilesAsync();
        public Task<Profile> FindProfileAsync(Guid id);
        public Task<Profile> FindProfileAsync(string login);
        public Task<IpAddress> FindLastIp(Guid id);
    }
}
