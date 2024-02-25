using Microsoft.AspNetCore.Mvc;

namespace ClipUp.Shared.Tools.ActionResults
{
    public class LimitExceededProfile : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = 400;
            await response.WriteAsync("Превышен лимит профилей");
        }
    }
}
