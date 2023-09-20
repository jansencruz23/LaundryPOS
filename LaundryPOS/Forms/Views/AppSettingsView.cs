using Guna.UI2.WinForms.Suite;
using LaundryPOS.Contracts;
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

        public AppSettingsView(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
            InitializeThemes();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse(cbTheme.SelectedValue!.ToString(), out Theme theme))
            {
                var appSettings = await _unitOfWork.AppSettingsRepo.GetByID(FIRST_VALUE);

                if (appSettings != null)
                {
                    UpdateAppSettings(appSettings, theme);
                }
                else
                {
                    appSettings = CreateNewAppSettings(theme);
                    _unitOfWork.AppSettingsRepo.Insert(appSettings);
                }
                
                await _unitOfWork.SaveAsync();
            }
            else
            {
                MessageBox.Show("Invalid theme selection");
            }
        }

        private void InitializeThemes()
        {
            var themes = Enum.GetValues(typeof(Theme)) as Theme[];
            cbTheme.DataSource = themes;
        }

        private AppSettings CreateNewAppSettings(Theme theme) => new()
        {
            Name = txtName.Text,
            Address = txtAddress.Text,
            Description = txtDescription.Text,
            PhoneNumber = txtNumber.Text,
            Email = txtEmail.Text,
            Theme = theme
        };

        private void UpdateAppSettings(AppSettings appSettings, Theme theme)
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