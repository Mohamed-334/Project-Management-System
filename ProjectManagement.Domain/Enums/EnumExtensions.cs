using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ProjectManagement.Domain.Enums
{
    public static class EnumExtensions
    {
        #region Enums
        public enum OtpReasonEnum
        {
            Register = 1,
            ResetPassword = 2,
            Resend = 3,

        }
        public enum TaskStatusEnum
        {
            [Display(Name = "جاري العمل")]
            InProgress = 1,
            [Display(Name = "اكتمل")]
            Completed = 2,
            [Display(Name = "تم الالغاء")]
            Cancelled = 3,
        }
        public enum TaskPriorityEnum
        {
            [Display(Name = "حرج")]
            Critical = 1,
            [Display(Name = "اولوية عليا")]
            High = 2,
            [Display(Name = "اولوية متوسطة")]
            Medium = 3,
            [Display(Name = "اولوية منخفظة")]
            Low = 4,
        }
        #endregion

        #region method
        public static string? GetDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();
            var rawDisplay = displayAttribute?.GetName() ?? enumValue.ToString();
            return rawDisplay;
        }
        public static string? SplitEnumName(this Enum value)
        {
            return Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1 $2");
        }
        #endregion
    }
}
