using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Forms.Views.BaseViews;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views
{
    public partial class AppSettingsView : BaseAppSettingsView
    {
        private string theme = "#000";

        public AppSettingsView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, styleManager, changeAdminView)
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
            imgPic.Image = GetImage(appData);
            theme = appData.Theme;
            
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

            MessageDialog.Show(ParentForm, "Restart application to see results. Press OK to restart the application", 
                "App Restarting", MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
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
            Theme = theme,
            Image = GetImagePath(SaveToImages(lblPath.Text, "Logo"), "Logo") 
        };

        private void UpdateAppSettings(AppSettings appSettings)
        {
            appSettings.Name = txtName.Text;
            appSettings.Address = txtAddress.Text;
            appSettings.Description = txtDescription.Text;
            appSettings.PhoneNumber = txtNumber.Text;
            appSettings.Email = txtEmail.Text;
            appSettings.Theme = theme;
            appSettings.Image = GetImagePath(SaveToImages(lblPath.Text, "Logo"), "Logo");

            _unitOfWork.AppSettingsRepo.Update(appSettings);
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyOutlineThemeToButton(btnColor);
            await _styleManager.Theme.ApplyThemeToButton(btnSave);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeDashboardView();
        }

        private void imgPic_Click(object sender, EventArgs e)
        {
            using var file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico|All Files|*.*";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == DialogResult.OK)
            {
                lblPath.Text = file.FileName;
                imgPic.Image = Image.FromFile(file.FileName);
            }
        }
    }
}