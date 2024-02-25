using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace ClipUp.Services
{
    public class JwtService : IJwtService
    {
        #region Переменные
        public IConfiguration Configuration { get; set; }
        private IJwtEncoder _accessEncoder { get; set; }
        private IJwtEncoder _refreshEncoder { get; set; }
        private IJwtDecoder _accessDecoder { get; set; }
        private IJwtDecoder _refreshDecoder { get; set; }
        private string _accessKey { get; set; }
        private string _refreshKey { get; set; }
        #endregion

        public JwtService(IConfiguration configuration)
        {
            Configuration = configuration;
            _accessKey = Configuration["Secure:AccessKey"]!;
            _refreshKey = Configuration["Secure:RefreshKey"]!;
            _accessEncoder = GetEncoder();
            _refreshEncoder = GetEncoder();
            _accessDecoder = GetDecoder();
            _refreshDecoder = GetDecoder();
        }

        private IDictionary<string, object> GetDefaultPayload(Profile profile)
        {
            IDictionary<string, object> Payload = new Dictionary<string, object>
            {
                { nameof(Profile.Id), profile.Id },
                { nameof(Profile.Email), profile.Email },
                { nameof(Profile.DataProfile.Login), profile.DataProfile!.Login },
                { nameof(Profile.Roles), from role in profile.Roles
                                         select role.RoleName },
                { "CurrentIp", (from ip in profile.IpAddresses
                               let date = ip.LastLoginDate
                               orderby date ascending
                               let currentIp = ip.IPAddress.ToString()
                               select currentIp).LastOrDefault()! },
            };
            return Payload;
        }

        private IJwtEncoder GetEncoder()
        {;
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder;
        }

        private IJwtDecoder GetDecoder()
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            return decoder;
        }

        public string GenerateAccessToken(Profile profile)
        {
            IDictionary<string, object?> payload = new Dictionary<string, object?>(GetDefaultPayload(profile)!)
            {
                { "exp", UnixEpoch.GetSecondsSince(new UtcDateTimeProvider().GetNow().AddMinutes(30)) }
            };
            string token = _accessEncoder.Encode(payload, _accessKey);
            return token;
        }

        public string GenerateRefreshToken(Profile profile)
        {
            IDictionary<string, object?> payload = new Dictionary<string, object?>(GetDefaultPayload(profile)!)
            {
                { "exp", UnixEpoch.GetSecondsSince(new UtcDateTimeProvider().GetNow().AddMonths(1)) }
            };
            string token = _refreshEncoder.Encode(payload, _refreshKey);
            return token;
        }

        public IDictionary<string, object>? RefreshTokenIsValid(string refreshToken)
        {
            try
            {
                IDictionary<string, object> data = _refreshDecoder
                    .DecodeToObject<IDictionary<string, object>>(refreshToken, _refreshKey);
                return data;
            } catch
            {
                return null;
            }
        }

        public IDictionary<string, object>? AccessTokenIsValid(string accessToken)
        {
            try
            {
                IDictionary<string, object> data = _accessDecoder
                    .DecodeToObject<IDictionary<string, object>>(accessToken, _accessKey);
                return data;
            } catch
            {
                return null;
            }
        }
    }
}
