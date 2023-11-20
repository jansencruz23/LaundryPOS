using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Managers;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms
{
    public partial class ChangePassword : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;

        public ChangePassword(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employee = employee;

            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }

        private async void ChangePassword_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToPanel(panelBg, 1f);
            await _styleManager.Theme.ApplyThemeToButton(btnSubmit);
            await _styleManager.Theme.ApplyOutlineThemeToButton(btnClose);
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var oldPassword = txtOldPassword.Text;
            var newPassword = txtNewPassword.Text;

            if (!ValidateInputs())
            {
                MessageDialog.Show(this, "Please make sure all the fields are valid.", "Change password failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            if (!_employee.ValidatePassword(oldPassword))
            {
                MessageDialog.Show(this, "Incorrect password", "Change password failed", 
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                lblOldPWValidation.Visible = true;
                return;
            }

            _employee.SetPassword(newPassword);
            _unitOfWork.EmployeeRepo.Update(_employee);
            await _unitOfWork.SaveAsync();

            MessageDialog.Show(this, "Password updates successfully!", "Password Change Successful",
                    MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
            Close();
        }

        private bool ValidateInputs()
        {
            var isValid = true;
            var isPasswordValid = RegexValidator.IsValidPassword(txtNewPassword.Text);

            isValid &= ValidateField(!isPasswordValid, lblNewPWValidation);
            isValid &= ValidateField(!txtConfirmPassword.Text.Equals(txtNewPassword.Text), lblConfirmPWValidation);

            return isValid;
        }
        
        private bool ValidateField(bool condition, Label label)
        {
            label.Visible = condition;
            return !condition;
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.PasswordChar = txtConfirmPassword.PasswordChar = 
                txtOldPassword.PasswordChar =
                cbShowPassword.Checked
                    ? '\0'
                    : '•';
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            lblOldPWValidation.Visible = false;
            lblNewPWValidation.Visible = false;
            lblConfirmPWValidation.Visible = false;
        }
    }
}