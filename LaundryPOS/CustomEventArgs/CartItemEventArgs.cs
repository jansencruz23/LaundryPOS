using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.CustomEventArgs
{
    public class CartItemEventArgs : EventArgs
    {
        public Service Service { get; set; }
        public int Quantity { get; set; }

        public CartItemEventArgs(Service service, int quantity)
        {
            Service = service;
            Quantity = quantity;
        }
    }
}