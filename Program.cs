using Dev_Events_App.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dev_Events_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder
                .Configuration
                .GetConnectionString("DevEventsCs");

            builder.Services.AddDbContext<DevEventsDbContext>(
                d => d.UseSqlite(connectionString)
            );

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
