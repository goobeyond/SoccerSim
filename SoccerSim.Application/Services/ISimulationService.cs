﻿using SoccerSim.Application.Models;
using SoccerSim.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Application.Services
{
    public interface ISimulationService
    {
        public Task<MatchResult> OrchestrateGroupMatchSimulation(int groupId, string teamNameA, string teamNameB);
        public Task<MatchResult> OrchestrateSingleMatchSimulation(string homeTeamName, string awayTeamName);
    }
}
