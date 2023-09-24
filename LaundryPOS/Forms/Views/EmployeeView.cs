using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Services;
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
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;

        public EmployeeView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

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

            HideUnwantedColumns();
            ConfigureImageColumn();
            HandleImageColumnFormatting();
        }

        private void HideUnwantedColumns()
        {
            dgvService.Columns[nameof(Employee.EmployeeId)].Visible = false;
            dgvService.Columns[nameof(Employee.PicPath)].Visible = false;
            dgvService.Columns[nameof(Employee.HashedPassword)].Visible = false;
            dgvService.Columns[nameof(Employee.Salt)].Visible = false;
            dgvService.Columns[nameof(Employee.IsActive)].Visible = false;
        }

        private void ConfigureImageColumn()
        {
            var imageColumn = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvService.Columns.Add(imageColumn);
            dgvService.Columns["Image"].DisplayIndex = 0;
        }

        private void HandleImageColumnFormatting()
        {
            dgvService.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == dgvService.Columns["Image"].Index && e.RowIndex >= 0)
                {
                    var rowData = dgvService.Rows[e.RowIndex].DataBoundItem as Item;
                    var imagePath = rowData?.PicPath;
                    e.Value = !string.IsNullOrEmpty(imagePath)
                        ? Image.FromFile(imagePath)
                        : null;
                }
            };
        }

        private async Task RefreshData()
        {
            dgvService.DataSource = null;
            await DisplayServices();
        }
    }
}