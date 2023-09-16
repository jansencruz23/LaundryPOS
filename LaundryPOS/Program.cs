using LaundryPOS.Contracts;
using LaundryPOS.DAL;
using LaundryPOS.Data;
using LaundryPOS.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LaundryPOS
{
    internal static class Program
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
                    var CONNECTION_STRING = context.Configuration.GetConnectionString("MySqlServer");
                    //const string CONNECTION_STRING = "SqlServer";
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(CONNECTION_STRING, ServerVersion.AutoDetect(CONNECTION_STRING));
                        //options.UseSqlServer(context.Configuration.GetConnectionString(CONNECTION_STRING));
                    }, ServiceLifetime.Scoped);

                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddScoped<MainForm>();
                }).Build();

            var form = host.Services.GetService<MainForm>();
            Application.Run(form);
        }
    }
}