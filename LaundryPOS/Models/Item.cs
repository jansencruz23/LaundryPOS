using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string? PicPath { get; set; }

        public int Stock { get; set; }
    }
}