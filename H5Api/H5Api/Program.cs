using H5Api.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace H5Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddAuthorization();

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("allowpaired", builder =>
                {
                    builder
                        .WithOrigins(
                            "http://h5.lymim.com",
                            "http://localhost:5272")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            #endregion

            #region Sticky

            var stickyConnectionString = builder.Configuration["ConnectionStrings:stickyConnection"];
            builder.Services.AddDbContext<StickyContext>(optionsBuilder =>
                optionsBuilder.UseMySql(stickyConnectionString, ServerVersion.AutoDetect(stickyConnectionString)));

            builder.Services.AddDataProtection();

            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            });

            app.MapControllers();
            app.UseCors();

            app.Run();
        }
    }
}