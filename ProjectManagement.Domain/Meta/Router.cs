namespace ProjectManagement.Domain.Meta
{
    public static class Router
    {
        //public const string Domain = "https://localhost:5001";
        public const string Root = "/api/";
        public static class AuthenticationRouting
        {
            public const string Prefix = Root + "Authentication/";
            public const string GoogleAuthenticationRequest = Prefix + "GoogleAuthenticationRequest";
            public const string LinkedInAuthenticationRequest = Prefix + "LinkedInAuthenticationRequest";
            public const string ExternalAuthentication = Prefix + "ExternalAuthentication";
            public const string SignUp = Prefix + "SignUp";
            public const string SignIn = Prefix + "SignIn";
            public const string ResendOtp = Prefix + "ResendOtp";
            public const string VerifyRegistrationOtp = Prefix + "VerifyRegistrationOtp";
            public const string VerifyResetPasswordOtp = Prefix + "VerifyResetPasswordOtp";
            public const string ChangePassword = Prefix + "ChangePassword";
            public const string ResetPasswordRequest = Prefix + "ResetPasswordRequest";
            public const string ResetPassword = Prefix + "ResetPassword";
            public const string Logout = Prefix + "Logout";
        }
        public static class UserRouting
        {
            public const string Prefix = Root + "User/";
            public const string GetById = Prefix + "GetById/{id}";
            public const string GetList = Prefix + "GetList";
            public const string GetUserRoles = Prefix + "GetUserRoles/{id}";
            public const string GetPaginatedList = Prefix + "GetPaginatedList";
            public const string Update = Prefix + "Update";
            public const string HardDelete = Prefix + "HardDelete/{id}";
            public const string SoftDeleteAndActivate = Prefix + "SoftDeleteAndActivate/{id}";
        }
        public static class RoleRouting
        {
            public const string Prefix = Root + "Role/";
            public const string GetById = Prefix + "GetById/{id}";
            public const string GetList = Prefix + "GetList";
            public const string GetPaginatedList = Prefix + "GetPaginatedList";
            public const string Create = Prefix + "Create";
            public const string Update = Prefix + "Update";
            public const string Delete = Prefix + "Delete/{id}";
            public const string SoftDeleteAndActivate = Prefix + "SoftDeleteAndActivate/{id}";
        }
        public static class EmailRouting
        {
            public const string Prefix = Root + "Email/";
            public const string SendEmail = Prefix + "SendEmail";
        }
        public static class NotificationRouting
        {
            public const string Prefix = Root + "Notification/";

            public const string GetById = Prefix + "GetById/{id}";
            public const string GetList = Prefix + "GetList";
            public const string GetPaginatedList = Prefix + "GetPaginatedList";
            public const string GetDropDownList = Prefix + "GetDropDownList";
            public const string Create = Prefix + "Create";
            public const string Send = Prefix + "Send";
            public const string Update = Prefix + "Update";
            public const string HardDelete = Prefix + "HardDelete/{id}";
            public const string SoftDeleteAndActivate = Prefix + "SoftDeleteAndActivate/{id}";
        }
        public static class ProjectRouting
        {
            public const string Prefix = Root + "Project/";
            public const string GetById = Prefix + "GetById/{id}";
            public const string GetList = Prefix + "GetList";
            public const string GetPaginatedList = Prefix + "GetPaginatedList";
            public const string GetDropDownList = Prefix + "GetDropDownList";
            public const string Create = Prefix + "Create";
            public const string Update = Prefix + "Update";
            public const string HardDelete = Prefix + "HardDelete/{id}";
            public const string SoftDeleteAndActivate = Prefix + "SoftDeleteAndActivate/{id}";
        }
        public static class TaskRouting
        {
            public const string Prefix = Root + "Task/";
            public const string GetById = Prefix + "GetById/{id}";
            public const string GetList = Prefix + "GetList";
            public const string GetPaginatedList = Prefix + "GetPaginatedList";
            public const string GetDropDownList = Prefix + "GetDropDownList";
            public const string GetProjectTasksPaginatedList = Prefix + "GetProjectTasksPaginatedList";
            public const string Create = Prefix + "Create";
            public const string Update = Prefix + "Update";
            public const string HardDelete = Prefix + "HardDelete/{id}";
            public const string SoftDeleteAndActivate = Prefix + "SoftDeleteAndActivate/{id}";
        }
    }
}
