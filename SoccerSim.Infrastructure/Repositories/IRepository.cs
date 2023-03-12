using SoccerSim.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Infrastructure.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
    }
}
