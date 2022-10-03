using System.Net;

namespace CoreApiTest.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        private readonly HttpStatusCode _statusCode;

        public ApiException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            _statusCode = httpStatusCode;
        }

        public ApiException(HttpStatusCode httpStatusCode, string message, System.Exception inner)
            : base(message, inner)
        {
            _statusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode => _statusCode;
    }
}