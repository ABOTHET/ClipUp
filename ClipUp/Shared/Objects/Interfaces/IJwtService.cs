using ClipUp.Shared.Objects.Models;

namespace ClipUp.Shared.Objects.Interfaces
{
    public interface IJwtService
    {
        public string GenerateAccessToken(Profile profile);
        public string GenerateRefreshToken(Profile profile);
        public IDictionary<string, object>? AccessTokenIsValid(string accessToken);
        public IDictionary<string, object>? RefreshTokenIsValid(string refreshToken);
    }
}
