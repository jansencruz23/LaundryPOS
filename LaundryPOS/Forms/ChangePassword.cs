using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Managers;

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
            var confirmPassword = txtConfirmPassword.Text;

            var allTextboxesFilled = Controls.OfType<Guna2TextBox>()
                .All(t => !string.IsNullOrEmpty(t.Text));

            if (!allTextboxesFilled)
            {
                MessageDialog.Show(this, "Please fill all the fields.", "Change password failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            if (!_employee.ValidatePassword(oldPassword))
            {
                MessageDialog.Show(this, "Incorrect password", "Change password failed", 
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            if (!newPassword.Equals(confirmPassword))
            {
                MessageDialog.Show(this, "Passwords do not match", "Change password failed", 
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            _employee.SetPassword(newPassword);
            _unitOfWork.EmployeeRepo.Update(_employee);
            await _unitOfWork.SaveAsync();

            MessageDialog.Show(this, "Password updates successfully!", "Password Change Successful",
                    MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
            Close();
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtNewPassword.PasswordChar = txtConfirmPassword.PasswordChar = 
                cbShowPassword.Checked
                    ? '\0'
                    : '•';
        }
    }
}