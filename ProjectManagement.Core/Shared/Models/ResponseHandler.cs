using ProjectManagement.Infrastructure.Shared.Localization;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ProjectManagement.Core.Shared.Models
{
    public class ResponseHandler
    {

        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public ResponseHandler(IStringLocalizer<AppLocalization> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Methods

        public Response<T> Deleted<T>(string? msg = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = msg ?? _stringLocalizer[AppLocalizationKeys.Deleted]
            };
        }
        public Response<T> Success<T>(T? entity = default, string? msg = null, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = msg ?? _stringLocalizer[AppLocalizationKeys.Success],
                Meta = Meta
            };
        }
        public Response<T> Unauthorized<T>(string? msg = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = msg ?? _stringLocalizer[AppLocalizationKeys.UnAuthorized]
            };
        }
        public Response<T> BadRequest<T>(string? Message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? _stringLocalizer[AppLocalizationKeys.BadRequest] : Message
            };
        }
        public Response<T> UnprocessableEntity<T>(string? Message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = Message == null ? _stringLocalizer[AppLocalizationKeys.UnprocessableEntity] : Message
            };
        }
        public Response<T> NotFound<T>(string? message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? _stringLocalizer[AppLocalizationKeys.NotFound] : message
            };
        }

        public Response<T> Created<T>(T? entity = default, string? msg = null, object? Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = msg ?? _stringLocalizer[AppLocalizationKeys.Created],
                Meta = Meta
            };
        }
        #endregion
    }
}
