using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
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
    public partial class ChangePassword : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;

        public ChangePassword(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;

            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }

        private async void ChangePassword_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToPanel(panelBg, 1f);
            await _themeManager.ApplyThemeToButton(btnSubmit);
            await _themeManager.ApplyOutlineThemeToButton(btnClose);
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var oldPassword = txtOldPassword.Text;
            var newPassword = txtNewPassword.Text;
            var confirmPassword = txtConfirmPassword.Text;

            var allTextboxesFilled = Controls.OfType<Guna2TextBox>()
                .All(t => !string.IsNullOrEmpty(t.Text));

            if (!allTextboxesFilled)
            {
                MessageBox.Show("Please fill all the fields.", "Change password failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_employee.ValidatePassword(oldPassword))
            {
                MessageBox.Show("Incorrect password", "Change password failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!newPassword.Equals(confirmPassword))
            {
                MessageBox.Show("Passwords do not match", "Change password failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _employee.SetPassword(newPassword);
            _unitOfWork.EmployeeRepo.Update(_employee);
            await _unitOfWork.SaveAsync();

            MessageBox.Show("Password updates successfully!", "Password Change Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.PasswordChar = txtConfirmPassword.PasswordChar = 
                cbShowPassword.Checked
                    ? '\0'
                    : '•';
        }
    }
}