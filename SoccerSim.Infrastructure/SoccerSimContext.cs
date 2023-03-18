using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoccerSim.Infrastructure.Models;

namespace SoccerSim.Infrastructure
{
    public class SoccerSimContext : DbContext
    {
        public SoccerSimContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=SoccerSim.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Group>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Team>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Group>()
                .HasMany(x => x.Teams);

            modelBuilder.Entity<Group>()
                .HasMany(x => x.Matches);

            modelBuilder.Entity<Group>()
                .HasMany(x => x.Standings);

            modelBuilder.Entity<Standing>()
                .HasIndex(standing => new { standing.GroupId, standing.TeamName });
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Standing> Standings { get; set; }
    }
}
