using ProjectManagement.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Core.Features.Authentication.Commands.RequestModels
{
    public class SignUpCommandRequestModel : IRequest<Response<string>>
    {
        public string? Name { get; set; }
        public string? NameLocalization { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? NationalNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public int? RoleId { get; set; }
        public IFormFile? ProfileImageFile { get; set; }
        public string? ProfileImageUrl { get; set; }

    }
}
