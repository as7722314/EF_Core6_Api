using System.Net;

namespace CoreApiTest.Exceptions
{
    [Serializable]
    public class UnAuthException : ApiException
    {
        public UnAuthException() : base(HttpStatusCode.Unauthorized, string.Empty)
        {
        }
    }
}