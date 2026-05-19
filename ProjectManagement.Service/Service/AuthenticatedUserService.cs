using ProjectManagement.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ProjectManagement.Service.Service
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion
        #region Methods
        public int GetAuthenticatedUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return 0;
            }
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
        public string GetAuthenticatedUserName()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity?.Name ?? string.Empty;
        }
        public string GetAuthenticatedUserEmail()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var emailClaim = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            return emailClaim?.Value ?? string.Empty;
        }
        public string GetAuthenticatedUserFullName()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var emailClaim = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            return emailClaim?.Value ?? string.Empty;
        }
        public bool IsAuthenticated()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user != null && user.Identity.IsAuthenticated;
        }
        public bool IsInRole(string role)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user != null && user.IsInRole(role);
        }
        #endregion
    }
}
