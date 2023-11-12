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
                MessageBox.Show("Admin is empty", "Error Occurred",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidateInputs())
            {
                MessageBox.Show("Invalid admin update. Please make sure all fields are valid.", "Admin Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!admin.ValidatePassword(txtOldPassword.Text))
            {
                MessageBox.Show("Old admin password is incorrect.", "Admin Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            admin.Username = txtUsername.Text;
            admin.SetPassword(txtNewPassword.Text);

            _unitOfWork.EmployeeRepo.Update(admin);
            await _unitOfWork.SaveAsync();

            MessageBox.Show("Admin successfully updated!", "Admin Update Successfully",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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