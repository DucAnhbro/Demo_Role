using Microsoft.AspNetCore.Authorization;

namespace Demo_Role.CustomAuthorization.Requirement
{
    public class CustomPolicyRequirement: IAuthorizationRequirement
    {
        public CustomPolicyRequirement() { }
    }
}
