using LaundryPOS.Contracts;
using LaundryPOS.Models;
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
                var appSettings = new AppSettings
                {
                    Name = txtName.Text,
                    Address = txtAddress.Text,
                    Description = txtDescription.Text,
                    PhoneNumber = txtNumber.Text,
                    Email = txtEmail.Text,
                    Theme = theme
                };

                _unitOfWork.AppSettingsRepo.Insert(appSettings);
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
    }
}
