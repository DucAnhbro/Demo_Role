
using Demo_Role.CustomAuthorization.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace Demo_Role.CustomAuthorization.Handler
{
    public class CustomPolicyHandler:AuthorizationHandler<CustomPolicyRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomPolicyRequirement requirement)
        {
            if (context.Resource != null)
            {
                var filterContext = (DefaultHttpContext)context.Resource;
                var httpcontext = filterContext?.HttpContext;
                var email = httpcontext?.Session.GetString("Email");
                if (!string.IsNullOrEmpty(email))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
