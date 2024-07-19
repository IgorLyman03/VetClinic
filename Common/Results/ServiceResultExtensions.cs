using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Common.Results
{
    public static class ServiceResultExtensions
    {
        public static ServiceResult<T> ToServiceResult<T>(this Result<T> result, ServiceErrorType errorType = ServiceErrorType.InternalError)
        {
            return result.IsSuccess
                ? ServiceResult<T>.Success(result.Value)
                : ServiceResult<T>.Failure(result.Error, errorType);
        }

        public static ServiceResult<T> ToServiceResult<T>(this Maybe<T> maybe, string errorMessage = "Value not found", ServiceErrorType errorType = ServiceErrorType.NotFound)
        {
            return maybe.HasValue
                ? ServiceResult<T>.Success(maybe.Value)
                : ServiceResult<T>.Failure(errorMessage, errorType);
        }
        public static ServiceErrorType ToServiceErrorType(this System.Net.HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                System.Net.HttpStatusCode.NotFound => ServiceErrorType.NotFound,
                System.Net.HttpStatusCode.BadRequest => ServiceErrorType.BadRequest,
                System.Net.HttpStatusCode.Unauthorized => ServiceErrorType.Unauthorized,
                System.Net.HttpStatusCode.Forbidden => ServiceErrorType.Forbidden,
                System.Net.HttpStatusCode.InternalServerError => ServiceErrorType.InternalError,
                System.Net.HttpStatusCode.Conflict => ServiceErrorType.Conflict,
                System.Net.HttpStatusCode.RequestTimeout => ServiceErrorType.RequestTimeout,
                System.Net.HttpStatusCode.NotImplemented => ServiceErrorType.NotImplemented,
                System.Net.HttpStatusCode.ServiceUnavailable => ServiceErrorType.ServiceUnavailable,
                _ => ServiceErrorType.None
            };
        }

        public static ActionResult<T> ToActionResult<T>(this ServiceResult<T> serviceResult)
        {
            if (serviceResult.IsSuccess)
            {
                return new OkObjectResult(serviceResult.Value);
            }

            var statusCode = (int)serviceResult.Error.ErrorType;
            return new ObjectResult(new { error = serviceResult.Error.Message })
            {
                StatusCode = statusCode
            };
        }
    }
}
