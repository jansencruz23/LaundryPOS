using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string? PicPath { get; set; }
    }
}