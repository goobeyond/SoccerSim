using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Infrastructure.Models
{
    public class Standing
    {
        public Guid Id { get; set; }
        public int GroupId { get; set; }
        public string TeamName { get; set; }
        public int Rank { get; set; }
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Loss { get; set; }
        public int For { get; set; }
        public int Against { get; set; }
        public int Diff { get; set; }
        public int Points { get; set; }
    }
}
