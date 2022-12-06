using Demo_Role.Models;

namespace Demo_Role.Dto
{
    public class EmployeDetail
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<EmployeRoleDto>? Roles { get; set; }

            public EmployeDetail() { }

        public EmployeDetail(int id, string name)
        {
            Id = id;
            Name = name;
        }
        //public EmployeDetail(Employe user)
        //{
        //    Id = user.Id;
        //    Name = user.UserName;
        //}

    }

    public class EmployeRoleDto
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Action { get; set; }
            public string Controller { get; set; }
            public bool Status { get; set; }
     }
}
