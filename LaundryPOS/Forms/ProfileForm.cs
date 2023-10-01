using LaundryPOS.Contracts;
using LaundryPOS.Forms.Views;
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
    public partial class ProfileForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;

        public ProfileForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager, Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;

            InitializeComponent();
        }

        private async void ProfileForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            InitializeProfile();
        }

        private void InitializeProfile()
        {
            lblName.Text = _employee.FullName;
            lblUsername.Text = _employee.Username;
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.PicPath)
                ? _employee.PicPath
                : "./default.png");
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnEdit);
            await _themeManager.ApplyLighterThemeToPanel(bgPanel, 1f);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var form = new UpdateProfileForm(_unitOfWork, _themeManager, _employee);
            form.FormClosed += (s, args) => Close();
            form.ShowDialog();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                RestartApplication();
            }
        }

        private void RestartApplication()
        {
            string appPath = Application.ExecutablePath;
            System.Diagnostics.Process.Start(appPath);
            Application.Exit();
        }
    }
}
