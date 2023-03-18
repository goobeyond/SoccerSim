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

        public async Task<Group?> GetGroupById(int groupId)
        {
            return await _context.Groups
                .Include(x => x.Teams)
                .Include(x => x.Matches)
                .Include(x => x.Standings)
                .FirstOrDefaultAsync(x => x.Id == groupId);
        }

        public async Task UpdateGroupAsync(Group group)
        {
            _context.Update(group);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Standing>> GetRankedStandingsAsync(int groupId)
        {
            FormattableString sqlraw = @$"SELECT Id, GroupId, TeamName, Played, Win, Draw, Loss, For, Against, Diff, Points, 
                                            RANK() OVER (ORDER BY Points DESC, Diff DESC, `For` DESC, Against ASC) AS Rank 
                                        FROM Standings WHERE GroupId = {groupId};";


            var result = _context.Standings.FromSql(sqlraw).ToList();

            return result;
        }
    }
}
