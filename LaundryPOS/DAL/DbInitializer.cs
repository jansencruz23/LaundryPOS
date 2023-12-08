using LaundryPOS.Data;
using LaundryPOS.Models;

namespace LaundryPOS.DAL
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.AppSettings.Any() || context.Employees.Any())
            {
                return;
            }

            var appSettings = new AppSettings
            {
                Address = "eg. 123 Address",
                Email = "eg. name@email.com",
                Name = "Point-of-Sale System",
                PhoneNumber = "eg. 09123456789",
                Theme = "#950426",
                Image = @"Icons\pos.png"
            };

            context.AppSettings.Add(appSettings);
            context.SaveChangesAsync();

            var adminProfile = new Employee
            {
                FirstName = "Admin",
                LastName = "Admin",
                Age = 0,
                BirthDate = DateTime.Now,
                IsActive = true,
                Username = "admin",
                UserRole = "admin"
            };

            adminProfile.SetPassword("admin");

            context.Employees.Add(adminProfile);
            context.SaveChangesAsync();
        }
    }
}
