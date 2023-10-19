using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[]? Image { get; set; }

        public int Stock { get; set; }

        // Navigation Property/

        public Category Category { get; set; }
    }
}