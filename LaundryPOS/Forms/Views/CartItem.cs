using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Forms.Views
{
    public class CartItem
    {
        public Item Item { get; }

        public int Quantity { get; set; }

        public double SubTotal { get => Item.Price * Quantity; }

        public CartItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}