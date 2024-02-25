using Microsoft.Extensions.Primitives;
using System.Net;

namespace ClipUp.Shared.Tools.ExtensionMethods
{
    public static class HttpRequestExtension
    {
        public static IPAddress GetIpAddress(this HttpRequest httpRequest)
        {
            IPAddress ipAddress = httpRequest.HttpContext.Connection.RemoteIpAddress!.MapToIPv4();
            return ipAddress;
        }
        public static string? GetRefreshToken(this HttpRequest httpRequest)
        {
            string? refreshToken = null;
            httpRequest.Cookies.TryGetValue("RefreshToken", out refreshToken);
            return refreshToken;
        }
        public static string? GetAccessToken(this HttpRequest httpRequest)
        {
            StringValues headers;
            httpRequest.Headers.TryGetValue("Authorization", out headers);
            if (headers.Count == 0) { return null; }
            string? bearerToken = (from header in headers
                                 where header.IndexOf("Bearer") != -1
                                 select header).FirstOrDefault();
            if (bearerToken == null) { return null; }
            try
            {
                string[] data = bearerToken.Split(' ');
                return data[1];
            } catch
            {
                return null;
            }
        }
        public static Guid GetId(this HttpRequest httpRequest)
        {
            StringValues values;
            httpRequest.Headers.TryGetValue("Id", out values);
            if (values.Count == 0) { throw new Exception("Не удаётся получить id профиля"); }
            Guid id = new Guid(values[0]!);
            return id;
        }
        public static void SetId(this HttpRequest httpRequest, Guid id)
        {
            httpRequest.Headers.Append("Id", id.ToString());
        }
    }
}
