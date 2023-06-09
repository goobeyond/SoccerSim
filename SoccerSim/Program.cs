using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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

            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "SoccerSim",
                        Description = "API's to simulate a soccer match!",
                        Version = "v1",
                        TermsOfService = null,
                        Contact = new OpenApiContact
                        {
                            // Check for optional parameters
                        },
                    });
                var filePath = Path.Combine(AppContext.BaseDirectory, "SoccerSim.xml");
                o.IncludeXmlComments(filePath);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.


            app.UseSwagger();
            app.UseSwaggerUI();


            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SoccerSimContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();

                dbContext.Teams.AddRange(new List<Team>()
                {
                    {new Team() { Name = "A", Att = 10, Mid = 4, Def = 5 } },
                    {new Team() { Name = "B", Att = 5, Mid = 6 , Def = 3 } },
                    {new Team() { Name = "C", Att = 1, Mid = 2 , Def = 9 } },
                    {new Team() { Name = "D", Att = 7, Mid = 10, Def = 6 } },
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
                    {new Standing() {TeamId = teams[0].Id, } },
                    {new Standing() {TeamId = teams[1].Id, } },
                    {new Standing() {TeamId = teams[2].Id, } },
                    {new Standing() {TeamId = teams[3].Id, } }
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