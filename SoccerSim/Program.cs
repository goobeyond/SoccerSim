using Microsoft.EntityFrameworkCore;
using SoccerSim.Domain.Services;
using SoccerSim.Infrastructure;
using SoccerSim.Infrastructure.Repositories;

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

            builder.Services.AddScoped<ITeamService, TeamService>();
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
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}