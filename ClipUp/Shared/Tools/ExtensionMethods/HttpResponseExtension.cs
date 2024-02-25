namespace ClipUp.Shared.Tools.ExtensionMethods
{
    public static class HttpResponseExtension
    {
        public static void SetRefreshToken(this HttpResponse httpResponse, string refreshToken)
        {
            httpResponse.Cookies.Append("RefreshToken", refreshToken, new()
            { HttpOnly = true, MaxAge = TimeSpan.FromDays(30) });
        }
        public static void DeleteRefreshToken(this HttpResponse httpResponse)
        {
            httpResponse.Cookies.Delete("RefreshToken");
        }
    }
}
