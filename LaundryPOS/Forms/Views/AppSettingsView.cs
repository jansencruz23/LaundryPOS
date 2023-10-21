using Guna.UI2.WinForms.Suite;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using Microsoft.Reporting.Map.WebForms.BingMaps;
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
    public partial class AppSettingsView : BaseViewControl
    {
        private const int FIRST_VALUE = 1;
        private string theme = "#000";

        public AppSettingsView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, themeManager, changeAdminView)
        {
            InitializeComponent();
        }

        private async void AppSettingsView_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            await FillData();
        }

        private async Task FillData()
        {
            var appData = await _unitOfWork.AppSettingsRepo.GetByID(FIRST_VALUE);

            txtName.Text = appData.Name;
            txtAddress.Text = appData.Address;
            txtNumber.Text = appData.PhoneNumber;
            txtEmail.Text = appData.Email;
            txtDescription.Text = appData.Description ?? string.Empty;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var appSettings = await _unitOfWork.AppSettingsRepo.GetByID(FIRST_VALUE);

            if (appSettings != null)
            {
                UpdateAppSettings(appSettings);
            }
            else
            {
                appSettings = CreateNewAppSettings();
                _unitOfWork.AppSettingsRepo.Insert(appSettings);
            }

            await _unitOfWork.SaveAsync();

            MessageBox.Show("Restart application to see results");
            RestartApplication();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;
                theme = ColorTranslator.ToHtml(selectedColor);
            }
        }

        private AppSettings CreateNewAppSettings() => new()
        {
            Name = txtName.Text,
            Address = txtAddress.Text,
            Description = txtDescription.Text,
            PhoneNumber = txtNumber.Text,
            Email = txtEmail.Text,
            Theme = theme
        };

        private void UpdateAppSettings(AppSettings appSettings)
        {
            appSettings.Name = txtName.Text;
            appSettings.Address = txtAddress.Text;
            appSettings.Description = txtDescription.Text;
            appSettings.PhoneNumber = txtNumber.Text;
            appSettings.Email = txtEmail.Text;
            appSettings.Theme = theme;
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyOutlineThemeToButton(btnColor);
            await _themeManager.ApplyOutlineThemeToButton(btnCancel);
            await _themeManager.ApplyThemeToButton(btnSave);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }
    }
}