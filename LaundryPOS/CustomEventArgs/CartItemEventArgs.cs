using LaundryPOS.Forms.Views;
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
        public CartItem CartItem { get; set; }

        public CartItemEventArgs(CartItem cartItem)
        {
            CartItem = cartItem;
        }
    }
}