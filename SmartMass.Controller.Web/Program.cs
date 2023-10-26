using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
using SmartMass.Controller.Web.Data;
using SmartMass.Controller.Web.Services;

namespace SmartMass.Controller.Web
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

            builder.Services.AddHostedService<MqttService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}