using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Xml.Linq;
using Guna.UI2.WinForms;
using System.Text.RegularExpressions;
using LaundryPOS.Migrations;

namespace LaundryPOS
{
    public partial class RegisterForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;

        public RegisterForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;

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
                MessageBox.Show("Invalid registration. Please make sure all fields are valid.", "Registration failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!txtConfirmPassword.Text.Equals(txtPassword.Text))
            {
                MessageBox.Show("Passwords do not match", "Registration failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var employeeIsExisting = await _unitOfWork.EmployeeRepo
                .IsEmployeeExisting(txtUsername.Text);

            if (employeeIsExisting)
            {
                MessageBox.Show("Username already exists.", "Registration failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var imagePath = SaveToImages(txtPath.Text);
            var employee = new Employee
            {
                Username = txtUsername.Text,
                Image = GetImagePath(imagePath),
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

        private string GetImagePath(string imagePath)
            => Path.Combine("Icons", "Images", "Employees", Path.GetFileName(imagePath));

        private string SaveToImages(string imagePath)
        {
            string uniqueName = $"{Guid.NewGuid().ToString()[^8..]}_" +
                $"{Path.GetFileName(imagePath)[Math.Max(0, Path.GetFileName(imagePath).Length - 10)..]}";
            string destinationPath = Path.Combine(Application.StartupPath, "Icons", "Images", "Employees", uniqueName);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            File.Copy(imagePath, destinationPath);

            return uniqueName;
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
            await _themeManager.ApplyThemeToButton(btnRegister);
            await _themeManager.ApplyThemeToPanel(bgPanel, 1f, true);
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
            cbShowPassword.Font = _themeManager.Helvetica(11.25f);
        }

        private void StyleFontsButton(float size, params Guna2Button[] buttons)
        {
            Array.ForEach(buttons, btn
                => btn.Font = _themeManager.HelveticaBold(size));
        }

        private void StyleFontsTextBox(float size, params Guna2TextBox[] textbox)
        {
            Array.ForEach(textbox, txt
                => txt.Font = _themeManager.Helvetica(size));
        }

        private void StyleFontsLabel(float size, bool bold, params Label[] labels)
        {
            if (bold)
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = _themeManager.HelveticaBold(size));
            }
            else
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = _themeManager.Helvetica(size));
            }
        }
    }
}