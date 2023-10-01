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
    public partial class UpdateProfileForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;

        public UpdateProfileForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager, 
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;

            InitializeComponent();
        }

        private async void UpdateProfileForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            InitializeProfile();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnUpdate);
            await _themeManager.ApplyThemeToButton(btnChangePassword);
            await _themeManager.ApplyLighterThemeToPanel(bgPanel, 1f, true);
        }

        private void InitializeProfile()
        {
            txtFirstName.Text = _employee.FirstName;
            txtLastName.Text = _employee.LastName;
            txtPath.Text = _employee.PicPath ?? string.Empty;
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.PicPath)
                ? _employee.PicPath
                : "./default.png");
            dtpBirthday.Value = _employee.BirthDate;
            txtUsername.Text = _employee.Username;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!_employee.ValidatePassword(txtPassword.Text))
            {
                MessageBox.Show("Incorrect password");
                return;
            }

            _employee.FirstName = txtFirstName.Text;
            _employee.LastName = txtLastName.Text;
            _employee.BirthDate = dtpBirthday.Value;
            _employee.PicPath = txtPath.Text;
            _employee.Age = DateTime.Now.Year - _employee.BirthDate.Year -
                (DateTime.Now < _employee.BirthDate.AddYears(_employee.Age) 
                    ? 1 
                    : 0);
            _employee.Username = txtUsername.Text;

            _unitOfWork.EmployeeRepo.Update(_employee);
            await _unitOfWork.SaveAsync();
        }
    }
}