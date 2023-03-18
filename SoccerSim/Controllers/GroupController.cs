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

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroup(int groupId)
        {
            return Ok(await _teamService.GetGroupAsync(groupId));
        }

        [HttpGet("{groupId}/standings")]
        public async Task<IActionResult> GetStandings(int groupId)
        {
            return Ok(await _teamService.GetStandings(groupId));
        }

        [HttpPost("{groupId}/simulate")]
        public async Task<IActionResult> SimulateMatch(int groupId, string teamNameA, string teamNameB)
        {
            return Ok(await _simService.OrchestrateGroupMatchSimulation(groupId, teamNameA, teamNameB));
        }
    }
}
