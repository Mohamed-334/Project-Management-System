namespace ProjectManagement.Core.Features.Projects.Dto
{
    public class ProjectFullDataDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameLocalization { get; set; }
        public string? Description { get; set; }
        public string? DescriptionLocalization { get; set; }
    }
}
