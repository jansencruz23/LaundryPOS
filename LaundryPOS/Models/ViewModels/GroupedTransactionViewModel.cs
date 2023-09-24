using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models.ViewModels
{
    public class GroupedTransactionViewModel
    {
        public int TransactionId { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public Order Order { get; set; }

        public decimal Total { get => Order.Items.Sum(o => o.SubTotal); }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}