using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        ITokenRepository Token { get; }
    
        
        void Save();

        void SaveAsync();
    }

}
