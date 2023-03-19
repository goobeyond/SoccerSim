using SoccerSim.Application.Models;
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

        public async Task<IEnumerable<NamedStanding>> GetStandingsAsync(int groupId)
        {
            var teamStandings = await _repository.GetRankedStandingsAsync(groupId);

            if (teamStandings.Count == 0)
            {
                throw new Exception("Group doesn't exist.");
            }

            var namedStandings = teamStandings.Select(x =>
                                                new NamedStanding()
                                                {
                                                    TeamName = x.Key,
                                                    Against = x.Value.Against,
                                                    Diff = x.Value.Diff,
                                                    Draw = x.Value.Draw,
                                                    For = x.Value.For,
                                                    GroupId = x.Value.GroupId,
                                                    Loss = x.Value.Loss,
                                                    Played = x.Value.Played,
                                                    Points = x.Value.Points,
                                                    Rank = x.Value.Rank,
                                                    TeamId = x.Value.TeamId,
                                                    Win = x.Value.Win,
                                                })
                                            .ToList();

            var groupsWithSameRank = namedStandings
                        .GroupBy(x => x.Rank)
                        .Where(x => x.Count() > 1);

            if (AllTeamsHaveSameRank(groupsWithSameRank))
            {
                for (int i = 0; i < 4; i++)
                {
                    namedStandings.ElementAt(i).Rank += i;
                }

                return namedStandings.OrderBy(x => x.Rank);
            }

            foreach (var group in groupsWithSameRank)
            {
                var teams = group.Select(x => x.TeamId).ToArray();

                var headToHeadResult = await _repository.GetMatchResultAsync(teams[0], teams[1]);

                if (headToHeadResult is not null && headToHeadResult.Winner.HasValue)
                {
                    var loser = group.First(x => x.TeamId != headToHeadResult.Winner);
                    namedStandings.First(x => x.TeamId == loser.TeamId).Rank++;
                }
                else
                {
                    namedStandings.First(x => x.TeamId == teams[0]).Rank++; // if the teams have not played or they had a draw, just add a rank to one of them randomly.
                }
            }

            return namedStandings.OrderBy(x => x.Rank);
        }

        private bool AllTeamsHaveSameRank(IEnumerable<IGrouping<int, NamedStanding>> sameRanks)
        {
            return sameRanks.Count() == 1 && sameRanks.First().Count() == 4;
        }
    }
}
