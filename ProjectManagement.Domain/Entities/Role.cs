using ProjectManagement.Domain.Shared.BaseEntity.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace ProjectManagement.Domain.Entities
{
    public class Role : IdentityRole<int>, IBaseEntityWithName
    {
        public string? NameLocalization { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatorName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? ModifierName { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? DeleterName { get; set; }
        public DateTime? DeletionDate { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public string? GetLocalizedName()
        {
            if (string.IsNullOrEmpty(NameLocalization))
                return Name;
            CultureInfo CultureInfo = Thread.CurrentThread.CurrentCulture;
            if (CultureInfo.TwoLetterISOLanguageName.ToLower().Equals("ar"))
                return NameLocalization;
            return Name;
        }
    }
}
