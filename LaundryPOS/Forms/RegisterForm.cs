using LaundryPOS.Contracts;
using LaundryPOS.Models;
using System.Security.Cryptography.X509Certificates;

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
            employee.SetPassword(txtPassword.Text);

            await CreateEmployee(employee);
        }

        public async Task CreateEmployee(Employee employee)
        {
            _unitOfWork.EmployeeRepo.Insert(employee);
            await _unitOfWork.SaveAsync();
        }
    }
}