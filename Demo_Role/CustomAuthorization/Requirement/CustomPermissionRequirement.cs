using Microsoft.AspNetCore.Authorization;

namespace Demo_Role.CustomAuthorization.Requirement
{
        public class CustomPermissionRequirement : IAuthorizationRequirement
        {
            public string Action { get; private set; }
            public string Function { get; private set; }

            public CustomPermissionRequirement(string action, string function)
            {
                Action = action;
                Function = function;
            }
        }
}
