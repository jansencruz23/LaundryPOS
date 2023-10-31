using LaundryPOS.Data;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.DAL
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.AppSettings.Any())
            {
                return;
            }

            var appSettings = new AppSettings
            {
                Address = "eg. 123 Address",
                Email = "eg. name@email.com",
                Name = "Point-of-Sale System",
                PhoneNumber = "eg. 09123456789",
                Theme = "#950426"
            };

            context.AppSettings.Add(appSettings);
            context.SaveChangesAsync();
        }
    }
}
