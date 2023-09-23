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

namespace LaundryPOS.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;

        public LoginForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text;
                var password = txtPassword.Text;

                var employee = await _unitOfWork.EmployeeRepo.GetEmployeeByUsername(username);

                if (IsValidUser(employee, password, out string userRole))
                {
                    Hide();

                    Form form = userRole == "admin"
                        ? new AdminForm(_unitOfWork, _themeManager)
                        : new MainForm(_unitOfWork);
                    form.FormClosed += (s, args) => Close();
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }

                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsValidUser(Employee employee, string password, out string userRole)
        {
            userRole = null!;

            if (employee != null & employee!.ValidatePassword(password))
            {
                userRole = employee.UserRole;
                return true;
            }

            return false;
        }
    }
}