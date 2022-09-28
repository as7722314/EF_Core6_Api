using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreApiTest.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case UnAuthException:
                    context.Result = new UnauthorizedResult();
                    break;

                default:
                    break;
            }
        }
    }
}