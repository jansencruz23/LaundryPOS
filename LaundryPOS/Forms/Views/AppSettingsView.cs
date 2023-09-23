using Guna.UI2.WinForms.Suite;
using LaundryPOS.Contracts;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
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
        private string theme = "#000";

        public AppSettingsView(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
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
    }
}