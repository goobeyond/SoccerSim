using SoccerSim.Infrastructure.Models;
using SoccerSim.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository _repository;
        public GroupService(IRepository repository) 
        {
            _repository = repository;
        }

        public async Task<Group?> GetGroupAsync(int groupId)
        {
            return await _repository.GetGroupById(groupId);
        }

        public async Task<IEnumerable<Standing>> GetStandings(int groupId)
        {
            return await _repository.GetRankedStandingsAsync(groupId);
        }

    }
}
