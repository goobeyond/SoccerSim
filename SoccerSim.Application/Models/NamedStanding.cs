﻿using SoccerSim.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerSim.Application.Models
{
    public class NamedStanding 
    {
        public string TeamName { get; set; }
        public int GroupId { get; set; }
        public Guid TeamId { get; set; }
        public int Rank { get; set; }
        public int Played { get; set; } = 0;
        public int Win { get; set; } = 0;
        public int Draw { get; set; } = 0;
        public int Loss { get; set; } = 0;
        public int For { get; set; } = 0;
        public int Against { get; set; } = 0;
        public int Diff { get; set; } = 0;
        public int Points { get; set; } = 0;
    }
}
