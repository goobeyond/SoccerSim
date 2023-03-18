using SoccerSim.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Application.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
    }
}
