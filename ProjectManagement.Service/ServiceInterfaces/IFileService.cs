using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IFileService
    {
        Task<string> UploadImage(string DirectorName, IFormFile file);
    }
}
