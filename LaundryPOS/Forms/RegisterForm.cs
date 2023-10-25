using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;

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
        }

        private async void RegisterForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Please fill all the fields including image.", "Registration failed",
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

        private bool ValidateInput()
            => !(string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPath.Text));

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
    }
}