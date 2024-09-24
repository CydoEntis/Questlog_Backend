using Questlog.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class MainQuestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MainQuestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Task<MainQuestDTO> GetMainQuest(int id)
        {

        }
    }
}
