using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
using SmartMass.Controller.Web.Data;

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

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // connect mqtt client
            var mqtt = app.Services.GetService<Mqtt.IMqttClient>();
            mqtt?.Connect(app.Configuration.GetValue<string>("mqtt:host"), app.Configuration.GetValue<string>("mqtt:clientid"),
                app.Configuration.GetValue<string>("mqtt:user"), app.Configuration.GetValue<string>("mqtt:password")).GetAwaiter().GetResult();

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