using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public Employee Employee { get; set; }

        public List<TransactionItem> Services { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsCompleted { get; set; }
    }
}