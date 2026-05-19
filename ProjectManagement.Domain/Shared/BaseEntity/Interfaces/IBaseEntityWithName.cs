using System.Globalization;

namespace ProjectManagement.Domain.Shared.BaseEntity.Interfaces
{
    public interface IBaseEntityWithName : IBaseEntity
    {
        public string? Name { get; set; }
        public string? NameLocalization { get; set; }

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
