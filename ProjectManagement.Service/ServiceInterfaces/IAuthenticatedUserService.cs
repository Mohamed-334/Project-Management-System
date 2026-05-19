namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IAuthenticatedUserService
    {
        int GetAuthenticatedUserId();
        string GetAuthenticatedUserName();
        string GetAuthenticatedUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);


    }
}
