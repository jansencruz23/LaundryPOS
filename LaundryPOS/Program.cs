using LaundryPOS.Contracts;
using LaundryPOS.DAL;
using LaundryPOS.Data;
using LaundryPOS.Forms;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LaundryPOS
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            ApplicationConfiguration.Initialize();

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //var CONNECTION_STRING = context.Configuration.GetConnectionString("MySqlServer");
                    const string CONNECTION_STRING = "SqlServer";
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        //options.UseMySql(CONNECTION_STRING, ServerVersion.AutoDetect(CONNECTION_STRING));
                        options.UseSqlServer(context.Configuration.GetConnectionString(CONNECTION_STRING));
                    }, ServiceLifetime.Scoped);

                    services.AddMemoryCache();
                    services.AddScoped<ThemeManager>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddScoped<LoginForm>();
                }).Build();

            //var context = host.Services.GetRequiredService<ApplicationDbContext>();
            //context.Database.EnsureCreated();
            //DbInitializer.Initialize(context);

            var form = host.Services.GetService<LoginForm>();
            Application.Run(form);
        }
    }
}