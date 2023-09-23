using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class TransactionItem
    {
        public int TransactionItemId { get; set; }

        public int TransactionId { get; set; }

        public int ItemId { get; set; }
        
        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }

        // Navigation Property
        public Transaction Transaction { get; set; }

        public Item Item { get; set; }
    }
}