using LaundryPOS.Contracts;
using LaundryPOS.DAL;
using LaundryPOS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
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
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]
                .ConnectionString;
            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .BuildServiceProvider();

            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(unitOfWork));
        }
    }
}