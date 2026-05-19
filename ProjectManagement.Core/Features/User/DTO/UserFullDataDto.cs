namespace ProjectManagement.Core.Features.ApplicationUser.DTO
{
    public class UserFullDataDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? NationalNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public int? RoleId { get; set; }
        public string? Role { get; set; }
    }
}
