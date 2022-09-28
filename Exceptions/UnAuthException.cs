using System.Net;

namespace CoreApiTest.Exceptions
{
    [Serializable]
    public class UnAuthException : ApiException
    {
        public UnAuthException() : base((HttpStatusCode)401, string.Empty)
        {
        }
    }
}