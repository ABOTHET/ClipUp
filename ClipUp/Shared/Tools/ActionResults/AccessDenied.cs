using Microsoft.AspNetCore.Mvc;

namespace ClipUp.Shared.Tools.ActionResults
{
    public class AccessDenied : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = 403;
            await response.WriteAsync("В доступе отказано");
        }
    }
}
