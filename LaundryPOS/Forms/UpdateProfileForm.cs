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
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private async void UpdateProfileForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            InitializeProfile();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnUpdate);
            await _themeManager.ApplyThemeToPanel(bgPanel, 1f, true);
            await _themeManager.ApplyFocusedThemeToTextBox(txtFirstName);
            await _themeManager.ApplyFocusedThemeToTextBox(txtLastName);
            await _themeManager.ApplyFocusedThemeToTextBox(txtPassword);
        }

        private void InitializeProfile()
        {
            lblName.Text = $"Hello! {_employee.FirstName}";
            txtFirstName.Text = _employee.FirstName;
            txtLastName.Text = _employee.LastName;
            txtPath.Text = _employee.Image ?? string.Empty;
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.Image)
                ? _employee.Image
                : "./default.png");
            dtpBirthday.Value = _employee.BirthDate;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!_employee.ValidatePassword(txtPassword.Text))
            {
                MessageBox.Show("Incorrect password", "Update failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _employee.FirstName = txtFirstName.Text;
            _employee.LastName = txtLastName.Text;
            _employee.BirthDate = dtpBirthday.Value;
            _employee.Image = txtPath.Text;
            _employee.Age = (int)(DateTime.Now.Subtract(dtpBirthday.Value).TotalDays / 365.25);

            _unitOfWork.EmployeeRepo.Update(_employee);
            await _unitOfWork.SaveAsync();
        }

        private void imgPic_Click(object sender, EventArgs e)
        {
            using var file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico|All Files|*.*";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = file.FileName;
                imgPic.Image = Image.FromFile(file.FileName);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            Opacity = 0;
            var form = new ChangePassword(_unitOfWork, _themeManager, _employee);
            form.ShowDialog();
            Opacity = 100;
        }
    }
}