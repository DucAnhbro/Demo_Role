using Microsoft.AspNetCore.Authorization;

namespace Demo_Role.CustomAuthorization
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
