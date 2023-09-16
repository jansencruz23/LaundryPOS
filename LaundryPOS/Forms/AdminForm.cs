using LaundryPOS.Contracts;
using LaundryPOS.Forms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms
{
    public partial class AdminForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminForm(IUnitOfWork unitOfWorK)
        {
            _unitOfWork = unitOfWorK;
            InitializeComponent();
            servicePanel.Controls.Add(new ServiceView(_unitOfWork));
        }
    }
}
