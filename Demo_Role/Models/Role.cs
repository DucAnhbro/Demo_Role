using System;
using System.Collections.Generic;

namespace Demo_Role.Models
{
    public partial class Role
    {
        public Role()
        {
            EmployeRoles = new HashSet<EmployeRole>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }

        public virtual ICollection<EmployeRole> EmployeRoles { get; set; }
    }
}
