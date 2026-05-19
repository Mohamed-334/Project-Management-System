namespace ProjectManagement.Core.Features.Roles.Dto
{
    public class UserRolesDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameLocalization { get; set; }
        public bool HasRole { get; set; }
    }
}
