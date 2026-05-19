using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Service.Service
{
    public class FileService : IFileService
    {
        #region Fields
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public FileService(IWebHostEnvironment webHostEnvironment, IStringLocalizer<AppLocalization> stringLocalizer)
        {
            _webHostEnvironment = webHostEnvironment;
            _stringLocalizer = stringLocalizer;
        }

        #endregion
        #region Methods

        public async Task<string> UploadImage(string DirectorName, IFormFile file)
        {
            var path = _webHostEnvironment.WebRootPath + "/Images/" + DirectorName + "/";
            if (file != null && file.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = File.Create(path + file.FileName))
                    {
                        path += file.FileName;
                        await file.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                        return $"/Images/{DirectorName}/{file.FileName}";
                    }
                }
                catch (Exception)
                {
                    return _stringLocalizer[AppLocalizationKeys.FailedToUploadImage];
                }
            }
            else
            {
                return _stringLocalizer[AppLocalizationKeys.NoImage];
            }
        }
        #endregion


    }
}
