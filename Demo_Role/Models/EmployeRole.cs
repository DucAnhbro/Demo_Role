using System;
using System.Collections.Generic;

namespace Demo_Role.Models
{
    public partial class EmployeRole
    {
        public int Id { get; set; }
        public int EmployeId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }

        public virtual Employe Employe { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
