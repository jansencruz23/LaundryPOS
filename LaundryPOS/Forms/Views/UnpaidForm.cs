using LaundryPOS.Contracts;
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
        }

        private async void InitializeTable()
        {
            //var unpaidTransaction = await _unitOfWork.TransactionRepo

            //unpaidTable.DataSource
        }
    }
}
