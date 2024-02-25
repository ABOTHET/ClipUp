using ClipUp.Database;
using ClipUp.Shared.Objects.DTOs;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Objects.ViewModels;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClipUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public ApplicationContext ApplicationContext { get; set; }
        public IJwtService JwtService { get; set; }
        public IProfileService ProfileService { get; set; }

        public AuthenticationController(ApplicationContext applicationContext, IJwtService jwtService,
            IProfileService profileService) 
        {
            ApplicationContext = applicationContext;
            JwtService = jwtService;
            ProfileService = profileService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            Profile? profile = await ApplicationContext.Profiles
                .Where(profile => profile.Email == loginDTO.Email)
                .FirstOrDefaultAsync();
            if (profile == null) { return NotFound(); }
            bool isVerify = Verify(loginDTO.Password, profile.Password);
            if (!isVerify) { return BadRequest("Неверные данные"); }
            string refreshToken = JwtService.GenerateRefreshToken(profile);
            string accessToken = JwtService.GenerateAccessToken(profile);
            profile.JwtToken!.RefreshToken = refreshToken;
            Response.SetRefreshToken(refreshToken);
            IPAddress currentIp = Request.GetIpAddress();
            IpAddress lastIp = await ProfileService.FindLastIp(profile.Id);
            if (lastIp.IPAddress.ToString() == currentIp.ToString())
            {
                lastIp.LastLoginDate = DateTime.Now;
            } else
            {
                IpAddress ip = new()
                {
                    IPAddress = currentIp,
                    LastLoginDate = DateTime.Now,
                };
                profile.IpAddresses.Add(ip);
            }
            await ApplicationContext.SaveChangesAsync();
            IdAccessTokenViewModel result = new() { Id = profile.Id, AccessToken = accessToken };
            return Ok(result);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            string? refreshToken = Request.GetRefreshToken();
            if (refreshToken == null) { return Unauthorized(); }
            JwtToken? jwtToken = await ApplicationContext.JwtTokens
                .Where(token => token.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();
            if (jwtToken == null) { return Unauthorized(); }
            IDictionary<string, object>? data = JwtService.RefreshTokenIsValid(refreshToken);
            if (data == null || refreshToken != jwtToken.RefreshToken) { return Unauthorized(); }
            jwtToken.RefreshToken = null;
            Response.DeleteRefreshToken();
            await ApplicationContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync()
        {
            string? refreshTokenFromRequest = Request.GetRefreshToken();
            if (refreshTokenFromRequest == null) { return Unauthorized(); }
            JwtToken? jwtToken = await ApplicationContext.JwtTokens
                .Where(token => token.RefreshToken == refreshTokenFromRequest)
                .FirstOrDefaultAsync();
            if (jwtToken == null) { return Unauthorized(); }
            IDictionary<string, object>? data = JwtService.RefreshTokenIsValid(refreshTokenFromRequest);
            if (data == null || refreshTokenFromRequest != jwtToken.RefreshToken) { return Unauthorized(); }
            Guid id = new Guid(data[nameof(Profile.Id)].ToString()!);
            Profile? profile = await ApplicationContext.Profiles.FindAsync(id);
            if (profile == null) { return NotFound(); }
            string refreshToken = JwtService.GenerateRefreshToken(profile);
            string accessToken = JwtService.GenerateAccessToken(profile);
            profile.JwtToken!.RefreshToken = refreshToken;
            Response.SetRefreshToken(refreshToken);
            await ApplicationContext.SaveChangesAsync();
            IdAccessTokenViewModel result = new() { Id = profile.Id, AccessToken = accessToken };
            return Ok(result);
        }
    }
}
