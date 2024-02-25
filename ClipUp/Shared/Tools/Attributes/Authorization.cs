using ClipUp.Database;
using ClipUp.Shared.Objects.Enums;
using ClipUp.Shared.Objects.Interfaces;
using ClipUp.Shared.Objects.Models;
using ClipUp.Shared.Tools.ActionResults;
using ClipUp.Shared.Tools.ExtensionMethods;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ClipUp.Shared.Tools.Attributes
{
    public class Authorization : Attribute, IAsyncAuthorizationFilter
    {
        private RolesEnum[] _roles { get; set; }

        public Authorization(params RolesEnum[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            HttpResponse response = context.HttpContext.Response;
            #region Проверка токена
            string? token = request.GetAccessToken();
            if (token == null) { context.Result = new UnauthorizedResult(); return; }
            IJwtService jwtService = context.HttpContext.RequestServices.GetService<IJwtService>()!;
            IDictionary<string, object>? payload = jwtService.AccessTokenIsValid(token);
            if (payload == null) { context.Result = new UnauthorizedResult(); return; }
            ApplicationContext database = context.HttpContext.RequestServices
                .GetService<ApplicationContext>()!;
            Guid id = new Guid(payload[nameof(Profile.Id)].ToString()!);
            Profile? profile = await database.Profiles.FindAsync(id);
            if (profile == null) { context.Result = new BadRequestResult(); return; }
            IPAddress currentIp;
            IPAddress.TryParse(payload["CurrentIp"].ToString(), out currentIp!);
            if (currentIp == null) { context.Result = new UnauthorizedResult(); return; }
            IPAddress? ip = request.GetIpAddress();
            if (ip == null) { context.Result = new BadRequestResult(); return; }
            if (currentIp.ToString() != ip.ToString()) { context.Result = new UnauthorizedResult(); return; }
            request.SetId(id);
            #endregion
            #region Проверка ролей
            if (_roles.Length <= 0) { return; }
            IEnumerable<RolesEnum> roles = from role in profile.Roles
                                           select role.RoleName;
            IEnumerable<RolesEnum> isAccess = from _role in _roles
                                              from role in roles
                                              where _role == role
                                              select _role;
            if (!isAccess.Any()) { context.Result = new AccessDenied(); return; }
            #endregion
        }
    }
}
