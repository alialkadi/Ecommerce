namespace AdminDashboard.Models
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
