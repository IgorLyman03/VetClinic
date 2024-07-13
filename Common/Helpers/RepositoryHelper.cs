using Common.Results;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class RepositoryHelper
    {
        public static async Task<ServiceResult<T>> ExecuteSafeAsync<T>(Func<Task<ServiceResult<T>>> repositoryFunction)
        {
            try
            {
                ServiceResult<T> result = await repositoryFunction();
                return result;
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.Failure( new ServiceError($"Repository error: {ex.Message}", ServiceErrorType.InternalError) );
            }
        }
    }
}
