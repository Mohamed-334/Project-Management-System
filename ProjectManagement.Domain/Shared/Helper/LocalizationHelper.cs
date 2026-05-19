using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Shared.Helper
{
    public static class LocalizationHelper
    {
        private static bool IsArabic => CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

        public static string GetLocalizedName(string name, string? nameLocalization)
        {
            return IsArabic ? (nameLocalization ?? name) : name;
        }

        public static string GetLocalizedValue(string defaultValue, string? localizedValue)
        {
            return IsArabic ? (localizedValue ?? defaultValue) : defaultValue;
        }
    }
}
