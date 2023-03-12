using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Infrastructure.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Att { get; set; }
        public int Def { get; set; }
        public int Mid { get; set; }
    }
}
