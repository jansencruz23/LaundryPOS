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

        public int EmployeeId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Change { get; set; }

        public bool IsCompleted { get; set; }

        // Navigation Properties
        public Employee Employee { get; set; }

        public ICollection<TransactionItem>? Items { get; set; }
    }
}