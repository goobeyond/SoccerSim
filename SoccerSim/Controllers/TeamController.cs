using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SoccerSim.Application.Services;

namespace SoccerSim.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ISimulationService _simService;

        public TeamController(ITeamService teamService, ISimulationService simService)
        {
            _teamService = teamService;
            _simService = simService;
        }

        /// <summary>
        /// Get All Teams
        /// </summary>
        /// <returns>List of all Teams.</returns>
        [HttpGet("/Teams")]
        public async Task<IActionResult> GetTeams()
        {
            return Ok(await _teamService.GetTeamsAsync());
        }

        /// <summary>
        /// Simulate a match between two teams.
        /// </summary>
        /// <param name="teamNameA">Name of first team.</param>
        /// <param name="teamNameB">Name of second team.</param>
        /// <returns>Match result.</returns>
        /// <remarks>Match result is not stored anywhere.</remarks>
        [HttpPost("/simulate")]
        public async Task<IActionResult> SimulateMatch(string teamNameA, string teamNameB)
        {
            return Ok(await _simService.OrchestrateSingleMatchSimulation(teamNameA, teamNameB));
        }
    }
}
