using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Services;
using Microsoft.Identity.Client;
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
        private Employee _employee;

        public EmployeeView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

            InitializeComponent();
        }

        private async void EmployeeView_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            await DisplayEmployees();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm(_unitOfWork).ShowDialog();
            await RefreshData();
        }

        private async Task DisplayEmployees()
        {
            var employeeList = await _unitOfWork.EmployeeRepo
                .Get(filter: e => e.IsActive);
            employeeTable.DataSource = employeeList;

            HideUnwantedColumns();
            ConfigureImageColumn();
            HandleImageColumnFormatting();
        }

        private void HideUnwantedColumns()
        {
            employeeTable.Columns[nameof(Employee.EmployeeId)].Visible = false;
            employeeTable.Columns[nameof(Employee.PicPath)].Visible = false;
            employeeTable.Columns[nameof(Employee.HashedPassword)].Visible = false;
            employeeTable.Columns[nameof(Employee.Salt)].Visible = false;
            employeeTable.Columns[nameof(Employee.IsActive)].Visible = false;
        }

        private void ConfigureImageColumn()
        {
            var imageColumn = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            employeeTable.Columns.Add(imageColumn);
            employeeTable.Columns["Image"].DisplayIndex = 0;
        }

        private void HandleImageColumnFormatting()
        {
            employeeTable.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == employeeTable.Columns["Image"].Index && e.RowIndex >= 0)
                {
                    var rowData = employeeTable.Rows[e.RowIndex].DataBoundItem as Employee;
                    var imagePath = rowData?.PicPath;
                    e.Value = !string.IsNullOrEmpty(imagePath)
                        ? Image.FromFile(imagePath)
                        : null;
                }
            };
        }

        private async Task RefreshData()
        {
            employeeTable.DataSource = null;
            await DisplayEmployees();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnEmployee);
            await _themeManager.ApplyLighterThemeToDataGridView(employeeTable);
        }

        private void ChangeAdminView<T>(Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _themeManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new CategoryView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new ItemView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new TransactionView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new AdminProfileView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_employee != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _employee.IsActive = false;
                    _unitOfWork.EmployeeRepo.Update(_employee);
                    await _unitOfWork.SaveAsync();

                    MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void employeeTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = employeeTable.Rows[e.RowIndex];
                var employeeId = (selectedRow.DataBoundItem as Employee)?.EmployeeId;
                _employee = await _unitOfWork.EmployeeRepo
                    .GetByID(employeeId.Value);

                lblName.Text = _employee.FullName;
                lblUsername.Text = _employee.Username;
                lblAge.Text = _employee.Age.ToString();
                lblBirthday.Text = _employee.BirthDate.ToShortDateString();
                imgPic.Image = Image.FromFile(_employee.PicPath ?? "./default.png");

                btnDelete.Enabled = true;
            }
        }
    }
}