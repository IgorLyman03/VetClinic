using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Results
{
    public class ServiceError
    {
        public string Message { get; }
        public ServiceErrorType ErrorType { get; }
        public int ErrorCode => (int)ErrorType;

        public ServiceError(string message, ServiceErrorType errorType)
        {
            Message = message;
            ErrorType = errorType;
        }
    }

    public enum ServiceErrorType
    {
        None = 0,
        NotFound = 404,
        ValidationError = 400,
        Unauthorized = 401,
        Forbidden = 403,
        InternalError = 500,
        BadRequest = 400,
        Conflict = 409,
        RequestTimeout = 408,
        NotImplemented = 501,
        ServiceUnavailable = 503
    }

}
