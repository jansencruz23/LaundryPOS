using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using Guna.UI2.WinForms;
using LaundryPOS.Managers;

namespace LaundryPOS
{
    public partial class RegisterForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;

        public RegisterForm(IUnitOfWork unitOfWork,
            IStyleManager styleManager)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;

            InitializeComponent();
            StyleFonts();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private async void RegisterForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageDialog.Show(this, "Invalid registration. Please make sure all fields are valid.", 
                    "Registration Failed", MessageDialogButtons.OK, MessageDialogIcon.Error,
                    MessageDialogStyle.Light);
                return;
            }

            var employeeIsExisting = await _unitOfWork.EmployeeRepo
                .IsEmployeeExisting(txtUsername.Text);

            if (employeeIsExisting)
            {
                MessageDialog.Show(this, "Username already exists.", "Registration Failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            var employee = CreateEmployee();
            _unitOfWork.EmployeeRepo.Insert(employee);
            await _unitOfWork.SaveAsync();

            Close();
        }

        private bool ValidateInputs()
        {
            var isValid = true;
            var isFirstNameValid = RegexValidator.IsValidPersonName(txtFirstName.Text);
            var isLastNameValid = RegexValidator.IsValidPersonName(txtLastName.Text);
            var isUsernameValid = RegexValidator.IsValidUsername(txtUsername.Text);
            var isPasswordValid = RegexValidator.IsValidPassword(txtPassword.Text);

            isValid &= ValidateField(!isFirstNameValid, lblFirstNameValidation);
            isValid &= ValidateField(!isLastNameValid, lblLastNameValidation);
            isValid &= ValidateField(!isUsernameValid, lblUsernameValidation);
            isValid &= ValidateField(!isPasswordValid, lblPasswordValidation);
            isValid &= ValidateField(!txtConfirmPassword.Text.Equals(txtPassword.Text), lblConfirmPWValidation);
            isValid &= ValidateField(string.IsNullOrWhiteSpace(txtPath.Text), lblIconValidation);

            return isValid;
        }

        private bool ValidateField(bool condition, Label label)
        {
            label.Visible = condition;
            return !condition;
        }

        private Employee CreateEmployee()
        {
            var imagePath = ImageSaveHelper.SaveToImages(txtPath.Text, "Employees");
            var employee = new Employee
            {
                Username = txtUsername.Text,
                Image = ImageSaveHelper.GetImagePath(imagePath, "Employees"),
                BirthDate = dtpBirthday.Value,
                Age = (int)(DateTime.Now.Subtract(dtpBirthday.Value).TotalDays / 365.25),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                IsActive = true,
                UserRole = "user"
            };
            employee.SetPassword(txtPassword.Text);

            return employee;
        }

        private void btnFile_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnRegister);
            await _styleManager.Theme.ApplyThemeToPanel(bgPanel, 1f, true);
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = 
                txtConfirmPassword.PasswordChar = cbShowPassword.Checked
                    ? '\0'
                    : '•';
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void StyleFonts()
        {
            StyleFontsButton(11.25f, btnRegister);
            StyleFontsLabel(18f, true, lblTitle, lblDescription);
            StyleFontsLabel(11.25f, false, lblFirstName, lblLastName,
                lblBirthday, lblUsername, lblPassword, lblConfirmPassword);
            StyleFontsTextBox(11.25f, txtFirstName, txtLastName,
                txtUsername, txtPassword, txtConfirmPassword);
            StyleFontsLabel(8.25f, false, lblFirstNameValidation, lblIconValidation,
                lblLastNameValidation, lblUsernameValidation, lblPasswordValidation,
                lblConfirmPWValidation);
            StyleFontsLabel(9.75f, false, lblSubDescription);
            cbShowPassword.Font = _styleManager.Font.Helvetica(11.25f);
        }

        private void StyleFontsButton(float size, params Guna2Button[] buttons)
        {
            Array.ForEach(buttons, btn
                => btn.Font = _styleManager.Font.HelveticaBold(size));
        }

        private void StyleFontsTextBox(float size, params Guna2TextBox[] textbox)
        {
            Array.ForEach(textbox, txt
                => txt.Font = _styleManager.Font.Helvetica(size));
        }

        private void StyleFontsLabel(float size, bool bold, params Label[] labels)
        {
            if (bold)
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = _styleManager.Font.HelveticaBold(size));
            }
            else
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = _styleManager.Font.Helvetica(size));
            }
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            lblIconValidation.Visible = false;
            lblFirstNameValidation.Visible = false;
            lblLastNameValidation.Visible = false;
            lblUsernameValidation.Visible = false;
            lblPasswordValidation.Visible = false;
            lblConfirmPWValidation.Visible = false;
            lblIconValidation.Visible = false;
        }
    }
}