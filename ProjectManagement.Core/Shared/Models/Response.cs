using System.Net;

namespace ProjectManagement.Core.Shared.Models
{
    public class Response<TResult>
    {
        public Response() { }
        public Response(TResult? data, string? message = null!)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string? message)
        {
            Succeeded = false;
            Message = message;
        }
        public Response(string? message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object? Meta { get; set; }

        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public TResult? Data { get; set; }
    }
}
