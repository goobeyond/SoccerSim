using SoccerSim.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Application.Services
{
    public interface IGroupService
    {
        Task<Group?> GetGroupAsync(int groupId);
        Task<IEnumerable<Standing>> GetStandings(int groupId);
    }
}
