using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class BaseService
    {
        protected async Task<ServiceResult<T>> HandleExceptions<T>(Func<Task<ServiceResult<T>>> action)
        {
            try
            {
                return await action();
            }
            catch (DbUpdateException dbEx)
            {
                // Log the error
                return ServiceResult<T>.Failure("An error occurred while interacting with the database. Please try again.");
            }
            catch (Exception ex)
            {
                // Log the error
                return ServiceResult<T>.Failure("An unexpected error occurred. Please try again.");
            }
        }
    }
}
