using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class AppSettings
    {
        public int AppSettingsId { get; set; }
        
        public string Name { get; set; }

        public string Theme { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }
    }
}