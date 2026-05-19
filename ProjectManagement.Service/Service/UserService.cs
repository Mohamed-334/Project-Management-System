using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.ExtensionMethods;
using ProjectManagement.Service.Shared.PaginatedList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ProjectManagement.Service.Service
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly IFileService _fileService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public UserService(UserManager<User> userManager, IStringLocalizer<AppLocalization> stringLocalizer, IFileService fileService)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
            _fileService = fileService;
        }
        #endregion

        #region Methods
        public async Task<List<User>?> GetAllAsync()
        {
            var Users = await _userManager.Users
                            .AsNoTracking()
                            .Include(u => u.UserRoles!)
                            .ThenInclude(ur => ur.Role)
                            .Where(x => x.IsDeleted == false)
                            .ToListAsync();
            return Users;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            var User = await _userManager.Users
                            .Include(u => u.UserRoles!)
                            .ThenInclude(ur => ur.Role)
                            .Where(u => u.Id == id)
                            .FirstOrDefaultAsync();
            return User;
        }
        public async Task<IdentityResult> EditAsync(User entity)
        {
            var result = await _userManager.UpdateAsync(entity);
            return result;
        }
        public async Task<IdentityResult> HardDeleteAsync(User entity)
        {
            var result = await _userManager.DeleteAsync(entity);
            return result;
        }
        public async Task<PaginatedList<User>> GetPaginatedListAsync(int pageNumber = 1, int pageSize = 10)
        {
            var Users = _userManager.Users
                                    .AsNoTracking()
                                    .Where(x => x.IsDeleted == false)
                                    .AsQueryable();

            var PaginatedList = await Users.ToPaginatedListAsync(pageNumber, pageSize);
            return PaginatedList;
        }
        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }
        public async Task<IdentityResult> AddUserToRoleAsync(User user, string role) => await _userManager.AddToRoleAsync(user, role);
        public async Task<IdentityResult> RemoveRolesAsync(User user, List<string> roles) => await _userManager.RemoveFromRolesAsync(user, roles);
        public async Task<bool> IsUserInRoleAsync(User user, string role) => (role != null && await _userManager.IsInRoleAsync(user, role));
        public async Task<User?> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<bool> IsUserNameExistAsync(string userName) => (await _userManager.FindByNameAsync(userName)) != null;
        public async Task<bool> IsUserIdExistAsync(int Id) => (await _userManager.FindByIdAsync(Id.ToString())) != null;
        public async Task<bool> IsEmailExistAsync(string email) => (await _userManager.FindByEmailAsync(email)) != null;

        public async Task<string?> UploadFileAsync(string FolderName, IFormFile file)
        {
            var UploadResult = await _fileService.UploadImage(FolderName, file);
            if (UploadResult == _stringLocalizer[AppLocalizationKeys.FailedToUploadImage])
                return _stringLocalizer[AppLocalizationKeys.FailedToUploadImage];

            if (UploadResult == _stringLocalizer[AppLocalizationKeys.NoImage])
                return null;
            else
                return UploadResult;
        }
        #endregion
    }


}
