using LaundryPOS.Contracts;
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
    public partial class AdminProfileView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminProfileView(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var admin = await _unitOfWork.EmployeeRepo.GetAdmin();

            if (admin != null)
            {
                admin.Username = txtUsername.Text;
                admin.SetPassword(txtPassword.Text);
            }
            else
            {
                MessageBox.Show("Admin is empty");
            }
        }
    }
}