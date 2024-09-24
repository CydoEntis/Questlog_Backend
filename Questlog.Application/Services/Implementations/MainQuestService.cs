using AutoMapper;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Implementations
{
    public class MainQuestService : IMainQuestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MainQuestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<MainQuest> GetMainQuest(string mainQuestId)
        {
            var mainQuest = await _unitOfWork.MainQuest.GetAsync(mainQuest => mainQuest.Id == mainQuestId);
            return mainQuest;
        }
    }
}
