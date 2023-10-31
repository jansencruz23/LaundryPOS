using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms.Views
{
    public partial class AppSettingsView : BaseViewControl
    {
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
            var appDataList = await _unitOfWork.AppSettingsRepo.Get();
            var appData = appDataList.FirstOrDefault();

            txtName.Text = appData.Name;
            txtAddress.Text = appData.Address;
            txtNumber.Text = appData.PhoneNumber;
            txtEmail.Text = appData.Email;
            txtDescription.Text = appData.Description ?? string.Empty;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var appDataList = await _unitOfWork.AppSettingsRepo.Get();
            var appSettings = appDataList.FirstOrDefault();

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

            MessageBox.Show("Restart application to see results. Press OK to restart the application", 
                "App Restarting", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            await _themeManager.ApplyThemeToButton(btnSave);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }
    }
}