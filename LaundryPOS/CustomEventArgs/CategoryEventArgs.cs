using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.CustomEventArgs
{
    public class CategoryEventArgs : EventArgs
    {
        public Category Category { get; set; }

        public CategoryEventArgs(Category category)
        {
            Category = category;
        }
    }
}