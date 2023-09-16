using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Salt { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]

        public string FirstName { get; set; } = "firstname";

        [Required]
        [StringLength(100, MinimumLength = 1)]

        public string LastName { get; set; } = "lastname";

        public int Age { get; set; }

        public string? PicPath { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsActive { get; set; } = true;

        
        public void SetPassword(string password)
        {
            Salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password, Salt);
        }

        public bool ValidatePassword(string enteredPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, HashedPassword);
        }
    }
}