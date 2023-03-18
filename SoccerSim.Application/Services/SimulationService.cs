using SoccerSim.Application.Dtos;
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
    public class SimulationService : ISimulationService
    {
        private readonly IRepository _repository;
        public SimulationService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<MatchResult> OrchestrateGroupMatchSimulation(int groupId, string homeTeamName, string awayTeamName)
        {
            var group = await GetGroup(groupId);

            int homePoints = 0, awayPoints = 0;

            var homeTeam = group.Teams.First(x => x.Name == homeTeamName);
            var awayTeam = group.Teams.First(x => x.Name == awayTeamName);

            var result = SimulateGame(homeTeam, awayTeam);

            var match = group.Matches.FirstOrDefault(x => x.HomeTeam == homeTeam.Id && x.AwayTeam == awayTeam.Id && x.Draw is null);

            if (match is null)
            {
                match = group.Matches.FirstOrDefault(x => x.HomeTeam == awayTeam.Id && x.AwayTeam == homeTeam.Id && x.Draw is null);
            }

            if (match is null)
            {
                throw new Exception("Either one of the teams doesn't exist or match has already been played.");
            }
            
            var homeStanding = group.Standings.First(x => x.TeamName == homeTeam.Name);
            var awayStanding = group.Standings.First(x => x.TeamName == awayTeam.Name);

            match.HomeScore = result.HomeScore;
            match.AwayScore = result.AwayScore;
            match.Draw = false;

            homeStanding.Played++;
            homeStanding.For += result.HomeScore;
            homeStanding.Against += result.AwayScore;
            homeStanding.Diff = homeStanding.For - homeStanding.Against;

            awayStanding.Played++;
            awayStanding.For += result.HomeScore;
            awayStanding.Against += result.AwayScore;
            awayStanding.Diff = awayStanding.For - awayStanding.Against;


            if (result.HomeScore > result.AwayScore)
            {
                match.Winner = homeTeam.Id;
                homeStanding.Win++;
                awayStanding.Loss++;
                homePoints = 3;
            }
            else if (result.HomeScore < result.AwayScore)
            {
                match.Winner = awayTeam.Id;
                awayStanding.Win++;
                homeStanding.Loss++;
                awayPoints = 3;
            }
            else
            {
                awayStanding.Draw++;
                homeStanding.Draw++;
                match.Draw = true;
                homePoints = 1;
                awayPoints = 1;
            }

            homeStanding.Points += homePoints;
            awayStanding.Points += awayPoints;

            await _repository.UpdateGroupAsync(group);
            return result;
        }


        public MatchResult SimulateGame(Team home, Team away)
        {
            Random rand = new Random();
            var value = rand.Next(0, 100);

            if (value %2 == 0)
            {
                return new MatchResult()
                {
                    HomeScore = 1,
                    AwayScore = 1,
                };
            }
            else
            {
                return new MatchResult()
                {
                    HomeScore = 3,
                    AwayScore = 1,
                };
            }
        }

        private async Task<Group> GetGroup(int groupId)
        {
            var group = await _repository.GetGroupById(groupId);

            if (group == null)
            {
                throw new Exception("Group doesn't exist");
            }

            return group;
        }
    }
}
