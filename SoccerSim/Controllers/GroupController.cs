using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SoccerSim.Application.Services;

namespace SoccerSim.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _teamService;
        private readonly ISimulationService _simService;

        public GroupController(IGroupService teamService, ISimulationService simService)
        {
            _teamService = teamService;
            _simService = simService;
        }

        /// <summary>
        /// Get a Group with all its details.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>The group.</returns>
        /// <remarks>The standings are not ordered correctly here, use standings endpoint for correct ranking.</remarks>
        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroup(int groupId)
        {
            return Ok(await _teamService.GetGroupAsync(groupId));
        }

        /// <summary>
        /// Get standings for a particular group.
        /// </summary>
        /// <param name="groupId">Group Id to get ranking for.</param>
        /// <returns></returns>
        [HttpGet("{groupId}/standings")]
        public async Task<IActionResult> GetStandings(int groupId)
        {
            return Ok(await _teamService.GetStandings(groupId));
        }

        /// <summary>
        /// Simlate a match between two teams is a Group.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="teamNameA"></param>
        /// <param name="teamNameB"></param>
        /// <returns>The match result.</returns>
        /// <remarks>Match result is stored and impacts the standing. Each match can be simulated only once.</remarks>
        [HttpPost("{groupId}/simulate")]
        public async Task<IActionResult> SimulateMatch(int groupId, string teamNameA, string teamNameB)
        {
            return Ok(await _simService.OrchestrateGroupMatchSimulation(groupId, teamNameA, teamNameB));
        }
    }
}
