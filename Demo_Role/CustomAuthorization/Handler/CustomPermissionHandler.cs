using Demo_Role.CustomAuthorization.Requirement;
using Microsoft.AspNetCore.Authorization;
using Demo_Role.Models;
using Demo_Role.Constants;
using Demo_Role.Dto;

namespace Demo_Role.CustomAuthorization.Handler
{
    public class CustomPermissionHandler : AuthorizationHandler<CustomPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomPermissionRequirement requirement)
        {
            var filterContext = (DefaultHttpContext)context.Resource;
            var httpcontext = filterContext?.HttpContext;
            var userStr = httpcontext?.Session.GetString(SessionConstants.USERROLE);
            var userRoles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeRoleDto>>(userStr);
            //userRoles.Count();
            var isAllowAccess = userRoles.Where(x => x.Action == requirement.Action && x.Controller == requirement.Function).ToList();
            if (isAllowAccess != null)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }   
}
