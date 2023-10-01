using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Services;

namespace LaundryPOS
{
    public partial class RegisterForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;

        public RegisterForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;

            InitializeComponent();
        }

        public RegisterForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;

            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            var employee = new Employee();
            employee.Username = txtUsername.Text;
            employee.PicPath = txtPath.Text;
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
    }
}