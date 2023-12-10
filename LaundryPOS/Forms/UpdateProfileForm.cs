using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Helpers;
using LaundryPOS.Models;

namespace LaundryPOS.Forms
{
    public partial class UpdateProfileForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;

        public UpdateProfileForm(IUnitOfWork unitOfWork,
            IStyleManager styleManager, 
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employee = employee;

            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private async void UpdateProfileForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            InitializeProfile();
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnUpdate);
            await _styleManager.Theme.ApplyThemeToPanel(bgPanel, 1f, true);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtFirstName);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtLastName);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtPassword);
        }

        private void InitializeProfile()
        {
            lblName.Text = $"Hello! {_employee.FirstName}";
            txtFirstName.Text = _employee.FirstName;
            txtLastName.Text = _employee.LastName;
            txtPath.Text = _employee.Image ?? string.Empty;
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.Image)
                ? _employee.Image
                : "./default.png");
            dtpBirthday.Value = _employee.BirthDate;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageDialog.Show(this, "Invalid profile update. Please make sure all fields are valid.",
                    "Update Profile Failed", MessageDialogButtons.OK, MessageDialogIcon.Error,
                    MessageDialogStyle.Light);
                return;
            }

            if (!_employee.ValidatePassword(txtPassword.Text))
            {
                MessageDialog.Show(this, "Incorrect password", "Update failed", MessageDialogButtons.OK, 
                    MessageDialogIcon.Error, MessageDialogStyle.Light);
                lblPasswordValidation.Visible = true;
                return;
            }

            UpdateProfile();
            _unitOfWork.EmployeeRepo.Update(_employee);
            await _unitOfWork.SaveAsync();

            MessageDialog.Show(this, "Profile updated successfully.",
                "Profile Updated Successfully", MessageDialogButtons.OK, MessageDialogIcon.Information,
                MessageDialogStyle.Light);
            Close();
        }

        private void UpdateProfile()
        {
            var imagePath = ImageSaveHelper.SaveToImages(txtPath.Text, "Employees");
            _employee.FirstName = txtFirstName.Text;
            _employee.LastName = txtLastName.Text;
            _employee.BirthDate = dtpBirthday.Value;
            _employee.Image = ImageSaveHelper.GetImagePath(imagePath, "Employees");
            _employee.Age = (int)(DateTime.Now.Subtract(dtpBirthday.Value).TotalDays / 365.25);
        }

        private bool ValidateInputs()
        {
            var isValid = true;
            var isFirstNameValid = RegexValidator.IsValidPersonName(txtFirstName.Text);
            var isLastNameValid = RegexValidator.IsValidPersonName(txtLastName.Text);
            var isPasswordValid = RegexValidator.IsValidPassword(txtPassword.Text);

            isValid &= ValidateField(!isFirstNameValid, lblFirstNameValidation);
            isValid &= ValidateField(!isLastNameValid, lblLastNameValidation);
            isValid &= ValidateField(!isPasswordValid, lblPasswordValidation);
            isValid &= ValidateField(string.IsNullOrWhiteSpace(txtPath.Text), lblIconValidation);

            return isValid;
        }

        private bool ValidateField(bool condition, Label label)
        {
            label.Visible = condition;
            return !condition;
        }

        private void imgPic_Click(object sender, EventArgs e)
        {
            using var file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico|All Files|*.*";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = file.FileName;
                imgPic.Image = Image.FromFile(file.FileName);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            Opacity = 0;
            var form = new ChangePassword(_unitOfWork, _styleManager, _employee);
            form.ShowDialog();
            Opacity = 100;
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbShowPassword.Checked
                ? '\0'
                : '•';
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            lblFirstNameValidation.Visible = false;
            lblLastNameValidation.Visible = false;
            lblIconValidation.Visible = false;
            lblPasswordValidation.Visible = false;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            txtFirstName.Focus();
        }
    }
}