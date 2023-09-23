using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.Views
{
    public partial class UnpaidForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;

        public UnpaidForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            InitializeComponent();
            InitializeTable();
        }

        private async void InitializeTable()
        {
            var transactionItems = await _unitOfWork.TransactionItemRepo
                .Get(filter: ti => ti.Transaction.IsCompleted == false,
                includeProperties: "Item");

            unpaidTable.DataSource = transactionItems;

            unpaidTable.AutoGenerateColumns = false;

            // Add columns manually
            unpaidTable.Columns.Add("TransactionItemId", "Transaction Item ID");
            unpaidTable.Columns.Add("ItemId", "Item ID");
            unpaidTable.Columns.Add("Quantity", "Quantity");
            unpaidTable.Columns.Add("SubTotal", "SubTotal");

        }

        private async void unpaidTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == unpaidTable.Columns["ItemId"].Index && e.RowIndex >= 0)
            {
                var transactionItem = (TransactionItem)unpaidTable.Rows[e.RowIndex].DataBoundItem;

                // Access the related Item properties directly without a database query
                e.Value = transactionItem.Item.Name; // Assuming Item has a "Name" property
                e.FormattingApplied = true;
            }
        }
    }
}