using LaundryPOS.Contracts;
using LaundryPOS.Models;

namespace LaundryPOS
{
    public partial class RegisterForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterForm(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}