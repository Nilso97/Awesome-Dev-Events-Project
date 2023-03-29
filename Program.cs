using System.Reflection;
using Dev_Events_App.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

            builder.Services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DevEventsApp.API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Nilso Junior",
                        Email = "nilsojunior90@gmail.com",
                        Url = new Uri("https://github.com/Nilso97")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

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
