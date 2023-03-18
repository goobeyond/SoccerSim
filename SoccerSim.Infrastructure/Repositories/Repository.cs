using Microsoft.EntityFrameworkCore;
using SoccerSim.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private SoccerSimContext _context;

        public Repository(SoccerSimContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _context.Teams.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<Standing>> GetStandings(int groupId)
        {
           //var query = new StringBuilder();
           return new List<Standing>();
            
        }
    }
}
