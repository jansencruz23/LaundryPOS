using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Helpers
{
    public class AuthService
    {
        public void SetPassword(Employee employee, string password)
        {
            employee.Salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            employee.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password, employee.Salt);
        }

        public bool ValidatePassword(Employee employee, string enteredPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, employee.HashedPassword);
        }
    }
}