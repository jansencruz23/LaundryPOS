using LaundryPOS.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class Order
    {
        public List<CartItem> Items { get; set; }

        public Order()
        {
            Items = new();
        }
    }
}