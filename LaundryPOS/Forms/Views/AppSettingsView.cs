using Guna.UI2.WinForms.Suite;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Services;
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
    public partial class AppSettingsView : UserControl
    {
        private const int FIRST_VALUE = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;
        private string theme = "#000";

        public AppSettingsView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

            InitializeComponent();
            ApplyTheme();
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

        private async void ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnColor);
            await _themeManager.ApplyThemeToButton(btnSave);
        }
    }
}