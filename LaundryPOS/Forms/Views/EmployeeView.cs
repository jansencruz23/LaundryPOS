using LaundryPOS.Contracts;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.Views
{
    public partial class EmployeeView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeView(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
            InitializeAsync();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm(_unitOfWork).ShowDialog();
            await RefreshData();
        }

        private async void InitializeAsync()
        {
            await DisplayServices();
        }

        private async Task DisplayServices()
        {
            var employeeList = await _unitOfWork.EmployeeRepo.Get();
            dgvService.DataSource = employeeList;

            dgvService.Columns[nameof(Employee.EmployeeId)].Visible = false;
            dgvService.Columns[nameof(Employee.PicPath)].Visible = false;
            dgvService.Columns[nameof(Employee.HashedPassword)].Visible = false;
            dgvService.Columns[nameof(Employee.Salt)].Visible = false;
            dgvService.Columns[nameof(Employee.IsActive)].Visible = false;

            // For image
            // dgvService.Columns.Add(nameof(Service.Name), nameof(Service.Name));
        }

        private async Task RefreshData()
        {
            dgvService.DataSource = null;
            await DisplayServices();
        }
    }
}