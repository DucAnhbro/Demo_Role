using System;
using System.Collections.Generic;

namespace Demo_Role.Models
{
    public partial class Group
    {
        public Group()
        {
            Employes = new HashSet<Employe>();
        }

        public int Id { get; set; }
        public string? GroupName { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Employe> Employes { get; set; }
    }
}
