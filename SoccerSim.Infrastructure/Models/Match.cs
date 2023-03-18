using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Infrastructure.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public Guid HomeTeam { get; set; }
        public Guid AwayTeam { get; set; }
        public Guid? Winner { get; set; }
        public bool? Draw { get; set; }
        public int HomeScore { get; set; } = 0;
        public int AwayScore { get; set; } = 0;
    }
}
