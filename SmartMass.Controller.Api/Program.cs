using Microsoft.EntityFrameworkCore;
using MQTTnet;
using SmartMass.Controller.Api.Data;
using SmartMass.Controller.Api.Services;
using System.Reflection;
using Microsoft.AspNetCore.ResponseCompression;
using SmartMass.Controller.Api.Hubs;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartMass.Controller.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables().Build();

            builder.Services.AddDbContext<SmartMassDbContext>(options =>
            {
                var s = builder.Configuration.GetConnectionString("smartmassdb");
                options.UseSqlite(s);
            });

            builder.Services.AddTransient<MqttFactory>();
            builder.Services.AddSingleton<Mqtt.IMqttClient, Mqtt.MqttClient>();

            builder.Services.AddSingleton<IDiscoveredDevices, DiscoveredDevices>();
            builder.Services.AddHostedService<MqttService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR();
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            var app = builder.Build();

            app.UseResponseCompression();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapHub<MessageHub>("/messages");
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}