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

namespace LaundryPOS.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginForm(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;
            var employee = await _unitOfWork.EmployeeRepo.GetEmployeeByUsername(username);

            if (employee != null && employee.ValidatePassword(password))
            {
                MessageBox.Show("Sucesful");
            }
            else
            {
                MessageBox.Show("wong");
            }
        }
    }
}