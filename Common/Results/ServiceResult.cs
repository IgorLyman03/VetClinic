using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Results
{
    public class ServiceResult<T> 
    {
        private readonly Result<T> _result;
        public bool IsSuccess => _result.IsSuccess;
        public bool IsFailure => _result.IsFailure;
        public T Value => _result.Value;
        public ServiceError Error { get; }

        private ServiceResult(Result<T> result, ServiceError error = null)
        {
            _result = result;
            Error = error;
        }

        public static ServiceResult<T> Success(T value) => new ServiceResult<T>(Result.Success(value));

        public static ServiceResult<T> Failure(ServiceError error) => new ServiceResult<T>(Result.Failure<T>(error.Message), error);

        public static ServiceResult<T> Failure(string errorMessage, ServiceErrorType errorType) =>
            Failure(new ServiceError(errorMessage, errorType));

        public ServiceResult<TK> Map<TK>(Func<T, TK> mapper)
        {
            return IsSuccess
                ? ServiceResult<TK>.Success(mapper(Value))
                : ServiceResult<TK>.Failure(Error);
        }

        public ServiceResult<TK> Bind<TK>(Func<T, ServiceResult<TK>> f)
        {
            return IsSuccess ? f(Value) : ServiceResult<TK>.Failure(Error);
        }

        public TK Match<TK>(Func<T, TK> onSuccess, Func<ServiceError, TK> onFailure)
        {
            return IsSuccess ? onSuccess(Value) : onFailure(Error);
        }
    }

}
