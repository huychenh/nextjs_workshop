using System.Collections.Specialized;
using System.Net;

namespace CarStore.TestAuthentication.Responses.Objects
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public HttpStatusCode NotificationCode { get; set; }
        public string InfoMessage { get; set; }
        public string ErrorMessage { get; set; }
        public ListDictionary DetailErrorMessage { get; set; }
    }

    public class Error
    {
        public Error(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }
    }
}
