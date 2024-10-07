using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger<BaseService> _logger;

        public BaseService(ILogger<BaseService> logger)
        {
            _logger = logger;
        }

        protected async Task<ServiceResult<T>> HandleExceptions<T>(Func<Task<ServiceResult<T>>> action)
        {
            try
            {
                return await action();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while interacting with the database.");
                return ServiceResult<T>.Failure("An error occurred while interacting with the database. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return ServiceResult<T>.Failure("An unexpected error occurred. Please try again.");
            }
        }
    }
}
