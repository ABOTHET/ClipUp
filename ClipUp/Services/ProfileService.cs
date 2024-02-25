using ClipUp.Database;
using ClipUp.Shared.Objects.Classes;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Objects.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClipUp.Services
{
    public class ProfileService : IProfileService
    {
        public ApplicationContext ApplicationContext { get; set; }

        public ProfileService(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        public async Task<Profile> CreateProfileAsync(CreateProfileDTO createProfileDTO, CreateProfileOptions options = null!)
        {
            IPAddress ipAddress = (options == null || options.IPAddress == null)
                ? new IPAddress(new byte[] { 0, 0, 0, 0 })
                : options.IPAddress;
            Profile profile = new()
            {
                Email = createProfileDTO.Email,
                Password = createProfileDTO.Password,
                DataProfile = new()
                {
                    Login = createProfileDTO.Login,
                    Name = createProfileDTO.Name,
                    Surname = createProfileDTO.Surname,
                    DateOfBirth = createProfileDTO.DateOfBirth
                },
                JwtToken = new(),
                TechnicalDataProfile = new()
                {
                    FirstIp = ipAddress
                }
            };
            Role? defaultRole = await ApplicationContext.Roles
                .Where(role => role.RoleName == RolesEnum.User)
                .FirstOrDefaultAsync();
            profile.Roles.Add(defaultRole!);
            IpAddress lastIpAddress = new() { IPAddress = ipAddress };
            profile.IpAddresses.Add(lastIpAddress);
            return profile;
        }

        public async Task<Profile> FindProfileAsync(Guid id)
        {
            Profile? profile = await ApplicationContext.Profiles.FindAsync(id);
            return profile!;
        }

        public async Task<Profile> FindProfileAsync(string login)
        {
            Profile? profile = await (from profileFromDatabase in ApplicationContext.Profiles
                               .Include(profile => profile.DataProfile)
                               where profileFromDatabase.DataProfile!.Login == login
                               select profileFromDatabase).FirstOrDefaultAsync();
            return profile!;
        }

        public async Task<IEnumerable<Profile>> FindProfilesAsync()
        {
            List<Profile> profiles = await ApplicationContext.Profiles.ToListAsync();
            return profiles;
        }

        public async Task<IpAddress> FindLastIp(Guid id)
        {
            Profile? profile = await ApplicationContext.Profiles.FindAsync(id);
            IpAddress? lastIp = (from ip in profile!.IpAddresses
                                let date = ip.LastLoginDate
                                orderby date ascending
                                select ip).LastOrDefault()!;
            return lastIp;
        }
    }
}
