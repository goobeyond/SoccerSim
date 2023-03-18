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

            var homeTeam = group.Teams.First(x => x.Name == homeTeamName);
            var awayTeam = group.Teams.First(x => x.Name == awayTeamName);

            var match = group.Matches.FirstOrDefault(x => ((x.HomeTeam == homeTeam.Id && x.AwayTeam == awayTeam.Id) 
                    || (x.HomeTeam == awayTeam.Id && x.AwayTeam == homeTeam.Id))
                    && x.Draw is null);

            if (match is null)
            {
                throw new InvalidOperationException("Match has already been played.");
            }

            // redo team identification in case the positions were swapped 
            // when finding match.
            homeTeam = group.Teams.First(x => x.Id == match.HomeTeam);
            awayTeam = group.Teams.First(x => x.Id == match.AwayTeam);

            var result = SimulateGame(homeTeam, awayTeam);

            var homeStanding = group.Standings.First(x => x.TeamName == homeTeam.Name);
            var awayStanding = group.Standings.First(x => x.TeamName == awayTeam.Name);
            
            UpdateStandings(homeTeam, awayTeam, result, match, homeStanding, awayStanding);

            await _repository.UpdateGroupAsync(group);
            return result;
        }

        public async Task<MatchResult> OrchestrateSingleMatchSimulation(string homeTeamName, string awayTeamName)
        {
            var teams = await _repository.GetTeamsAsync();
            var homeTeam = teams.First(x => x.Name == homeTeamName);
            var awayTeam = teams.First(x => x.Name == awayTeamName);

            return SimulateGame(homeTeam, awayTeam);
        }

        private void UpdateStandings(Team homeTeam, Team awayTeam, MatchResult result, Match match, Standing homeStanding, Standing awayStanding)
        {
            int homePoints = 0, awayPoints = 0;

            match.HomeScore = result.TeamAScore;
            match.AwayScore = result.TeamBScore;
            match.Draw = false;

            homeStanding.Played++;
            homeStanding.For += result.TeamAScore;
            homeStanding.Against += result.TeamBScore;
            homeStanding.Diff = homeStanding.For - homeStanding.Against;

            awayStanding.Played++;
            awayStanding.For += result.TeamBScore;
            awayStanding.Against += result.TeamAScore;
            awayStanding.Diff = awayStanding.For - awayStanding.Against;


            if (result.TeamAScore > result.TeamBScore)
            {
                match.Winner = homeTeam.Id;
                homeStanding.Win++;
                awayStanding.Loss++;
                homePoints = 3;
            }
            else if (result.TeamAScore < result.TeamBScore)
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
        }

        private MatchResult SimulateGame(Team home, Team away)
        {
            int homeScore = CalculateScore(home, away);
            int awayScore = CalculateScore(away, home);

            return new MatchResult()
            {
                TeamAScore = homeScore,
                TeamBScore = awayScore,
            };
        }

        private int CalculateScore(Team attacker, Team defender)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            int score = 0;
            
            // from a max difference of 10 to -10, we have a range of 20 so we divide by 20
            // to convert the chance to integer, we multiply by 10, so in the end, we divide by 2
            var chanceToScore = (10 + attacker.Att - defender.Def) / 2; 

            for (int i = 0; i < attacker.Mid; i++) // we consider the Mid score as the number of attempts on goal
            {
                var outcome = random.Next(0, 11);
                if (outcome <= chanceToScore)
                {
                    score++;
                }
            }

            return score;
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
