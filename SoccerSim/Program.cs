using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoccerSim.Application.Services;
using SoccerSim.Infrastructure;
using SoccerSim.Infrastructure.Models;
using SoccerSim.Infrastructure.Repositories;
using System.Reflection;

namespace SoccerSim
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<SoccerSimContext>(options =>
               options
                .UseSqlite(@"Data Source=SoccerSim.db")
               );

            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<ISimulationService, SimulationService>();
            builder.Services.AddScoped<IRepository, Repository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SoccerSimContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();

                dbContext.Teams.AddRange(new List<Team>()
                {
                    {new Team() { Name = "A", Att = 10, Def = 5, Mid = 4 } },
                    {new Team() { Name = "B", Att = 5, Def = 3, Mid = 6 } },
                    {new Team() { Name = "C", Att = 1, Def = 9, Mid = 2 } },
                    {new Team() { Name = "D", Att = 7, Def = 6, Mid = 10 } },
                });

                dbContext.SaveChanges();

                var teams = dbContext.Teams.ToArray();

                var group = new Group();

                group.Teams = teams;

                group.Matches = new List<Match>()
                {
                    { new Match() { HomeTeam = teams[0].Id, AwayTeam = teams[1].Id } },
                    { new Match() { HomeTeam = teams[2].Id, AwayTeam = teams[3].Id } },
                    { new Match() { HomeTeam = teams[0].Id, AwayTeam = teams[2].Id } },
                    { new Match() { HomeTeam = teams[1].Id, AwayTeam = teams[3].Id } },
                    { new Match() { HomeTeam = teams[0].Id, AwayTeam = teams[3].Id } },
                    { new Match() { HomeTeam = teams[1].Id, AwayTeam = teams[2].Id } },
                };

                group.Standings = new List<Standing>()
                {
                    {new Standing() {TeamName = teams[0].Name, } },
                    {new Standing() {TeamName = teams[1].Name, } },
                    {new Standing() {TeamName = teams[2].Name, } },
                    {new Standing() {TeamName = teams[3].Name, } }
                };

                dbContext.Add(group);

                dbContext.SaveChanges();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}