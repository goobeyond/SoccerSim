using Moq;
using SoccerSim.Application.Services;
using Soccer = SoccerSim.Infrastructure.Models;
using SoccerSim.Infrastructure.Repositories;

namespace SoccerSim.Application.UnitTests.Services
{
    [TestClass]
    public class GroupServiceTests
    {
        private readonly Mock<IRepository> _repoMock;
        private readonly GroupService _groupService;

        private int _groupId = 1;
        private Guid Team1Id = Guid.NewGuid();
        private Guid Team2Id = Guid.NewGuid();
        private Guid Team3Id = Guid.NewGuid();
        private Guid Team4Id = Guid.NewGuid();

        public GroupServiceTests()
        {
            _repoMock = new Mock<IRepository>();
            _groupService = new GroupService(_repoMock.Object);
        }

        [TestMethod]
        public async Task GetStandings_GroupDoesntExist_ThrowsException()
        {
            _repoMock.Setup(x => x.GetRankedStandingsAsync(It.IsAny<int>())).ReturnsAsync(new Dictionary<string, Soccer.Standing>());

            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => _groupService.GetStandingsAsync(0));

            Assert.AreEqual("Group doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async Task GetStandings_NoMatchesPlayed_ReturnsStandings()
        {
            _repoMock.Setup(x => x.GetRankedStandingsAsync(It.IsAny<int>())).ReturnsAsync(GetStandingsNoMatchesPlayed());

            var result = await _groupService.GetStandingsAsync(1);

            Assert.AreNotEqual(result.First().Rank, result.Last().Rank);
        }

        [TestMethod]
        public async Task GetStandings_1Tie_ReturnsStandings()
        {
            _repoMock.Setup(x => x.GetRankedStandingsAsync(It.IsAny<int>())).ReturnsAsync(GetStandings1Tie());

            _repoMock.Setup(x => x.GetMatchResultAsync(Team3Id, Team4Id)).ReturnsAsync(GetMatchResult());
            var result = await _groupService.GetStandingsAsync(1);

            Assert.AreEqual(1, result.First(x => x.TeamId == Team1Id).Rank);
            Assert.AreEqual(2, result.First(x => x.TeamId == Team2Id).Rank);
            Assert.AreEqual(3, result.First(x => x.TeamId == Team3Id).Rank);
            Assert.AreEqual(4, result.First(x => x.TeamId == Team4Id).Rank);
        }

        private Soccer.Match GetMatchResult()
        {
            return new Soccer.Match() { Winner = Team3Id };
        }

        private IDictionary<string, Soccer.Standing> GetStandingsNoMatchesPlayed()
        {
            return new Dictionary<string, Soccer.Standing>()
            {
                { "A", new Soccer.Standing() {GroupId = _groupId, TeamId = Team1Id , Rank = 1} },
                { "B", new Soccer.Standing() {GroupId = _groupId, TeamId = Team2Id , Rank = 1} },
                { "C", new Soccer.Standing() {GroupId = _groupId, TeamId = Team3Id , Rank = 1} },
                { "D", new Soccer.Standing() {GroupId = _groupId, TeamId = Team4Id , Rank = 1} }
            };
        }

        private IDictionary<string, Soccer.Standing> GetStandings1Tie() // simulating what the sql query returns in this scenario
        {
            return new Dictionary<string, Soccer.Standing>()
            {
                { "A", new Soccer.Standing() {GroupId = _groupId, TeamId = Team1Id , Rank = 1} },
                { "B", new Soccer.Standing() {GroupId = _groupId, TeamId = Team2Id , Rank = 2} },
                { "C", new Soccer.Standing() {GroupId = _groupId, TeamId = Team3Id , Rank = 3} },
                { "D", new Soccer.Standing() {GroupId = _groupId, TeamId = Team4Id , Rank = 3} }
            };
        }
    }
}