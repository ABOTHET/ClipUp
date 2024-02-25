using ClipUp.Database;
using ClipUp.Shared.Objects.Classes;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Objects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClipUp.Shared.Tools.ExtensionMethods;
using System.Net;
using ClipUp.Shared.Tools.ActionResults;
using ClipUp.Shared.Tools.Attributes;
using Authorization = ClipUp.Shared.Tools.Attributes.Authorization;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }
        public IProfileService ProfileService { get; set; }

        public ProfilesController(ApplicationContext applicationContext, IProfileService profileService)
        {
            ApplicationContext = applicationContext;
            ProfileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> FindProfilesAsync()
        {
            // Без profile.DataProfile
            // IEnumerable<Profile> profiles = await ProfileService.FindProfiles();

            // С profile.DataProfile
            IEnumerable<Profile> profiles = await ApplicationContext.Profiles.ToListAsync();
            IEnumerable<ProfileViewModel> profilesViewModel = 
                profiles.Select(profile => new ProfileViewModel(profile));
            return Ok(profilesViewModel);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> FindProfileAsync(Guid id)
        {
            Profile? profile = await ProfileService.FindProfileAsync(id);
            if (profile == null) { return NotFound(); }
            ProfileViewModel profileViewModel = new(profile);
            return Ok(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfileAsync(CreateProfileDTO createProfileDTO)
        {
            IPAddress currentIp = Request.GetIpAddress();
            List<TechnicalDataProfile> technicalDataProfiles = await ApplicationContext.TechnicalDataProfiles
                .Where(technicalDataProfile => technicalDataProfile.FirstIp == currentIp)
                .ToListAsync();
            if (technicalDataProfiles.Count >= 3)
            {
                return new LimitExceededProfile();
            }
            // Хешируем пароль
            createProfileDTO.Password = HashPassword(createProfileDTO.Password); 
            CreateProfileOptions createProfileOptions = new() { IPAddress = Request.GetIpAddress() };
            Profile profile = await ProfileService.CreateProfileAsync(createProfileDTO, createProfileOptions);
            await ApplicationContext.Profiles.AddAsync(profile);
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }

    }
}
