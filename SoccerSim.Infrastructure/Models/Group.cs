using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Infrastructure.Models
{
    public class Group
    {
        public int Id { get; set; }

        public Guid Team1 { get; set; }
        public Guid Team2 { get; set; }
        public Guid Team3 { get; set; }
        public Guid Team4 { get; set; }

        public IEnumerable<Match> Matches { get; set; }

        public IEnumerable<Standing> Standings { get; set; }

    }
}
