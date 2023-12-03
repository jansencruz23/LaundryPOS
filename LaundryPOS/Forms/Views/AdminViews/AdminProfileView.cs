using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using LaundryPOS.Managers;

namespace LaundryPOS.Forms.Views
{
    public partial class AdminProfileView : BaseViewControl
    {
        public AdminProfileView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, styleManager, changeAdminView)
        {
            InitializeComponent();
            StyleFonts();
        }

        private async void AdminProfileView_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var admin = await _unitOfWork.EmployeeRepo.GetAdmin();

            if (admin == null)
            {
                MessageDialog.Show(ParentForm, "Admin is empty", "Error Occurred",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            if (!ValidateInputs())
            {
                MessageDialog.Show("Invalid admin update. Please make sure all fields are valid.",
                    "Admin Update Failed", MessageDialogButtons.OK, MessageDialogIcon.Error,
                    MessageDialogStyle.Light);
                return;
            }

            if (!admin.ValidatePassword(txtOldPassword.Text))
            {
                MessageDialog.Show(ParentForm, "Old admin password is incorrect.", "Admin Update Failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                lblOldPWValidation.Visible = true;
                return;
            }

            admin.Username = txtUsername.Text;
            admin.SetPassword(txtNewPassword.Text);

            _unitOfWork.EmployeeRepo.Update(admin);
            await _unitOfWork.SaveAsync();

            MessageDialog.Show(ParentForm, "Admin successfully updated!", "Admin Update Successfully",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
        }

        private bool ValidateInputs()
        {
            var isValid = true;
            var isUsernameValid = RegexValidator.IsValidUsername(txtUsername.Text);
            var isPasswordValid = RegexValidator.IsValidPassword(txtNewPassword.Text);

            isValid &= ValidateField(!isUsernameValid, lblUsernameValidation);
            isValid &= ValidateField(!isPasswordValid, lblNewPWValidation);
            isValid &= ValidateField(!txtConfirmPassword.Text.Equals(txtNewPassword.Text), lblConfirmPWValidation);

            return isValid;
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnAdminProfile);
            await _styleManager.Theme.ApplyThemeToButton(btnSave);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ChangeDashboardView();
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<CategoryView>());
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<EmployeeView>());
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<TransactionView>());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ConfirmLogout();
        }

        private void StyleFonts()
        {
            _styleManager.Font.StyleFontsButton(11.25f, btnItem, btnCategory, btnEmployee, btnEmployee,
                btnAdminProfile, btnSave, btnLogout);
            _styleManager.Font.StyleFontsLabel(18f, true, lblTitle);
            _styleManager.Font.StyleFontsLabel(11.25f, false, lblUsername, lblOldPassword, 
                lblNewPassword, lblConfirmPW);
            _styleManager.Font.StyleFontsTextBox(11.25f, txtUsername, txtOldPassword,
                txtNewPassword, txtConfirmPassword);
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            HideValidationError(lblUsernameValidation, lblOldPWValidation,
                lblNewPWValidation, lblConfirmPWValidation);
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtOldPassword.PasswordChar =
            txtNewPassword.PasswordChar =
            txtConfirmPassword.PasswordChar = 
                cbShowPassword.Checked
                    ? '\0'
                    : '•';
        }
    }
}