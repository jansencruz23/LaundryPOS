using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Services;

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
            var allTextboxesFilled = Controls.OfType<Guna2TextBox>()
                .All(t => !string.IsNullOrEmpty(t.Text));

            if (!allTextboxesFilled)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            var employee = new Employee
            {
                Username = txtUsername.Text,
                PicPath = txtPath.Text,
                BirthDate = dtpBirthday.Value,
                Age = (int)(DateTime.Now.Subtract(dtpBirthday.Value).TotalDays / 365.25),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                IsActive = true,
                UserRole = "user"
            };
            employee.SetPassword(txtPassword.Text);

            await CreateEmployee(employee);
        }

        public async Task CreateEmployee(Employee employee)
        {
            _unitOfWork.EmployeeRepo.Insert(employee);
            await _unitOfWork.SaveAsync();
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
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnRegister);
            await _themeManager.ApplyLighterThemeToPanel(bgPanel, 1f, true);
        }
    }
}