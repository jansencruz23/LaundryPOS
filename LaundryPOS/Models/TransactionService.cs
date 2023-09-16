using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Models
{
    public class TransactionService
    {
        public int TransactionServiceId { get; set; }

        public Service Service { get; set; }
        
        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }
    }
}