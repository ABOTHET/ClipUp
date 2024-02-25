using ClipUp.Database;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Tools.Attributes;
using ClipUp.Shared.Tools.Classes;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProfilesController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }

        public DataProfilesController(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        [HttpPatch]
        [Authorization]
        public async Task<IActionResult> UpdateDataProfileAsync(UpdateDataProfileDTO updateProfileDTO)
        {
            Guid id = Request.GetId();
            DataProfile? dataProfile = await ApplicationContext.DataProfiles.FindAsync(id);
            if (dataProfile == null) { return NotFound(); }
            Tools.UpdateObject(updateProfileDTO, dataProfile);
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }
    }
}
